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
    public static bool spaceBarPressed;
    public static bool enterDown;
    public static bool escapeDown;
    public static float mouseScrollValue;
    public static float deltaScroll;

    private static KeyboardState prevState;
    private static KeyboardState curState;

    public Input(Game game)
        : base(game)
    {
        UpdateOrder = 1;
        mousePosition = Point.Zero;
        deltaMousePos = Point.Zero;
        movementDir = Vector2.Zero;

        curState = Keyboard.GetState();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        MouseState mouseState = Mouse.GetState();
        deltaMousePos = new Point(mouseState.X - mousePosition.X, mouseState.Y - mousePosition.Y);
        mousePosition.X = mouseState.X;
        mousePosition.Y = mouseState.Y;
        deltaScroll = mouseState.ScrollWheelValue - mouseScrollValue;
        mouseScrollValue = mouseState.ScrollWheelValue;
        leftMouseDown = mouseState.LeftButton == ButtonState.Pressed;
        rightMouseDown = mouseState.RightButton == ButtonState.Pressed;
        prevState = curState;
        curState = Keyboard.GetState();
        int posX = (curState.IsKeyDown(Keys.D) || curState.IsKeyDown(Keys.Right)) ? 1 : 0;
        int negX = (curState.IsKeyDown(Keys.A) || curState.IsKeyDown(Keys.Left)) ? -1 : 0;
        int posY = (curState.IsKeyDown(Keys.W) || curState.IsKeyDown(Keys.Up)) ? 1 : 0;
        int negY = (curState.IsKeyDown(Keys.S) || curState.IsKeyDown(Keys.Down)) ? -1 : 0;
        movementDir = new Vector2(posX + negX, posY + negY);
        if (movementDir != Vector2.Zero)
            movementDir.Normalize();
        spaceBarPressed = curState.IsKeyDown(Keys.Space) && prevState.IsKeyUp(Keys.Space);
        enterDown = curState.IsKeyDown(Keys.Enter);
        escapeDown = curState.IsKeyDown(Keys.Escape);
    }
}
