using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


public class Schekel
{
    Vector2 one, two;
    bool oneMoves, twoMoves, moving;
    public Schekel(Vector2 pos)
    {
        one = new Vector2(-3 + pos.X, pos.Y);
        two = new Vector2(3 + pos.X, pos.Y);
        oneMoves = twoMoves = moving = true;
    }

    public void update(float dt)
    {
        float speed = 100f;
        float offset = 2f;

        if (moving && Vector2.Distance(one, two) < 8)
        {
            if (oneMoves)
            {
                one += new Vector2(0, dt * speed);

                for (int i = -3; i < 0; i++)
                    if (JewSaver.height - one.Y < LevelBase.hm[(int)one.X + i] + offset)
                    {
                        oneMoves = false;
                        break;
                    }
            }

            if (twoMoves)
            {

                two += new Vector2(0, dt * speed);

                for (int i = 0; i <= 3; i++)
                    if (JewSaver.height - two.Y < LevelBase.hm[(int)two.X + i] + offset)
                    {
                        twoMoves = false;
                        break;
                    }
            }
            if (moving && !oneMoves && !twoMoves)
            {
                for (int i = (int)one.X; i <= (int)two.X; i++)
                    LevelBase.hm[i] += 2;
                moving = false;
            }
        }
        else if (moving)
        {
            for (int i = (int)one.X; i <= (int)two.X; i++)
                LevelBase.hm[i] += 2;
            moving = false;
        }
    }

    public void draw()
    {
        JewSaver.primitiveBatch.AddLine(one, two, Color.Gold, 2);
    }
}