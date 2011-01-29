using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class LocustSwarm
{
    static Random r = new Random();
    public bool active = true;

    public List<Locust> locusts;

    public class Locust
    {
        public Vector2 pos;
        private float height;
        private float speed;
        LocustSwarm swarm;
        public bool dead, splat;

        public Locust(Vector2 pos, LocustSwarm swarm)
        {
            this.swarm = swarm;
            height = pos.Y;
            this.pos = pos;
            speed = (float)(r.NextDouble() - .5);
            dead = splat = false;
        }

        double change = 0;

        public void draw() {
            if (!splat)
            {
                JewSaver.primitiveBatch.AddLine(pos, pos + new Vector2(10, 0), Color.DarkOliveGreen, 3);
                JewSaver.primitiveBatch.AddLine(pos + new Vector2(3, 0),
                    pos + new Vector2(8 + (float)Math.Sin(change * 2) * 4, -6), Color.DarkOliveGreen, 2);
            }
            else
            {
                JewSaver.primitiveBatch.AddLine(new Vector2(pos.X - LevelBase.scrollX, pos.Y),
                    new Vector2(pos.X - LevelBase.scrollX, pos.Y) + new Vector2(0, 2), Color.Red, 4);
            }
        }
        
        public void update(float dt) {
            if (!splat)
            {
                pos += new Vector2(((float)Math.Sin(change) + 1) * -4f, (float)Math.Sin(change) * 15f);

                //Console.WriteLine("pos: " + pos.X + " pos: " + pos.Y);
                if (pos.X < 0)
                {
                    Console.WriteLine("Locust destructing");
                    dead = true;
                    return;
                }

                if (pos.Y > JewSaver.height - LevelBase.heightMap[Math.Min(Math.Max(0, (int)(pos.X + LevelBase.scrollX)), LevelBase.levelLength - 1)])
                {
                    Console.WriteLine("Locust Y: " + pos.Y + " HMap: " + LevelBase.heightMap[(int)pos.X]);
                    Console.WriteLine("h: " + (JewSaver.height -
                        LevelBase.heightMap[Math.Min(Math.Max(0, (int)(pos.X)), LevelBase.levelLength - 1)]));
                    splat = true; Console.WriteLine("SplaT!");
                }
            } 
        }

    }

    float timeToLive = 0;

    public LocustSwarm(float duration)
    {
        timeToLive += duration;
        locusts = new List<Locust>();
    }

    public void draw() { 
        JewSaver.primitiveBatch.Begin(PrimitiveType.LineList);

        foreach (Locust l in locusts)
            l.draw();

        JewSaver.primitiveBatch.End();
    }

    float timeOut = 0.95f;
    public void update(float dt)
    {
        timeOut += dt;
        timeToLive -= dt;
        while (timeToLive > 0 && timeOut > .35)
            spawnLocust();
        foreach (Locust l in locusts)
            l.update(dt);
        for (int i = 0; i < locusts.Count; )
        {
            if (locusts[i].dead)
            {
                locusts.Remove(locusts[i]);
            }
            else i++;
        }
    }

    private void spawnLocust()
    {
        timeOut = 0;
        int spawnZone = 192 / 3;
        int rand = r.Next(100);
        if (rand > 40)
            locusts.Add(new Locust(new Vector2(LevelBase.levelLength, r.Next(0, spawnZone)), this));
        else if (rand > 10)
            locusts.Add(new Locust(new Vector2(LevelBase.levelLength, r.Next(0, spawnZone * 2)), this));
        else
            locusts.Add(new Locust(new Vector2(LevelBase.levelLength, r.Next(0, spawnZone * 3)), this));

        Console.WriteLine("Spawned at: " + locusts.Last<Locust>().pos.Y + "  - timeToLive: "
            + timeToLive + " heightmap: " + LevelBase.heightMap[(int)locusts.Last<Locust>().pos.X-1]);

    }
}
