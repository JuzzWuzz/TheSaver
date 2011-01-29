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
            AddCanyon(374, 374 + 128);
            AddCanyon(1040, 1160);
            AddTrees();
        }
        hasPlayed = true;
    }
}
