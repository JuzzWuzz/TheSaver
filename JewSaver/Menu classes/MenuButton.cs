using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public delegate void ButtonPressed();

public class MenuButton:Sprite, MenuInputElement
{
    // private variables
    bool enabled;
    bool visible;
    public bool selected;
    public bool held;
    // top left pixel coordinates for different button states
    Point unselectedTLP;
    public string buttonName;
    Color textColor;
    public SpriteFont font;
    bool lastMouseDown;

    // button event when pressed
    public event ButtonPressed buttonPressed;

    public MenuButton(Texture2D tex, Point pixelDimensions, Point textureCoord, Point screenDimensions, Point screenCoord, string text)
        : base(tex, pixelDimensions.X, pixelDimensions.Y, textureCoord.X, textureCoord.Y, screenDimensions.X, screenDimensions.Y, screenCoord.X, screenCoord.Y)
    {
        enabled = true;
        visible = true;
        selected = false;
        held = false;
        unselectedTLP = topLeftPixel;
        buttonName = text;
        lastMouseDown = false;
    }

    bool MenuInputElement.Enabled
    {
        get { return enabled; }
        set
        {
            enabled = value;
            if (enabled)
            {
                Colour = Color.White;
                Alpha = 1;
            }
            else
            {
                Colour = Color.Gray;
                Alpha = 0.6f;
            }
        }
    }

    bool MenuInputElement.Visible
    {
        get { return visible; }
        set { visible = value; }
    }

    void MenuInputElement.Draw(SpriteBatch spriteBatch)
    {
        if (visible)
        {
            if (held)
            {
                Colour = Color.Black;
                textColor = Color.White;
            }
            else if (selected)
            {
                Colour = Color.White;
                textColor = Color.Black;
            }
            else
            {
                Colour = Color.Black;
                textColor = Color.White;
            }
            base.Draw(spriteBatch);
            Vector2 textDimensions = font.MeasureString(buttonName);
            spriteBatch.DrawString(font, buttonName, new Vector2(screenRectangle.Center.X,screenRectangle.Center.Y), textColor, 0, 0.5f * textDimensions, 1, SpriteEffects.None, 1);
        }
    }

    void MenuInputElement.CheckInput()
    {
        if (enabled && visible)
        {
            if (screenRectangle.Contains(Input.mousePosition) && GetAlpha(Input.mousePosition.X - screenRectangle.Location.X, Input.mousePosition.Y - screenRectangle.Location.Y) > 0)
            {
                selected = true;
                if (!held)
                {
                    if (Input.leftMouseDown && !lastMouseDown)
                        held = true;
                }
                else if (!Input.leftMouseDown)
                {
                    selected = false;
                    held = false;
                    if (buttonPressed != null)
                        buttonPressed();
                }
            }
            else
            {
                selected = false;
                held = false;
            }
        }
        lastMouseDown = Input.leftMouseDown;
    }
}
