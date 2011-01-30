using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Level1:LevelBase
{
    public Level1(JewSaver game)
        : base(game, 1536)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        if (!hasPlayed)
        {
            // good canyon size is 170
            AddCanyon(374, 374 + 170);
            AddCanyon(1040, 1210);
            AddTrees();

            for (int i = 0; i < LevelBase.levelLength; i++)
                heightMapBak[i] = heightMap[i];
        }
        hasPlayed = true;

        openingText = true;
        finalTexts.Add("Level 1");
        finalTexts.Add("Get your people to safety!");
        finalTexts.Add("Press SpaceBar to jump");
        finalTexts.Add("Use Edit Mode to Sculpt the landscape to make it easier to save your people");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    protected override void NextLevel()
    {
        jewSaver.SwitchState(GameState.LEVEL_2);
    }
}
