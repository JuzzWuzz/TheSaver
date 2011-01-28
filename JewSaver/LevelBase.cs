using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class LevelBase:DrawableGameComponent
{
    protected enum LevelMode {EDIT, PLAY};
    float[] heightMap;

    Stickfigure[] stickies = new Stickfigure[10];

    /// <summary>
    /// Base constructor for a level
    /// </summary>
    /// <param name="game">pointer to main game</param>
    /// <param name="levelLength">level length in pixels</param>
    public LevelBase(JewSaver game, int levelLength)
        : base(game)
    {
        heightMap = new float [levelLength];
    }

    public override void Initialize()
    {
        base.Initialize();
        for (int i = 0; i < 10; i++)
            stickies[i] = new Stickfigure(new Vector2(i * 10, i * 10));
    }

    protected override void LoadContent()
    {
        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        foreach (Stickfigure s in stickies) {
            s.draw(); 
        }
    }
}
