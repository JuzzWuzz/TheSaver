using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Tumbleweed
{
    public float radius;
    public Vector2 position;
    public float scrollXValue;
    float rotation;
    float speedLeft;
    float speedDown;   

    public Tumbleweed(float radius, Vector2 pos)
    {
        this.radius = radius;
        position = pos;
        rotation = 0;
        speedLeft = 240.0f;
        speedDown = 0;
    }

    public void Update(GameTime gameTime, float heightLeft, float heightRight, bool isAtHole)
    {
        Vector2 gradient = new Vector2(-8,heightRight - heightLeft);
        gradient.Normalize();
        Vector2 velocity = new Vector2(speedLeft * -1, speedDown * 1);
        velocity.Normalize();
        position += 240.0f * velocity  * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (384 - position.Y - radius > (heightLeft + heightRight) / 2.0f || isAtHole)
        {
            speedDown += 0.5f;
            rotation += 0.5f * speedLeft * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (isAtHole)
                speedDown += 100;
            speedLeft += (float)(gradient.Y * gameTime.ElapsedGameTime.TotalSeconds * 120.0f);
        }
        else
        {
            position.Y = 384 - (heightLeft + heightRight) / 2.0f - radius;
            speedDown = 0;
            rotation += 1.5f * speedLeft * (float)gameTime.ElapsedGameTime.TotalSeconds;
            speedLeft += (float)(gradient.Y * gameTime.ElapsedGameTime.TotalSeconds * 240.0f);
        }
    }

    public void Draw()
    {
        JewSaver.primitiveBatch.Begin(PrimitiveType.LineList);
        Vector2 screenPos = position + new Vector2(-scrollXValue,0);
        for (int i = 0; i < 360; i += 20)
        {
            double radians = (i+ rotation)/ 360.0f * Math.PI * 2;
            float sin = (float)(radius * Math.Sin(radians));
            float cos = (float)(radius * Math.Cos(radians));
            JewSaver.primitiveBatch.AddLine(screenPos, screenPos + new Vector2(sin, cos), Color.Brown, Color.Orange, 2);
        }
        JewSaver.primitiveBatch.End();
    }
}
