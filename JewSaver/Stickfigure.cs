using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

class Stickfigure
{
    const float gravity = 500.0f;
    const float jumpForce = gravity * 30.0f;
    const double jumpAngle = Math.PI / 180.0f * 55.0f;
    
    
    private bool isPlayer = false;
    private bool newStickie;
    private bool inactive;
    
    public bool dead;
    public bool saved;
    public bool jumping;
    public bool sprinting;

    private Vector2 crotch, shoulder, lHand, rHand, lFoot, rFoot, neck, head;

    private int headSize;
    private int scale;

    public Vector2 position;
    private Vector2 origPosition;
    protected Vector2 velocity;
    protected float moveForce;
    protected float mass;
    protected float timer;
    protected float spawnTimer;
    protected Color color;
    protected int curJumpIdx;
    protected int curSprintIdx;
    protected int stickieIndex;

    public Stickfigure(Vector2 position, int index)
    {
        origPosition = position;
        color = Color.Yellow;
        stickieIndex = index;
        Initialize();
    }

    public void SetIsPlayer()
    {
        isPlayer = true;
        color = Color.Brown;
        mass = 1.2f;
    }

    public void Initialize()
    {
        shoulder = new Vector2(0, -10);
        lHand = rHand = new Vector2(5, 0);
        lFoot = rFoot = new Vector2(2, 7);

        this.scale = 2;
        headSize = 3;

        setLimbs(0.0f);

        this.position = this.origPosition;
        this.velocity = Vector2.Zero;
        this.moveForce = 50.0f;
        this.mass = 1.0f + 1.0f * (float)LevelBase.random.NextDouble();
        this.timer = 0.0f;
        this.spawnTimer = 0.0f;
        this.curJumpIdx = 0;
        this.curSprintIdx = 0;
        this.inactive = false;
        this.dead = false;
        this.saved = false;
        this.jumping = false;
        this.sprinting = false;
        this.newStickie = true;

        if (isPlayer)
            mass = 1.2f;
    }
    
    // Update Method
    public void update(float dt, float[] heightmap)
    {
        if (inactive)
            return;

        // If a new stickie make invulnarable when starting and spawn one after the other
        if (newStickie)
        {
            spawnTimer += dt;
            dead = false;
            if (spawnTimer > 0.75f * stickieIndex)
            {
                spawnTimer = 0.0f;
                newStickie = false;
            }
        }

        // Kill falled stickies
        if (position.Y > JewSaver.height)
        {
            dead = true;
            inactive = true;
            return;
        }

        // Save stickes that make it to the end
        if (position.X > JewSaver.width + 5 * scale)
        {
            saved = true;
            inactive = true;
            return;
        }
        
        // If not moving or dead then do this until stickie has moved off of screen
        if (dead)
        {
            setLimbs(dt);
            if (position.X < -5 * scale)
                inactive = true;
            return;
        }

        // Set the initial force value
        Vector2 force = new Vector2(0.0f, gravity);

        // Get the ground value at stickies x
        float ground;
        float drag = 0.99f;

        float gp = JewSaver.height - heightmap[Math.Min(Math.Max(0, (int)(LevelBase.scrollX + position.X - scale)), LevelBase.levelLength - 1)];
        float gn = JewSaver.height - heightmap[Math.Min(Math.Max(0, (int)(LevelBase.scrollX + position.X + scale)), LevelBase.levelLength - 1)];
        double angle = Math.Atan((double)Math.Abs(gn - gp) / (double)(scale * 2));
        ground = JewSaver.height - heightmap[Math.Min(Math.Max(0, (int)(LevelBase.scrollX + position.X)), LevelBase.levelLength - 1)];

        setLimbs(dt);
        if (!jumping)
        {
            if (gn > gp)
            {
                // Gradient = \
                //ground = JewSaver.height - heightmap[(int)LevelBase.scrollX + (int)rFoot.X];
                angle -= Math.PI / 36;
            }
            else
            {
                // Gradient = /
                //ground = JewSaver.height - heightmap[(int)LevelBase.scrollX + (int)lFoot.X];
                if (angle > Math.PI / 8.0f * 3.0f)
                {
                    //drag = 0.0f;
                    // Add dt to the timer value and if they are stuck for 4 seconds then kill them
                    timer += dt;
                    if (timer >= 4.0f)
                        dead = true;
                }
                else
                {
                    // Check to see if they climbing more than 22.5 degrees then start to apply
                    // the drag value based on that up to 67.5 degrees
                    float newAngle = Math.Abs((float)angle - (float)Math.PI / 8.0f);
                    drag -= 0.03f * newAngle / ((float)Math.PI / 4.0f);
                    // Minus dt from timer to reduce death chance
                    timer -= dt;
                }
            }

            // Basic collision response thingy
            if (lowestPoint().Y > ground)
            {
                force = -velocity * mass / dt;
                // If the impact force is higher than this value then kill stickie
                if (Math.Abs((force / gravity).Y) >= 50.0f)
                    dead = true;

                force = Vector2.Zero;
            }
            // Make sure the player can't die
            if (isPlayer && dead)
                dead = false;
            // Factor in walking if not dead
            if (!dead && !newStickie)
            {
                Vector2 newForce = new Vector2(moveForce * (float)Math.Cos(angle), moveForce * (float)Math.Sin(angle));
                if (sprinting)
                    newForce *= 2.0f;

                force += newForce;
            }
        }

        if (!jumping)
        {
            for (int i = curJumpIdx; i < LevelBase.jumpMarkers.Count; i++)
            {
                if (position.X + LevelBase.scrollX >= LevelBase.jumpMarkers[i])
                {
                    force = new Vector2(jumpForce * (float)Math.Cos(jumpAngle), -jumpForce * (float)Math.Sin(jumpAngle));
                    if (sprinting)
                        force *= 2.0f;
                    jumping = true;
                    curJumpIdx++;
                    timer = 0.0f;
                }
            }
        }

        if (!isPlayer)
        {
            for (int i = curSprintIdx; i < LevelBase.sprintMarkers.Count; i++)
            {
                if (position.X + LevelBase.scrollX >= LevelBase.sprintMarkers[i])
                {
                    sprinting ^= true;
                    curSprintIdx++;
                }
            }
        }

        // Calculate the acceleration value
        Vector2 accel = force / mass;
        // Add the acceleration to velocity and factor in dt
        velocity += accel * dt;
          // Apply drag 
        velocity *= drag;
        // Update the position
        position += velocity * dt;

        // Keep stickie above ground
        if (jumping)
        {
            timer += dt;
        }
        if (!jumping || (jumping && timer > 0.1f))
        {
            float diff = lowestPoint().Y - ground;
            if (diff > 0)
            {
                if (jumping)
                    velocity *= 0.1f;
                jumping = false;
                position.Y -= diff;
            }
        }

        if (dead)
            position.Y = ground;

        setLimbs(dt);
    }


    private double change = 0;
    private void setLimbs(float dt)
    {
        change += dt;

        crotch = position;
        rFoot = crotch + new Vector2(-2.5f * scale, 7 * scale) + Vector2.Multiply(new Vector2(2.5f * scale, 0), (float)Math.Cos(change));
        lFoot = crotch + new Vector2(2.5f * scale, 7 * scale) + Vector2.Multiply(new Vector2(-2.5f * scale, 0), (float)Math.Cos(change));
    
        crotch -= Vector2.Multiply(new Vector2(0, 1.5f * scale), (float)Math.Cos(change) + 1);

        shoulder = crotch + new Vector2(0, -8 * scale) + Vector2.Multiply(new Vector2(4,0), (float)Math.Cos(change * 2));
        neck = shoulder + new Vector2(0, -2 * scale);
        head = neck + new Vector2(0, -headSize * scale);

        lHand = shoulder + new Vector2(5 * scale, 0);
        rHand= shoulder + new Vector2(-5 * scale, 0);

        // If dead move all body parts to ground
        if (dead)
        {
            crotch.Y = position.Y;
            shoulder.Y = position.Y;
            neck.Y = position.Y;
            head.Y = position.Y;
            lHand.Y = position.Y;
            rHand.Y = position.Y;
            lFoot.Y = position.Y;
            rFoot.Y = position.Y;
        }
    }

    private int vecComp(Vector2 x, Vector2 y)
    {
        return (-x.Y.CompareTo(y.Y));
    }

    public Vector2 lowestPoint()
    {
        List<Vector2> l = new List<Vector2>();
        l.Add(crotch);
        l.Add(lFoot);
        l.Add(rFoot);
        l.Sort(vecComp);
        return (l[0]);
    }

    public void draw(float[] heightmap)
    {
        if (inactive)
            return;

        // Draw the head
        JewSaver.primitiveBatch.DrawCircle(head, Color.Orange, headSize * scale);
        
        // Begin primitive batch
        JewSaver.primitiveBatch.Begin(PrimitiveType.LineList);

        // Draw the main body
        JewSaver.primitiveBatch.AddLine(crotch, neck, color, color, scale);

        // Draw the arms
        JewSaver.primitiveBatch.AddLine(shoulder, lHand, color, color, scale);
        JewSaver.primitiveBatch.AddLine(shoulder, rHand, color, color, scale);
        
        // Draw the feet
        JewSaver.primitiveBatch.AddLine(crotch, lFoot, color, color, scale);
        JewSaver.primitiveBatch.AddLine(crotch, rFoot, color, color, scale);

        if (isPlayer)
            JewSaver.primitiveBatch.AddLine(crotch + new Vector2(8, 14),
                shoulder + new Vector2(8, -10), Color.Black, 2);

        // End primitive batch
        JewSaver.primitiveBatch.End();
    }
}
