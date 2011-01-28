using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class MenuTest:Menu
{
    Texture2D buttonTexture;
    Texture2D background;
    MenuButton newGame;
    MenuButton options;
    MenuButton exit;
    Sprite back;

    public MenuTest(JewSaver game)
        : base(game)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        exit = new MenuButton(buttonTexture, new Point(256, 64), new Point(0, 0), new Point(256, 64), new Point(384, 432));
        exit.SetTopLeftPixel(new Point(0, 64), new Point(0, 0));
        newGame = new MenuButton(buttonTexture, new Point(256, 64), new Point(0, 512), new Point(256, 64), new Point(384, 272));
        newGame.SetTopLeftPixel(new Point(0, 576), new Point(0, 512));
        options = new MenuButton(buttonTexture, new Point(256, 64), new Point(0, 256), new Point(256, 64), new Point(384, 352));
        options.SetTopLeftPixel(new Point(0, 320), new Point(0, 256));
        exit.buttonPressed += OnExitClicked;
        newGame.buttonPressed += OnNewGameClicked;
        options.buttonPressed += OnOptionsClicked;
        menuInputElements.Add(newGame);
        menuInputElements.Add(options);
        menuInputElements.Add(exit);
        back = new Sprite(background, 1024, 768, 0, 0, 1024, 768, 0, 0);
        menuDecorElements.Add(back);
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        buttonTexture = Game.Content.Load<Texture2D>("Textures//MainMenu");
        background = Game.Content.Load<Texture2D>("Textures//Pretty");
    }

    void OnExitClicked()
    {
        Game.Exit();
    }

    void OnNewGameClicked()
    {
    }

    void OnOptionsClicked()
    {
    }
}
