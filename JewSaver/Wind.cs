using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Wind
{
    public static Random r = new Random();
    TimeSpan timeLeft;

    //giving the user access to their sand backup data
    //and allowing the level to clear the sand from drawing.
    public static List<Sand> sandCollection, deadSand;

    public class Sand
    {
        public Vector2 pos;
        public Color col;
        public bool vis;
        float alpha;

        public Sand(Vector2 pos, Color col)
        {
            init(pos, col);
        }

        public void init(Vector2 pos, Color col)
        {
            this.pos = pos;
            this.col = col;
            vis = true;
            alpha = 1f;
            this.pos = new Vector2(pos.X,
                JewSaver.height - 4
                - LevelBase.heightMap[(int)Math.Min(pos.X + LevelBase.scrollX + 1,
                                                    LevelBase.levelLength - 1)] + 2);
        }

        public void update(GameTime gt)
        {
            pos += new Vector2(
                (float)gt.ElapsedGameTime.TotalSeconds * 35f
             , 
                (float)Math.Min(
                        JewSaver.height - 4 - LevelBase.heightMap[(int)Math.Min(pos.X + LevelBase.scrollX + 1, LevelBase.levelLength - 1)] + 2
                    , 
                        - gt.ElapsedGameTime.TotalSeconds * 15f)
             );

            alpha -= (float)gt.ElapsedGameTime.TotalSeconds;
            col.A = (byte)(alpha * 255);
            if (col.A <= 0) vis = false;
        }

        public void draw() {
            JewSaver.primitiveBatch.AddVertex(new Vector2(pos.X - LevelBase.scrollX, pos.Y), col);
            JewSaver.primitiveBatch.AddVertex(new Vector2(pos.X - 1 - LevelBase.scrollX, pos.Y - 1), col);
            JewSaver.primitiveBatch.AddVertex(new Vector2(pos.X - 1 - LevelBase.scrollX, pos.Y), col);
            JewSaver.primitiveBatch.AddVertex(new Vector2(pos.X - LevelBase.scrollX, pos.Y - 1), col);
        }
    }


    public Wind(TimeSpan duration)
    {
        timeLeft = duration;
        sandCollection = new List<Sand>(300);
        deadSand = new List<Sand>(300);
    }

    static TimeSpan millisecondsBetweenSandSpawns = new TimeSpan(0, 0, 0, 0, 50);
    static TimeSpan counter = new TimeSpan(0, 0, 0);
    public void update(GameTime gt, ref float[] heightmap)
    {
        //DEBUG
        /*if (sandCollection.Count > 0)
            Console.Write(sandCollection[0].pos.X + " " 
                + sandCollection[0].pos.Y + " " + sandCollection[0].col.A);
        Console.WriteLine(" -> "+sandCollection.Count);*/

        //always
        while (deadSand.Count < 200)
            deadSand.Add(new Sand(new Vector2(), Color.CornflowerBlue));

        foreach (Sand s in sandCollection)
            s.update(gt);
        for (int i = 0; i < sandCollection.Count; )
        {
            if (sandCollection[i].vis)
                i++;
            else
            {
                deadSand.Add(sandCollection[i]);
                sandCollection.RemoveAt(i);
            }
        }

        timeLeft -= gt.ElapsedGameTime;

        if (timeLeft > new TimeSpan(0, 0, 0))
        {
            //while running
            for (int i = 0; i < LevelBase.levelLength; i++)
                if (LevelBase.canSculpt[i] == LevelBase.TerrainType.SAND && heightmap[i] > 16)
                    LevelBase.heightMap[i] -= (float)gt.ElapsedGameTime.TotalSeconds * 3;

            counter += gt.ElapsedGameTime;
            counter += gt.ElapsedGameTime;
            counter += gt.ElapsedGameTime;
            if (deadSand.Count > 0 && counter > millisecondsBetweenSandSpawns)
            {
                for (int i = 0; i < 10; i++)
                {
                    counter = new TimeSpan(0, 0, 0);
                    Sand s = deadSand[0];
                    int x = r.Next(LevelBase.levelLength);

                    s.init(new Vector2(x, LevelBase.heightMap[(int)(x)]), Color.Yellow);
                    deadSand.RemoveAt(0);
                    sandCollection.Add(s);

                }
            }
        }
    }

    public void draw()
    {
        JewSaver.primitiveBatch.Begin(PrimitiveType.PointList);
        foreach (Sand s in sandCollection)
            s.draw();
        JewSaver.primitiveBatch.End();
    }
}
