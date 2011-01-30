using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Locust
{
    public Vector2 pos;
    private float height;
    public bool dead, splat;

    public Locust(Vector2 pos)
    {
        height = pos.Y;
        this.pos = pos;
        dead = splat = false;
    }

    double change = 0;

    public void draw()
    {
        if (!splat)
        {
            JewSaver.primitiveBatch.AddLine(pos, pos + new Vector2(10, 0), Color.DarkOliveGreen, 3);
            JewSaver.primitiveBatch.AddLine(pos + new Vector2(3, 0), pos + new Vector2(8 + (float)Math.Sin(change * 2) * 4, -6), Color.DarkOliveGreen, 2);
        }
        else
        {
            JewSaver.primitiveBatch.AddLine(new Vector2(pos.X, pos.Y), new Vector2(pos.X, pos.Y) + new Vector2(0, 2), Color.Red, 4);
        }
    }

    public void update(float dt)
    {
        if (dead)
            return;

        if (!splat)
        {
            pos += new Vector2(((float)Math.Sin(change) + 1) * -150.0f, (float)Math.Sin(change) * 150.0f) * dt;

            if (pos.X < -10)
            {
                // Locust killed
                dead = true;
                return;
            }

            if (pos.Y > JewSaver.height - LevelBase.heightMap[Math.Min(Math.Max(0, (int)(pos.X + LevelBase.scrollX)), LevelBase.levelLength - 1)])
            {
                // SplaT
                splat = true;
            }
        }
    }
}
