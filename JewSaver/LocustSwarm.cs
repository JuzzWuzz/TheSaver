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
        public bool dead;

        public Locust(Vector2 pos, LocustSwarm swarm)
        {
            this.swarm = swarm;
            height = pos.Y;
            this.pos = pos;
            speed = (float)(r.NextDouble() - .5);
            dead = false;
        }

        public void draw() {
            JewSaver.primitiveBatch.AddLine(pos, pos + new Vector2(10, 0), Color.DarkOliveGreen, 3);
        }

        double change = 0;
       // public void update(float dt) {
       //     pos += new Vector2((float)Math.Sin(change) * -2f, height + (float)Math.Sin(change) * 5f);
        public void update(float dt) { 
            pos += new Vector2(((float)Math.Sin(change) + 1)* -2f, (float)Math.Sin(change) * 15f);
            Console.WriteLine("pos: " + pos.X + " pos: " + pos.Y);
            if (pos.X < 0)
            {
                Console.WriteLine("Locust destructing");
                dead = true;
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
        while (timeToLive > 0 && timeOut > .25)
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

        Console.WriteLine("Spawned at: " + locusts.Last<Locust>().pos.Y+"timeToLive: "+timeToLive);

    }
}
