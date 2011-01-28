using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class Input:GameComponent
{
    // public variables
    public static Point mousePosition;
    public static Point deltaMousePos;
    public static Vector2 movementDir;
    public static bool leftMouseDown;
    public static bool rightMouseDown;
    public static bool spaceBarDown;
    public static bool enterDown;
    public static bool escapeDown;

    public Input(Game game)
        : base(game)
    {
        UpdateOrder = 1;
        mousePosition = Point.Zero;
        deltaMousePos = Point.Zero;
        movementDir = Vector2.Zero;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        MouseState mouseState = Mouse.GetState();
        deltaMousePos = new Point(mouseState.X - mousePosition.X, mouseState.Y - mousePosition.Y);
        mousePosition.X = mouseState.X;
        mousePosition.Y = mouseState.Y;
        leftMouseDown = mouseState.LeftButton == ButtonState.Pressed;
        rightMouseDown = mouseState.RightButton == ButtonState.Pressed;
        KeyboardState keyState = Keyboard.GetState();
        int posX = (keyState.IsKeyDown(Keys.D) || keyState.IsKeyDown(Keys.Right)) ? 1 : 0;
        int negX = (keyState.IsKeyDown(Keys.A) || keyState.IsKeyDown(Keys.Left)) ? -1 : 0;
        int posY = (keyState.IsKeyDown(Keys.W) || keyState.IsKeyDown(Keys.Up)) ? 1 : 0;
        int negY = (keyState.IsKeyDown(Keys.S) || keyState.IsKeyDown(Keys.Down)) ? -1 : 0;
        movementDir = new Vector2(posX + negX, posY + negY);
        if (movementDir != Vector2.Zero)
            movementDir.Normalize();
        spaceBarDown = keyState.IsKeyDown(Keys.Space);
        enterDown = keyState.IsKeyDown(Keys.Enter);
        escapeDown = keyState.IsKeyDown(Keys.Escape);
    }
}
