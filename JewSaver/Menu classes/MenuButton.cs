using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public delegate void ButtonPressed();

public class MenuButton:Sprite, MenuInputElement
{
    // private variables
    bool enabled;
    bool visible;
    bool selected;
    bool held;
    // top left pixel coordinates for different button states
    Point selectedTLP;
    Point unselectedTLP;
    Point pressedTLP;

    // button event when pressed
    public event ButtonPressed buttonPressed;

    public MenuButton(Texture2D tex, Point pixelDimensions, Point textureCoord, Point screenDimensions, Point screenCoord)
        : base(tex, pixelDimensions.X, pixelDimensions.Y, textureCoord.X, textureCoord.Y, screenDimensions.X, screenDimensions.Y, screenCoord.X, screenCoord.Y)
    {
        enabled = true;
        visible = true;
        selected = false;
        held = false;
        unselectedTLP = topLeftPixel;
        selectedTLP = topLeftPixel;
        pressedTLP = topLeftPixel;
    }

    public void SetTopLeftPixel(Point selected, Point pressed)
    {
        selectedTLP = selected;
        pressedTLP = pressed;
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
                topLeftPixel = pressedTLP;
            else if (selected)
                topLeftPixel = selectedTLP;
            else
                topLeftPixel = unselectedTLP;
            base.Draw(spriteBatch);
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
                    if (Input.leftMouseDown)
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
    }
}
