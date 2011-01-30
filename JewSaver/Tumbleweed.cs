using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class Tumbleweed
{
    public float radius;
    public Vector2 position;
    public float scrollXValue;
    float rotation;
    float speedLeft;
    float speedDown;
    public static Random r;
    List<BurningParticle> particles;
   
    private class BurningParticle
    {
        Color col;
        Vector2 position;
        enum ParticleType {COAL, REDHOT}
        ParticleType type;
        public bool dead;
        TimeSpan lifetime;

        public BurningParticle(Vector2 pos)
        {
            dead = false;
            lifetime = new TimeSpan(0);
            position = pos;
            int typeRand = r.Next(100);
            if (typeRand < 50)
            {
                type = ParticleType.COAL;
                col = Color.Black;
            }
            else
            {
                type = ParticleType.REDHOT;
                col = Color.Red;
            }
        }

        public void update(GameTime gt) { 
            lifetime += gt.ElapsedGameTime;
            position += new Vector2(0, -35 * (float)gt.ElapsedGameTime.TotalSeconds);
            if (type == ParticleType.COAL)
            {
                if (lifetime > new TimeSpan(0, 0, 0, 0, 250))
                    dead = true;
                col = Color.Lerp(this.col, Color.TransparentBlack, 0.15f);
            }
            else
            {
                if (lifetime > new TimeSpan(0, 0, 0, 0, 150))
                    dead = true;
                if (lifetime < new TimeSpan(0, 0, 0, 0, 100))
                    col = Color.Lerp(this.col, Color.Yellow, 0.1f);
                else col = Color.Lerp(this.col, new Color(Color.Yellow,0f), 0.2f);
            }
        }

        public void draw() {
            JewSaver.primitiveBatch.AddVertex(new Vector2(position.X - LevelBase.scrollX, position.Y), col);
        }

    }

    public Tumbleweed(float radius, Vector2 pos)
    {
        this.radius = radius;
        position = pos;
        rotation = 0;
        speedLeft = 240.0f;
        speedDown = 0;
        r = new Random();
        particles = new List<BurningParticle>();
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

        for (int i = 0; i < 150; i++)
            particles.Add(new BurningParticle(
                new Vector2(
                    (r.Next((int)position.X - (int)radius, (int)position.X + (int)radius)
                    +r.Next((int)position.X - (int)radius, (int)position.X + (int)radius)
                    +r.Next((int)position.X - (int)radius, (int)position.X + (int)radius))/3,
                    (r.Next((int)position.Y - (int)radius, (int)position.Y + (int)radius)
                    + r.Next((int)position.Y - (int)radius, (int)position.Y + (int)radius)
                    + r.Next((int)position.Y - (int)radius, (int)position.Y + (int)radius))/3)));
        foreach (BurningParticle particle in particles)
            particle.update(gameTime);
        for (int i = 0; i < particles.Count;)
            if (particles[i].dead)
            { particles.RemoveAt(i); }
            else
                i++;
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

        JewSaver.primitiveBatch.Begin(PrimitiveType.PointList);
        foreach (BurningParticle particle in particles)
            particle.draw();
        JewSaver.primitiveBatch.End();
    }
}
