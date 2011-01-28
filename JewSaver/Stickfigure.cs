using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Stickfigure
{
    private Vector2 position;
    private bool moving = false;

    private Vector2 crotch, shoulder, lHand, rHand, lFoot, rFoot, neck, head;
    private float roShoulder, roLHand, roRHand, roLFoot, roRFoot;

    private int headSize;
    private int scale;

    Vector2[] points;

    protected Vector2 velocity;
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

        setLimbs(0f);

        this.velocity = Vector2.Zero;
        this.mass = 1.0f;
        moving = true;
    }

    // Update Method
    public void update(float dt)
    {
        if (!moving)
            return;

        Vector2 accel = new Vector2(0.0f, -100.0f) / mass;
        if (position.Y < JewSaver.height / 2.0f)
            accel *= -1f;
        velocity += accel * dt;

        velocity *= 0.99f;

        position += velocity * dt;

        setLimbs(dt);
    }


    private double change = 0;
    private void setLimbs(float dt)
    {
        change += dt;
        crotch = position;
        shoulder = crotch + new Vector2(0, -8 * scale) + Vector2.Multiply(new Vector2(4,0), (float)Math.Cos(change));
        neck = shoulder + new Vector2(0, -2 * scale);
        head = neck + new Vector2(0, -headSize * scale);
        lHand = shoulder + new Vector2(5 * scale, 0);
        rHand= shoulder + new Vector2(-5 * scale, 0);
        rFoot = crotch + new Vector2(-3 * scale, 6 * scale);
        lFoot = crotch + new Vector2(3 * scale, 6 * scale);

    }

    private int vecComp(Vector2 x, Vector2 y)
    {
        return x.Y.CompareTo(y.Y);
    }

    public Vector2 lowestPoint()
    {
        List<Vector2> l = new List<Vector2>(6);
        l.Add(crotch); l.Add(shoulder);
        l.Add(lHand); l.Add(rHand);
        l.Add(lFoot); l.Add(rFoot);
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
