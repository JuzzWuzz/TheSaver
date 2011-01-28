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

    //VertexPositionColor[] points;

    protected Vector2 velocity;
    protected float mass;

    public Stickfigure(Vector2 position)
    {
        this.position = position;
        roShoulder = roLHand = roRHand = roLFoot = roRFoot = 0f;

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
    }

    private void setLimbs()
    {
        crotch = position;
        //shoulder = position + Vector2.Dot(shoulder, Vector2
    }

    public void draw()
    {
        JewSaver.primitiveBatch.Begin(PrimitiveType.LineList);


        JewSaver.primitiveBatch.AddVertex(position, Color.Red);
        JewSaver.primitiveBatch.AddVertex(position + new Vector2(0, 10), Color.Yellow);


        JewSaver.primitiveBatch.End();
    }
}
