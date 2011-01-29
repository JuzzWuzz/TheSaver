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
    private bool jumping = false;

    static List<float> jumpMarkers = new List<float>();

    private bool isPlayer = false;
    private bool moving = false;
    private bool dead = false;

    private Vector2 crotch, shoulder, lHand, rHand, lFoot, rFoot, neck, head;

    private int headSize;
    private int scale;

    public Vector2 position;
    private Vector2 origPosition;
    protected Vector2 velocity;
    protected float moveForce;
    protected float mass;
    protected float timer;
    protected Color color;

    public Stickfigure(Vector2 position)
    {
        origPosition = position;
        color = Color.Yellow;
        Initialize();
    }

    public void SetIsPlayer()
    {
        isPlayer = true;
        color = Color.Brown;
        jumpMarkers.Clear();
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
        this.mass = 1.0f;
        this.timer = 0.0f;
        this.moving = true;
    }
    
    // Update Method
    public void update(float dt, float[] heightmap, float scrollX)
    {
        if (!moving || dead)
            return;

        // Set the initial force value
        Vector2 force = new Vector2(0.0f, gravity);

        // Get the ground value at stickies x
        float ground;
        float drag = 0.99f;

        float gp = JewSaver.height - heightmap[(int)scrollX + (int)position.X - scale];
        float gn = JewSaver.height - heightmap[(int)scrollX + (int)position.X + scale];
        double angle = Math.Atan((double)Math.Abs(gn - gp) / (double)(scale * 2));

        setLimbs(dt);
        if (gn > gp)
        {
            // Gradient = \
            ground = JewSaver.height - heightmap[(int)scrollX + (int)rFoot.X];
            angle -= Math.PI / 36;
        }
        else
        {
            // Gradient = /
            ground = JewSaver.height - heightmap[(int)scrollX + (int)lFoot.X];
            if (angle > Math.PI / 8.0f * 3.0f)
            {
                drag = 0.0f;
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
            if (Math.Abs((force / gravity).Y) >= 40.0f)
                dead = true;
            
            force = Vector2.Zero;
        }
        // Make sure the player can't die
        if (isPlayer && dead)
            dead = false;
        // Factor in walking if not dead
        if (!dead)
            force += new Vector2(moveForce * (float)Math.Cos(angle), moveForce * (float)Math.Sin(angle));

        if (isPlayer && !jumping && Input.spaceBarDown)
        {
            force += new Vector2(1, -1) * moveForce * 50;
            jumping = true;
            jumpMarkers.Add(position.X + LevelBase.scrollX);
        }
        if (!isPlayer && !jumping)
            foreach (float marker in jumpMarkers)
                if (position.X + LevelBase.scrollX - marker > 1 && position.X + LevelBase.scrollX - marker < 3)
                {
                    force += new Vector2(1, -1) * moveForce * 50;
                    jumping = true;
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
        float diff = ground - lowestPoint().Y;
        if (diff < 0)
        {
            jumping = false;
            position.Y += diff;
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
        l.Add(neck);
        l.Add(crotch);
        l.Add(shoulder);
        l.Add(lHand);
        l.Add(rHand);
        l.Add(lFoot);
        l.Add(rFoot);
        l.Sort(vecComp);
        return (l[0]);
    }

    public void draw()
    {
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
