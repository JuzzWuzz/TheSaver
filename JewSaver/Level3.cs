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
        //if (!hasPlayed)
        //{
        // good canyon size is 170
        //AddCanyon(374, 374 + 170);
        //AddCanyon(1040, 1210);
        //AddTrees();
        //}
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
        for (int i = 0; i < 20; i++)
            lights[i].EnableAnimation(2, 2, 12);
        sign = new Sprite(signBack, 128, 64, 0,0,128,64 ,-64 + 3064 + 384 + 512+1, 16 + 192+1);
        hasPlayed = true;
        for (int i = heightMap.Length - 1; i > heightMap.Length - 256; i--)
            canSculpt[i] = TerrainType.OTHER;
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
        //if (levelMode == LevelMode.PLAY)
        //{
            foreach (AnimatedSprite light in lights)
            {
                light.scrollXValue = scrollX;
                light.Update(gameTime);
            }
        //}
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
