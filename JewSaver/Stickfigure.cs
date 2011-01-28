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

    private Vector2 crotch, shoulder, lHand, rHand, lFoot, rFoot;
    private float roShoulder, roLHand, roRHand, roLFoot, roRFoot;

    protected Vector2 velocity;
    protected float mass;

    public Stickfigure(Vector2 position)
    {
        this.position = position;
        roShoulder = roLHand = roRHand = roLFoot = roRFoot = 0f;

        shoulder = new Vector2(0, -10);
        lHand = rHand = new Vector2(5, 0);
        lFoot = rFoot = new Vector2(2, 7);

        setLimbs();

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

        position += velocity * dt;

        setLimbs();
    }

    private void setLimbs()
    {
        crotch = position;
        shoulder = crotch + new Vector2(0, -10);
        lHand = shoulder + new Vector2(5, 0);
        rHand= shoulder + new Vector2(-5, 0);
        rFoot = crotch + new Vector2(-2, 7);
        lFoot = crotch + new Vector2(2, 7);

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
        JewSaver.primitiveBatch.Begin(PrimitiveType.LineList);

        JewSaver.primitiveBatch.AddVertex(crotch, Color.Red);
        JewSaver.primitiveBatch.AddVertex(shoulder, Color.Yellow);

        JewSaver.primitiveBatch.AddVertex(crotch, Color.Yellow);
        JewSaver.primitiveBatch.AddVertex(lFoot, Color.Yellow);
        JewSaver.primitiveBatch.AddVertex(crotch, Color.Yellow);
        JewSaver.primitiveBatch.AddVertex(rFoot, Color.Yellow);

        JewSaver.primitiveBatch.AddVertex(shoulder, Color.Yellow);
        JewSaver.primitiveBatch.AddVertex(lHand, Color.Yellow);
        JewSaver.primitiveBatch.AddVertex(shoulder, Color.Yellow);
        JewSaver.primitiveBatch.AddVertex(rHand, Color.Yellow);

        JewSaver.primitiveBatch.End();
    }
}
