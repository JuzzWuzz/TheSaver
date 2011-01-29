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
        }
        hasPlayed = true;
        locustTimeout = new TimeSpan(0,0,5);
        locustTime = new TimeSpan(0, 0, 20);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        //Console.WriteLine(locustTimeout);
    }

    protected override void NextLevel()
    {
        jewSaver.SwitchState(GameState.LEVEL_2);
    }
}
