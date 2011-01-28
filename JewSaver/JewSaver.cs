using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

// use to specify game state
public enum GameState {MAIN_MENU, PAUSED, LEVEL_1, LEVEL_2, LEVEL_3};

/// <summary>
/// This is the main type for your game
/// </summary>
public class JewSaver : Microsoft.Xna.Framework.Game
{
    GraphicsDeviceManager graphics;
    public static SpriteBatch spriteBatch;
    public static PrimitiveBatch primitiveBatch;
    public static int height;
    public static int width;
    MenuJewSaver mainMenu;
    LevelBase baseLevel;

    public JewSaver()
    {
        graphics = new GraphicsDeviceManager(this);
        width = 1024;
        height = 768;
        graphics.PreferredBackBufferHeight = height;
        graphics.PreferredBackBufferWidth = width;
        Content.RootDirectory = "Content";
        this.IsMouseVisible = true;
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        
        // add game components here
        this.Components.Add(new Input(this));
        mainMenu = new MenuJewSaver(this);
        this.Components.Add(mainMenu);
        baseLevel = new LevelBase(this, 4096);
        baseLevel.Visible = false;
        baseLevel.Enabled = false;
        this.Components.Add(baseLevel);

        base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);
        primitiveBatch = new PrimitiveBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// all content.
    /// </summary>
    protected override void UnloadContent()
    {
        // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
        // Allows the game to exit
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            this.Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }

    public void SwitchState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.LEVEL_1:
                mainMenu.Visible = false;
                mainMenu.Enabled = false;
                baseLevel.Visible = true;
                baseLevel.Enabled = true;
                break;
            default:
                break;
        }
    }
}

static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static void Main(string[] args)
    {
        using (JewSaver game = new JewSaver())
        {
            game.Run();
        }
    }
}
