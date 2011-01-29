using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Stickfigure
{
    const float gravity = 100.0f;


    private bool isPlayer = false;
    private bool moving = false;
    private bool dead = false;

    private Vector2 crotch, shoulder, lHand, rHand, lFoot, rFoot, neck, head;

    private int headSize;
    private int scale;

    private Vector2 position;
    protected Vector2 velocity;
    protected float moveForce;
    protected float mass;

    public Stickfigure(Vector2 position)
    {
        shoulder = new Vector2(0, -10);
        lHand = rHand = new Vector2(5, 0);
        lFoot = rFoot = new Vector2(2, 7);

        this.scale = 2;
        headSize = 3;

        setLimbs(0.0f);

        this.position = position;
        this.velocity = Vector2.Zero;
        this.moveForce = 50.0f;
        this.mass = 1.0f;
        moving = true;
    }

    public void SetIsPlayer()
    {
        isPlayer = true;
    }

    // Update Method
    public void update(float dt, float[] heightmap)
    {
        if (!moving || dead)
            return;

        // Set the initial force value
        Vector2 force = new Vector2(0.0f, gravity);

        // Get the ground value at stickies x
        float ground;

        float gp = JewSaver.height - heightmap[(int)position.X - scale];
        float gn = JewSaver.height - heightmap[(int)position.X + scale];

        setLimbs(dt);
        if (gn < gp)
        {
            // Gradient = \
            ground = JewSaver.height - heightmap[(int)lFoot.X];
        }
        else
        {
            // Gradient = /
            ground = JewSaver.height - heightmap[(int)rFoot.X];
        }
        double angle = Math.Atan((double)Math.Abs(gn - gp) / (double)(scale * 2));

        // Basic collision response thingy
        if (lowestPoint().Y > ground)
        {
            force = -velocity * mass / dt;

            // If the impact force is higher than 75 then kill stickie
            if (Math.Abs((force / gravity).Y) >= 75.0f)
                dead = true;
            force = Vector2.Zero;
        }
        // Factor in walking if not dead
        if (!dead)
            force += new Vector2(moveForce * (float)Math.Cos(angle), moveForce * (float)Math.Sin(angle));
        // Calculate the acceleration value
        Vector2 accel = force / mass;
        // Add the acceleration to velocity and factor in dt
        velocity += accel * dt;
          // Apply drag 
        velocity *= 0.99f;
        // Update the position
        position += velocity * dt;

        // Keep stickie above ground
        float diff = ground - lowestPoint().Y;
        if (diff < 0)
            position.Y += diff;

        if (dead)
            position.Y = ground;

        setLimbs(dt);
    }


    private double change = 0;
    private void setLimbs(float dt)
    {
        change += dt;

        crotch = position;
        rFoot = crotch + new Vector2(-2.5f * scale, 6 * scale) + Vector2.Multiply(new Vector2(2.5f * scale, 0), (float)Math.Cos(change));
        lFoot = crotch + new Vector2(2.5f * scale, 6 * scale) + Vector2.Multiply(new Vector2(-2.5f * scale, 0), (float)Math.Cos(change));
    
        crotch -= Vector2.Multiply(new Vector2(0, 1.5f * scale), (float)Math.Cos(change) + 1);

        //crotch = position + Vector2.Multiply(new Vector2(0, crotchUpAsLegsExtend * scale), (float)Math.Sin(change));

        shoulder = crotch + new Vector2(0, -8 * scale) + Vector2.Multiply(new Vector2(4,0), (float)Math.Cos(change));
        neck = shoulder + new Vector2(0, -2 * scale);
        head = neck + new Vector2(0, -headSize * scale);

        lHand = shoulder + new Vector2(5 * scale, 0);
        rHand= shoulder + new Vector2(-5 * scale, 0);
        rFoot = crotch + new Vector2(-2.5f * scale, 5 * scale);// + Vector2.Multiply(new Vector2(2.5f * scale, - crotchUpAsLegsExtend * scale), (float)Math.Cos(change));
        lFoot = crotch + new Vector2(2.5f * scale, 5 * scale); //+ Vector2.Multiply(new Vector2(-2.5f * scale, - crotchUpAsLegsExtend * scale), (float)Math.Cos(change));

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
        JewSaver.primitiveBatch.DrawCircle(head, Color.Blue, headSize * scale);
        
        // Begin primitive batch
        JewSaver.primitiveBatch.Begin(PrimitiveType.LineList);

        // Draw the main body
        JewSaver.primitiveBatch.AddLine(crotch, neck, Color.Red, Color.Yellow, scale);

        // Draw the arms
        JewSaver.primitiveBatch.AddLine(shoulder, lHand, Color.Yellow, Color.Yellow, scale);
        JewSaver.primitiveBatch.AddLine(shoulder, rHand, Color.Yellow, Color.Yellow, scale);
        
        // Draw the feet
        JewSaver.primitiveBatch.AddLine(crotch, lFoot, Color.Yellow, Color.Yellow, scale);
        JewSaver.primitiveBatch.AddLine(crotch, rFoot, Color.Yellow, Color.Yellow, scale);

        // End primitive batch
        JewSaver.primitiveBatch.End();
    }
}
