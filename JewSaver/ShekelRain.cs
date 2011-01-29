using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class ShekelRain : DrawableGameComponent
{
    Random r = new Random();

    List<string> messages;
    public static float[] hm;

    private class Schekel {
        Vector2 one, two;
        bool oneMoves, twoMoves, moving;
        public Schekel(Vector2 pos) {
            one = new Vector2(-3 + pos.X, pos.Y);
            two = new Vector2(3 + pos.X, pos.Y);
            oneMoves = twoMoves = moving = true;
        }

        public void update(float dt)
        {
            float speed = 40f;
            float offset = 3f;

            if (Vector2.Distance(one, two) < 8)
            {
                if (oneMoves)
                    if (384 - one.Y >= hm[(int)one.X] + offset)
                        one += new Vector2(0, dt * speed);
                    else
                    {
                        oneMoves = false;
                    }

                if (twoMoves) 
                    if (384 - two.Y >= hm[(int)two.X] + offset)
                        two += new Vector2(0, dt * speed);
                    else
                    {
                        twoMoves = false;
                    }
                if (moving && !oneMoves && !twoMoves)
                {
                    for (int i = (int)one.X; i <= (int)two.X; i++)
                        hm[i] += 2;
                    moving = false;
                }
            }
        }

        public void draw()
        {
            JewSaver.primitiveBatch.AddLine(one, two, Color.Gold, 2);
        }
    }

    List<Schekel> treasure;
    Schekel test;

    int sheckels;

    public ShekelRain(JewSaver game) : base(game)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        messages = new List<string>();
        messages.Add("Congratulations!");
        messages.Add("You have saved the Jews from near certaion");
        messages.Add("EXCTINCTION!");
        messages.Add("You have saved " + JewSaver.finalSavedStickies + " people.\n They are pretty thankful!");

        sheckels = 10 * JewSaver.finalSavedStickies 
            + 20 * JewSaver.finalSavedFemales 
            + 50 * JewSaver.finalSavedFatties;

        hm = new float[1024];
        for (int i = 0; i < 1024; i++)
        {
            hm[i] = LevelBase.heightMap[(int)LevelBase.scrollX + i];
        }
        //LevelBase.heightMap.CopyTo(hm, (int)LevelBase.scrollX);

        test = new Schekel(new Vector2(400, 0));
        treasure = new List<Schekel>();

    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        JewSaver.spriteBatch.Begin();
        
        JewSaver.spriteBatch.DrawString(LevelBase.font, messages[0], new Vector2(300, 150), Color.White, 0, 0.5f * LevelBase.font.MeasureString("VEGAS"), 1, SpriteEffects.None, 0);

        JewSaver.spriteBatch.End();

        JewSaver.primitiveBatch.Begin(PrimitiveType.LineList);
        test.draw();
        foreach (Schekel s in treasure)
            s.draw();
        JewSaver.primitiveBatch.End();

        JewSaver.primitiveBatch.Begin(PrimitiveType.PointList);
        for (int i = 0; i < 1024; i++)
            JewSaver.primitiveBatch.AddVertex(new Vector2(i, hm[i]), Color.Red);
        JewSaver.primitiveBatch.End();
    
    }

    float timeout;
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        test.update((float)gameTime.ElapsedGameTime.TotalSeconds);
        timeout -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (timeout < 0 && treasure.Count <= sheckels * 10)
        {
            timeout += .05f;
            int pos = 0;
            for (int i = 0; i < 3; i++)
                pos += r.Next(0, 1024);
            pos /= 3;
            treasure.Add(new Schekel(new Vector2(pos,0)));
        }
        foreach (Schekel s in treasure)
            s.update((float)gameTime.ElapsedGameTime.TotalSeconds);
    }
}
