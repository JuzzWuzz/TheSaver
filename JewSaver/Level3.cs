using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Level3 : LevelBase
{
    Texture2D vegasLight;
    Texture2D signBack;
    AnimatedSprite []lights;
    Sprite sign;
    public Level3(JewSaver game)
        : base(game, 4096)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        if (!hasPlayed)
        {   
            int increment = 512;
            for (int i = 0; i < 4096; i += increment)
            {
                if (i != 0 && i != 4048)
                {
                    int halfWidth = (int)(0.4f * increment / 2);
                    AddRocks(i - halfWidth, i + halfWidth);
                }
                if (increment > 0)
                    increment -= 24;
            }
        }
        lights = new AnimatedSprite[20];
        lights[0] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, -56 + 3064 + 384, 24 - 48);
        lights[1] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, -40 + 3064 + 384, 24-48);
        lights[2] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, -24 + 3064 + 384, 24-48);
        lights[3] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, -8 + 3064 + 384, 24-48);
        lights[4] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, 8 + 3064 + 384, 24-48);
        lights[5] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, 24 + 3064 + 384, 24-48);
        lights[6] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, 40 + 3064 + 384, 24-48);
        lights[7] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, 56 + 3064 + 384, 24-48);
        lights[8] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, 56 + 3064 + 384, 8-48);
        lights[9] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, 56 + 3064 + 384, -8-48);
        lights[10] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, 56 + 3064 + 384, -24-48);
        lights[11] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, 40 + 3064 + 384, -24-48);
        lights[12] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, 24 + 3064 + 384, -24-48);
        lights[13] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, 8 + 3064 + 384, -24-48);
        lights[14] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, -8 + 3064 + 384, -24-48);
        lights[15] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, -24 + 3064 + 384, -24-48);
        lights[16] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, -40 + 3064 + 384, -24-48);
        lights[17] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, -56 + 3064 + 384, -24-48);
        lights[18] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, -56 + 3064 + 384, -8-48);
        lights[19] = new AnimatedSprite(vegasLight, 16, 16, 0, 0, -56 + 3064 + 384, 8-48);
        for (int i = 0; i < 7; i++)
        {
            float frac1 = i / 7.0f;
            float frac2 = 1-frac1;
            lights[i].EnableAnimation(2, 2, 12);
            lights[i].Colour = new Color(frac2, frac1, 0);
        }
        for (int i = 0; i < 7; i++)
        {
            float frac1 = i / 7.0f;
            float frac2 = 1-frac1;
            lights[i+7].EnableAnimation(2, 2, 12);
            lights[i+7].Colour = new Color(0, frac2, frac1);
        }
        for (int i = 0; i < 6; i++)
        {
            float frac1 = i / 6.0f;
            float frac2 = 1-frac1;
            lights[i+14].EnableAnimation(2, 2, 12);
            lights[i+14].Colour = new Color(frac1, 0, frac2);
        }
        sign = new Sprite(signBack, 128, 64, 0,0,128,64 ,-64 + 3064 + 384 + 512+1, 16 + 192+1);
        hasPlayed = true;
        for (int i = heightMap.Length - 1; i > heightMap.Length - 256; i--)
            canSculpt[i] = TerrainType.OTHER;

        hasLocusts = true;

        openingText = true;
        finalTexts.Add("Level 3");
        finalTexts.Add("Make the final dash to paradise");
        finalTexts.Add("Watch out for locusts");
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        vegasLight = new Texture2D(Game.GraphicsDevice, 32, 32);
        Color[] data = new Color[32 * 32];
        Color[] light = CreateLight(Color.White, 4) ;
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                data[i * 32 + j] = light[i * 16 + j];
            }
        }
        light = CreateLight(Color.White, 6);
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
        light = CreateLight(Color.White, 8);
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                data[(16+i) * 32 + j] = light[i * 16 + j];
            }
        }
        vegasLight.SetData<Color>(data);
        signBack = new Texture2D(Game.GraphicsDevice, 128, 64);
        Color [] signData = new Color[128 * 64];
        for (int i = 0; i < 64; i++)
            for (int j = 0; j < 128; j++)
                signData[i * 128 + j] = Color.Black;
        signBack.SetData<Color>(signData);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (levelMode == LevelMode.PLAY)
        {
            for(int i = 0; i < lights.Length; i++)
            {
                lights[i].scrollXValue = scrollX;
                lights[i].Update(gameTime);
                lights[i].Colour = lights[i+1>19?0:i+1].Colour;
            }
        }
        sign.scrollXValue = scrollX;
    }

    public override void Draw(GameTime gameTime)
    {
        if (levelMode == LevelMode.PLAY)
        {
            JewSaver.primitiveBatch.Begin(PrimitiveType.LineList);
            JewSaver.primitiveBatch.AddLine(new Vector2(-40 + 3064 + 384+512 - scrollX, 272), new Vector2(-40 + 3064 + 384 +512- scrollX, 384 - 16), Color.Brown, Color.Black, 8);
            JewSaver.primitiveBatch.AddLine(new Vector2(40 + 3064 + 384+512 - scrollX, 272), new Vector2(40 + 3064 + 384+512 - scrollX, 384 - 16), Color.Brown, Color.Black, 8);
            JewSaver.primitiveBatch.End();
            JewSaver.spriteBatch.Begin();
            sign.Draw(JewSaver.spriteBatch);
            foreach (AnimatedSprite light in lights)
                light.Draw(JewSaver.spriteBatch);
            JewSaver.spriteBatch.DrawString(font, "VEGAS", new Vector2(sign.screenRectangle.Center.X - scrollX, sign.screenRectangle.Center.Y), Color.White, 0, 0.5f * font.MeasureString("VEGAS"),1, SpriteEffects.None, 0);
            JewSaver.spriteBatch.End();
        }
        base.Draw(gameTime);
    }

    protected override void NextLevel()
    {
        //jewSaver.SwitchState(GameState.CREDITS);
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
