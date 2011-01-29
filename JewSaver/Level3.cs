using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Level3 : LevelBase
{
    Texture2D vegasLight;
    AnimatedSprite []lights;
    public Level3(JewSaver game)
        : base(game, 4096)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        //if (!hasPlayed)
        //{
        // good canyon size is 170
        //AddCanyon(374, 374 + 170);
        //AddCanyon(1040, 1210);
        //AddTrees();
        //}
        lights = new AnimatedSprite[20];
        lights[0] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, -56, 24);
        lights[1] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, -40, 24);
        lights[2] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, -24, 24);
        lights[3] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, -8, 24);
        lights[4] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, 8, 24);
        lights[5] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, 24, 24);
        lights[6] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, 40, 24);
        lights[7] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, 56, 24);
        lights[8] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, 56, 8);
        lights[9] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, 56, -8);
        lights[10] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, 56, -24);
        lights[11] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, 40, -24);
        lights[12] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, 24, -24);
        lights[13] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, 8, -24);
        lights[14] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, -8, -24);
        lights[15] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, -24, -24);
        lights[16] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, -40, -24);
        lights[17] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, -56, -24);
        lights[18] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, -56, -8);
        lights[19] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, -56, 8);
        for (int i = 0; i < 20; i++)
            lights[i].EnableAnimation(2, 2, 12);
        hasPlayed = true;
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        vegasLight = new Texture2D(Game.GraphicsDevice, 32, 32);
        Color[] data = new Color[32 * 32];
        Color[] light = CreateLight(Color.Yellow, 4) ;
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                data[i * 32 + j] = light[i * 16 + j];
            }
        }
        light = CreateLight(Color.Yellow, 6);
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                data[i * 32 + j + 16] = light[i * 16 + j];
            }
        }
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                data[(16 + i) * 32 + j + 16] = light[i * 16 + j];
            }
        }
        light = CreateLight(Color.Yellow, 8);
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                data[(16+i) * 32 + j] = light[i * 16 + j];
            }
        }
        vegasLight.SetData<Color>(data);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        foreach (AnimatedSprite light in lights)
            light.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        JewSaver.spriteBatch.Begin();
        foreach(AnimatedSprite light in lights)
            light.Draw(JewSaver.spriteBatch);
        JewSaver.spriteBatch.End();
    }

    protected override void NextLevel()
    {
        jewSaver.SwitchState(GameState.MAIN_MENU);
    }

    private Color[] CreateLight(Color colour,float radius)
    {
        Color[] light = new Color[16 * 16];
        float radius2 = radius * radius;
        for (int i = -8; i < 8; i++)
        {
            for (int j = -8; j < 8; j++)
            {
                float dist2 = i*i+j*j;
                if (dist2 <= radius2)
                    light[(i+8) * 16 + j + 8] = new Color(colour.R/255.0f, colour.G/255.0f, colour.B/255.0f, (radius2 - dist2) / radius2);
                else
                    light[(i+8) * 16 + j + 8] = Color.TransparentWhite;
            }
        }
        return light;
    }
}
