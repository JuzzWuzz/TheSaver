using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Stickfigure
{
    const float gravity = 100.0f;


    private Vector2 position;
    private bool moving = false;
    private bool dead = false;

    private Vector2 crotch, shoulder, lHand, rHand, lFoot, rFoot, neck, head;
    private float roShoulder, roLHand, roRHand, roLFoot, roRFoot;

    private int headSize;
    private int scale;

    protected Vector2 velocity;
    protected float moveForce;
    protected float mass;

    public Stickfigure(Vector2 position)
    {
        this.position = position;
        roShoulder = roLHand = roRHand = roLFoot = roRFoot = 0f;

        shoulder = new Vector2(0, -10);
        lHand = rHand = new Vector2(5, 0);
        lFoot = rFoot = new Vector2(2, 7);

        this.scale = 5;
        headSize = 3;

        setLimbs();

        this.velocity = Vector2.Zero;
        this.moveForce = 50.0f;
        this.mass = 1.0f;
        moving = true;
    }

    // Update Method
    public void update(float dt)
    {
        if (!moving || dead)
            return;

        // Set the initial force value
        Vector2 force = new Vector2(0.0f, gravity);

        // Basic collision response thingy
        if (lowestPoint().Y > JewSaver.height / 2.0f)
        {
            force = -velocity * mass / dt;
            if (force.Y != 0.0f)
                Console.WriteLine((force / gravity).ToString());

            // If the impact force is higher than 75 then kill stickie
            if (Math.Abs((force / gravity).Y) >= 75.0f)
                dead = true;
        }
        // Factor in walking
        if (!dead)
            force.X = moveForce;
        // Calculate the acceleration value
        Vector2 accel = force / mass;
        // Add the acceleration to velocity and factor in dt
        velocity += accel * dt;
        // Apply drag 
        velocity *= 0.99f;
        // Update the position
        position += velocity * dt;

        setLimbs();
    }

    private void setLimbs()
    {
        crotch = position;
        shoulder = crotch + new Vector2(0, -10 * scale);
        neck = crotch + new Vector2(0, -12 * scale);
        head = neck + new Vector2(0, -headSize * scale);
        lHand = shoulder + new Vector2(4 * scale, 0);
        rHand= shoulder + new Vector2(-4 * scale, 0);
        rFoot = crotch + new Vector2(-3 * scale, 6 * scale);
        lFoot = crotch + new Vector2(3 * scale, 6 * scale);
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

        // Draw floor
        JewSaver.primitiveBatch.AddLine(new Vector2(0.0f, JewSaver.height / 2.0f), new Vector2(JewSaver.width, JewSaver.height / 2.0f), Color.Purple, 5);

        // End primitive batch
        JewSaver.primitiveBatch.End();
    }
}
