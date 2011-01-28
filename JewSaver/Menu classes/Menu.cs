using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Menu:DrawableGameComponent
{
    protected List<MenuInputElement> menuInputElements;
    protected List<Sprite> menuDecorElements;

    public Menu(JewSaver game)
        : base(game)
    {
        DrawOrder = 3;
        UpdateOrder = 2;
    }

    public override void Initialize()
    {
        base.Initialize();
        menuInputElements = new List<MenuInputElement>();
        menuDecorElements = new List<Sprite>();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        foreach (MenuInputElement obj in menuInputElements)
            obj.CheckInput();
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        JewSaver.spriteBatch.Begin();
        foreach (Sprite obj in menuDecorElements)
            obj.Draw(JewSaver.spriteBatch);
        foreach (MenuInputElement obj in menuInputElements)
            obj.Draw(JewSaver.spriteBatch);
        JewSaver.spriteBatch.End();
    }
}
