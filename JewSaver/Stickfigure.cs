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

    protected Vector2 velocity;
    protected float mass;

    public Stickfigure(Vector2 position)
    {
        this.position = position;
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
            accel *= -0.1f;
        velocity += accel * dt;

        position += velocity * dt;
    }

    // Draw method
    public void draw() {    
        VertexPositionColor[] points = {new VertexPositionColor(new Vector3(position, 0.0f), Color.White)};

        JewSaver.spriteBatch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
            PrimitiveType.PointList,
            points,
            0,
            1);
    }
}
