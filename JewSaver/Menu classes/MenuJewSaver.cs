using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class MenuJewSaver:Menu
{
    Texture2D buttonTexture;
    MenuButton newGame;
    MenuButton exit;
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
        exit = new MenuButton(buttonTexture, new Point(256, 64), new Point(0, 0), new Point(256, 64), new Point(384, 280), "EXIT");
        newGame = new MenuButton(buttonTexture, new Point(256, 64), new Point(0, 0), new Point(256, 64), new Point(384, 200), "NEW GAME");
        exit.buttonPressed += OnExitClicked;
        newGame.buttonPressed += OnNewGameClicked;
        menuInputElements.Add(newGame);
        menuInputElements.Add(exit);
        newGame.font = font;
        exit.font = font;
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
