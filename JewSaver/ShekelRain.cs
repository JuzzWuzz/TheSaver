using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class ShekelRain : DrawableGameComponent
{
    public ShekelRain(JewSaver game) : base(game)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        JewSaver.spriteBatch.Begin();
        JewSaver.spriteBatch.DrawString(LevelBase.font, "Congratulations! You have saved the Israelites\n from near certain extinction!", new Vector2(300, 150), Color.White, 0, 0.5f * LevelBase.font.MeasureString("VEGAS"), 1, SpriteEffects.None, 0);
        JewSaver.spriteBatch.End();
    
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}
