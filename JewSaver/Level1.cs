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
        AddCanyon(294, 294 + 256);
        AddCanyon(960, 1280);
        AddTrees();
    }
}
