using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Sprite
{
    protected Texture2D texture; // sprite sheet
    public Rectangle screenRectangle; // rectangle defined in screen space
    protected Point topLeftPixel; // top left pixel in texture coordinates
    protected Point pixelDimensions; // pixel dimensions in texture
    protected Vector2 scaleFactor; // pixel-to-texel ratio

    // to control translucency of sprite
    private float alpha;
    private Color colour;
    public float Alpha
    {
        get { return alpha; }
        set { alpha = value; colour = new Color(colour.R, colour.G, colour.B, alpha); }
    }
    public Color Colour
    {
        get { return colour; }
        set { colour = value; }
    }

    /// <summary>
    /// Sprite constructor - Screen and texture coordinates have origin at top left
    /// </summary>
    /// <param name="tex">sprite sheet</param>
    /// <param name="pixelWidth">pixel width in sprite sheet</param>
    /// <param name="pixelHeight">pixel height in sprite sheet</param>
    /// <param name="textureX">leftmost pixel in sprite sheet</param>
    /// <param name="textureY">topmost pixel in sprite sheet </param>
    /// <param name="screenWidth">pixel width on screen</param>
    /// <param name="screenHeight">pixel height on screen</param>
    /// <param name="screenX">leftmost pixel on screen</param>
    /// <param name="screenY">topmost pixel on screen</param>
    public Sprite(Texture2D tex, int pixelWidth, int pixelHeight, int textureX, int textureY, int screenWidth, int screenHeight, int screenX, int screenY)
    {
        texture = tex;
        topLeftPixel = new Point(textureX, textureY);
        pixelDimensions = new Point(pixelWidth, pixelHeight);
        screenRectangle = new Rectangle(screenX, screenY, screenWidth, screenHeight);
        scaleFactor = new Vector2(pixelWidth / (float)screenWidth, pixelHeight / (float)screenHeight);
        colour = Color.White;
        alpha = 1;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, screenRectangle, new Rectangle(topLeftPixel.X, topLeftPixel.Y, pixelDimensions.X, pixelDimensions.Y), colour);
    }

    protected byte GetAlpha(int pixelOffsetX, int pixelOffsetY)
    {
        Color[] colours = new Color[texture.Width * texture.Height];
        texture.GetData<Color>(colours);
        return colours[texture.Width * (topLeftPixel.Y + pixelOffsetY) + topLeftPixel.X + pixelOffsetX].A;
    }
}
