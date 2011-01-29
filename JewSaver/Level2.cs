using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Level2 : LevelBase
{
    public Level2(JewSaver game)
        : base(game, 3076)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        if (!hasPlayed)
        {
            AddWater(320, 448);
            AddWater(640, 768);
            AddTrees();
        }
        hasPlayed = true;
    }

    protected override void NextLevel()
    {
        jewSaver.SwitchState(GameState.LEVEL_3);
    }
}
