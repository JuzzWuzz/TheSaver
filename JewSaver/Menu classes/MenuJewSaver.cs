using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class MenuJewSaver:Menu
{
    Texture2D buttonTexture;
    MenuButton newGame;
    MenuButton exit;
    public static SpriteFont fontBig;
    public static SpriteFont font;
    JewSaver jewSaver;
    Texture2D vegasLight;
    AnimatedSprite[] lights;
    SpriteFont titleFont;
    SpriteFont titleFontSmall;
    Sprite backBoard;
    Texture2D black;

    public MenuJewSaver(JewSaver game)
        : base(game)
    {
        jewSaver = game;
    }

    public override void Initialize()
    {
        base.Initialize();
        exit = new MenuButton(buttonTexture, new Point(256, 64), new Point(0, 0), new Point(256, 64), new Point(384, 284), "EXIT");
        newGame = new MenuButton(buttonTexture, new Point(256, 64), new Point(0, 0), new Point(256, 64), new Point(384, 204), "NEW GAME");
        exit.buttonPressed += OnExitClicked;
        newGame.buttonPressed += OnNewGameClicked;
        menuInputElements.Add(newGame);
        menuInputElements.Add(exit);
        newGame.font = fontBig;
        exit.font = fontBig;
        lights = new AnimatedSprite[24 + 24 + 2 + 2];
        for (int i = 0; i < 24; i++)
        {
            lights[i] = new AnimatedSprite(vegasLight, 32, 32, 0, 0, -384 + 16 + i * 32, 144);
            lights[i + 24 + 2] = new AnimatedSprite(vegasLight, 32, 32, 0, 0, 384 - 16 - i * 32, 48);
            lights[i].EnableAnimation(2, 2, 12);
            lights[i + 24 + 2].EnableAnimation(2, 2, 12);
        }
        lights[24] = new AnimatedSprite(vegasLight, 32, 32, 0, 0, 384 - 16, 112);
        lights[24].EnableAnimation(2, 2, 12);
        lights[25] = new AnimatedSprite(vegasLight, 32, 32, 0, 0, 384 - 16, 80);
        lights[25].EnableAnimation(2, 2, 12);
        lights[50] = new AnimatedSprite(vegasLight, 32, 32, 0, 0, -384 + 16, 80);
        lights[50].EnableAnimation(2, 2, 12);
        lights[51] = new AnimatedSprite(vegasLight, 32, 32, 0, 0, -384 + 16, 112);
        lights[51].EnableAnimation(2, 2, 12);
        for (int i = 0; i < 17; i++)
        {
            float frac1 = i / 17.0f;
            float frac2 = 1 - frac1;
            lights[i].Colour = new Color(frac2, frac1, 0, 1);
        }
        for (int i = 0; i < 18; i++)
        {
            float frac1 = i / 18.0f;
            float frac2 = 1 - frac1;
            lights[i + 17].Colour = new Color(0, frac2, frac1, 1);
        }
        for (int i = 0; i < 17; i++)
        {
            float frac1 = i / 17.0f;
            float frac2 = 1 - frac1;
            lights[i + 35].Colour = new Color(frac1, 0, frac2, 1);
        }
        backBoard = new Sprite(black, 1, 1, 0, 0, 768 + 16, 128 + 16, (1024 - 768 - 16) / 2, 24); 
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        Color[] textureData = new Color[64 * 256];
        for (int i = 0; i < 64; i++)
        {
            for (int j = 0; j < 256; j++)
            {
                float fin = 1;
                if (j > 7 && j < 248)
                {
                    if (i < 8)
                        fin = i / 8.0f;
                    else if (i > 55)
                        fin = (8 - (i - 55)) / 8.0f;
                }
                else if (i > 7 && i < 56)
                {
                    if (j < 8)
                        fin = j / 8.0f;
                    else if (j > 247)
                        fin = (8 - (j - 247)) / 8.0f;
                }
                else
                {
                    float minI = i;
                    if (i > 55 && i - 55 < minI)
                        minI = 8 - (i - 55);
                    float minJ = j;
                    if (j > 247 && j - 247 < minJ)
                        minJ = 8 - (j - 247);
                    float min = (float)Math.Min(minI, minJ);
                    if (min < 8)
                        fin = min / 8.0f;
                }
                textureData[i * 256 + j] = new Color(1, 1, 1,fin);
            }
        }
        buttonTexture = new Texture2D(Game.GraphicsDevice, 256, 64);
        buttonTexture.SetData<Color>(textureData);
        font = Game.Content.Load<SpriteFont>("LindseyButton");
        fontBig = Game.Content.Load<SpriteFont>("Lindsey");
        titleFont = Game.Content.Load<SpriteFont>("Lindsey");
        titleFontSmall = Game.Content.Load<SpriteFont>("LindseySmall");
        Color[] blck = { Color.Black};
        black = new Texture2D(Game.GraphicsDevice, 1, 1);
        black.SetData<Color>(blck);
        
        // vegas lights are generated here
        vegasLight = new Texture2D(Game.GraphicsDevice, 64, 64);
        Color[] data = new Color[64 * 64];
        Color[] light = CreateLight(Color.White, 8);
        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                data[i * 64 + j] = light[i * 32 + j];
            }
        }
        light = CreateLight(Color.White, 12);
        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                data[i * 64 + j + 32] = light[i * 32 + j];
            }
        }
        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                data[(32 + i) * 64 + j + 32] = light[i * 32 + j];
            }
        }
        light = CreateLight(Color.White, 16);
        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                data[(32 + i) * 64 + j] = light[i * 32 + j];
            }
        }
        vegasLight.SetData<Color>(data);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].Update(gameTime);
            lights[i].Colour = lights[i + 1 > 51 ? 0 : i + 1].Colour;
        }
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        JewSaver.primitiveBatch.Begin(PrimitiveType.LineList);
        JewSaver.primitiveBatch.AddLine(new Vector2(256, 128), new Vector2(256, 384), Color.Brown, Color.Black, 16);
        JewSaver.primitiveBatch.AddLine(new Vector2(768, 128), new Vector2(768, 384), Color.Brown, Color.Black, 16);
        JewSaver.primitiveBatch.End();
        JewSaver.spriteBatch.Begin();
        backBoard.Draw(JewSaver.spriteBatch);
        foreach (AnimatedSprite light in lights)
        {
            if (light != null)
                light.Draw(JewSaver.spriteBatch);
        }
        Vector2 measure1 = titleFont.MeasureString("FOLLOW MOSES");
        Vector2 measure2 = titleFontSmall.MeasureString("A journey to the Promised Land");
        JewSaver.spriteBatch.DrawString(titleFont, "FOLLOW MOSES", new Vector2(512, 84) - 0.5f * measure1, Color.White);
        JewSaver.spriteBatch.DrawString(titleFontSmall, "A journey to the Promised Land", new Vector2(512, 116) - 0.5f * measure2, Color.White);
        JewSaver.spriteBatch.End();
    }

    void OnExitClicked()
    {
        Game.Exit();
    }

    void OnNewGameClicked()
    {
        jewSaver.SwitchState(GameState.LEVEL_1);
    }

    private Color[] CreateLight(Color colour, float radius)
    {
        Color[] light = new Color[32 * 32];
        float radius2 = radius * radius;
        for (int i = -16; i < 16; i++)
        {
            for (int j = -16; j < 16; j++)
            {
                float dist2 = i * i + j * j;
                if (dist2 <= radius2)
                    light[(i + 16) * 32 + j + 16] = new Color(colour.R / 255.0f, colour.G / 255.0f, colour.B / 255.0f, (radius2 - dist2) / radius2);
                else
                    light[(i + 16) * 32 + j + 16] = Color.TransparentWhite;
            }
        }
        return light;
    }
}
