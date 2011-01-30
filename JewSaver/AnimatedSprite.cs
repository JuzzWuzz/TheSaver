using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class AnimatedSprite:Sprite
{
    // basic attributes
    public Vector2 position;

    // advanced attributes
    private bool animated;
    private int framesX;
    private int framesY;
    private float frameTime;
    private double timer;
    private Point currentFrame;

    /// <summary>
    /// AnimatedSprite constructor
    /// </summary>
    /// <param name="tex">sprite sheet</param>
    /// <param name="width">width of sprite</param>
    /// <param name="height">height of sprite</param>
    /// <param name="textureX">leftmost pixel in texture</param>
    /// <param name="textureY">topmost pixel in texture</param>
    /// <param name="posX">world x-coordinate</param>
    /// <param name="posY">world y-coordinate</param>
    public AnimatedSprite(Texture2D tex, int width, int height, int textureX, int textureY, int posX, int posY)
        : base(tex, width, height, textureX, textureY, width, height, posX - width / 2 + JewSaver.width / 2, -posY - height / 2 + JewSaver.height / 2)
    {
        position = new Vector2(posX, posY);
    }

    public void EnableAnimation(int horizontalFrames, int verticalFrames, int framesPerSecond)
    {
        animated = true;
        framesX = horizontalFrames;
        framesY = verticalFrames;
        frameTime = 1.0f / framesPerSecond;
        timer = 0;
    }

    public virtual void Update(GameTime gameTime)
    {
        if (animated)
        {
            timer +=  gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= frameTime)
            {
                timer -= frameTime;
                currentFrame.X++;
                if (currentFrame.X == framesX)
                {
                    currentFrame.X = 0;
                    currentFrame.Y++;
                    if (currentFrame.Y == framesY)
                        currentFrame.Y = 0;
                }
                topLeftPixel.X = currentFrame.X * pixelDimensions.X;
                topLeftPixel.Y = currentFrame.Y * pixelDimensions.Y;
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
}
