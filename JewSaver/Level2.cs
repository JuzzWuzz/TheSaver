using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Level2 : LevelBase
{
    List<Tumbleweed> tumbles;
    float tumbleTimer;
    
    public Level2(JewSaver game)
        : base(game, 3076)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        tumbles = new List<Tumbleweed>();
        if (!hasPlayed)
        {
            for (int i = 768; i < heightMap.Length; i++)
            {
                heightMap[i] = (float)(32 + 8 * Math.Sin(i / 4096.0f * Math.PI * 24));
                canSculpt[i] = TerrainType.PARCHED_LAND;
            }
            AddWater(320, 448);
            AddWater(640, 768);
            AddTrees();
        }
        tumbleTimer = 0;
        AddTumbleWeed();
        hasPlayed = true;
    }

    protected override void NextLevel()
    {
        jewSaver.SwitchState(GameState.LEVEL_3);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (levelMode == LevelMode.PLAY)
        {
            tumbleTimer += LevelBase.gameSpeedFactor*(float)gameTime.ElapsedGameTime.TotalSeconds;
            if (tumbleTimer > 20)
            {
                tumbleTimer -= 3;
                AddTumbleWeed();
            }
            List<Tumbleweed> toBeRemoved = new List<Tumbleweed>();
            foreach (Tumbleweed weed in tumbles)
            {
                if (weed.position.X - 4 < 0 || weed.position.Y > 384 + 10)
                    toBeRemoved.Add(weed);
                else
                {
                    weed.Update(gameTime, heightMap[(int)weed.position.X - 4], heightMap[(int)weed.position.X + 4], canSculpt[(int)(weed.position.X)] == TerrainType.WATER || canSculpt[(int)(weed.position.X)] == TerrainType.CANYON);
                    weed.scrollXValue = scrollX;
                }
                int bucket1 = (int)((weed.position.X-weed.radius) / (heightMap.Length / 128));
                int bucket2 = (int)((weed.position.X + weed.radius) / (heightMap.Length / 128));
                if (bucket1 > -1)
                {
                    for (int i = 0; i < collisionBuckets[bucket1].Count; i++)
                    {
                        Stickfigure stick = collisionBuckets[bucket1][i];
                        if (!stick.dead)
                        {
                            double dist2 = Math.Pow(stick.position.X + scrollX - weed.position.X, 2) + Math.Pow(stick.position.Y - weed.position.Y, 2);
                            if (dist2 < weed.radius*weed.radius)
                                stick.dead = true;
                        }
                    }
                }
                if (bucket1 != bucket2 && bucket2 < 128)
                {
                    for (int i = 0; i < collisionBuckets[bucket2].Count; i++)
                    {
                        Stickfigure stick = collisionBuckets[bucket2][i];
                        if (!stick.dead)
                        {
                            double dist2 = Math.Pow(stick.position.X + scrollX - weed.position.X, 2) + Math.Pow(stick.position.Y - weed.position.Y, 2);
                            if (dist2 < weed.radius * weed.radius)
                                stick.dead = true;
                        }
                    }
                }
            }
            foreach (Tumbleweed weed in toBeRemoved)
                tumbles.Remove(weed);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        if (levelMode == LevelMode.PLAY)
        {
            foreach (Tumbleweed weed in tumbles)
                weed.Draw();
        }
    }

    private void AddTumbleWeed()
    {
        int radius= random.Next(12, 20);
        tumbles.Add(new Tumbleweed(radius, new Vector2(3076-5, 384 - (heightMap[1576] + radius))));
    }
}
