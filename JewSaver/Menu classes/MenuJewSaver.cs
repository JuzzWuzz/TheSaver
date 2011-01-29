using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class MenuJewSaver:Menu
{
    Texture2D buttonTexture;
    Texture2D background;
    MenuButton newGame;
    MenuButton options;
    MenuButton exit;
    Sprite back;
    SpriteFont font;
    JewSaver jewSaver;

    public MenuJewSaver(JewSaver game)
        : base(game)
    {
        jewSaver = game;
    }

    public override void Initialize()
    {
        base.Initialize();
        exit = new MenuButton(buttonTexture, new Point(256, 64), new Point(0, 0), new Point(256, 64), new Point(384, 240), "EXIT");
        newGame = new MenuButton(buttonTexture, new Point(256, 64), new Point(0, 0), new Point(256, 64), new Point(384, 80), "NEW GAME");
        options = new MenuButton(buttonTexture, new Point(256, 64), new Point(0, 0), new Point(256, 64), new Point(384, 160), "OPTIONS");
        exit.buttonPressed += OnExitClicked;
        newGame.buttonPressed += OnNewGameClicked;
        options.buttonPressed += OnOptionsClicked;
        menuInputElements.Add(newGame);
        menuInputElements.Add(options);
        menuInputElements.Add(exit);
        newGame.font = font;
        options.font = font;
        exit.font = font;
        back = new Sprite(background, 1024, 384, 0, 0, 1024, 384, 0, 0);
        menuDecorElements.Add(back);
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
        Color[] backgroundData = new Color[384 * 1024];
        Random random = new Random();
        for (int i = 0; i < 384; i++)
        {
            for (int j = 0; j < 1024; j++)
            {
                float sin =(float)Math.Abs(Math.Sin(j/1024.0f * 2 * Math.PI));
                float cos =(float)Math.Abs(Math.Cos(j/1024.0f * 2 * Math.PI));
                backgroundData[i * 1024 + j] = new Color(sin,0,0, 255);
            }
        }
        background = new Texture2D(Game.GraphicsDevice, 1024, 384);
        background.SetData<Color>(backgroundData);
        font = Game.Content.Load<SpriteFont>("ButtonText");
    }

    void OnExitClicked()
    {
        Game.Exit();
    }

    void OnNewGameClicked()
    {
        jewSaver.SwitchState(GameState.LEVEL_1);
    }

    void OnOptionsClicked()
    {
    }
}
