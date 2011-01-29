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
using NAudio;
using NAudio.Wave;
using System.IO;
using System.Net;
using System.Threading;

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
        height = 384;
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
        GraphicsDevice.Clear(Color.PowderBlue);

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
    public static void PlayMp3FromUrl()
    {
        string url = "http://music.incompetech.com/royalty-free/Blue%20Scorpion.mp3";
        using (Stream ms = new MemoryStream())
        {
            using (Stream stream = WebRequest.Create(url)
                .GetResponse().GetResponseStream())
            {
                byte[] buffer = new byte[32768];
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
            }

            ms.Position = 0;
            using (WaveStream blockAlignedStream =
                new BlockAlignReductionStream(
                    WaveFormatConversionStream.CreatePcmStream(
                        new Mp3FileReader(ms))))
            {
                using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                {
                    waveOut.Init(blockAlignedStream);
                    waveOut.Play();
                    while (waveOut.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
        }
    }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static void Main(string[] args)
    {
        Thread oThread = new Thread(new ThreadStart(Program.PlayMp3FromUrl));
        oThread.Start();

        using (JewSaver game = new JewSaver())
        {
            game.Run();
        }
        oThread.Abort();
    }
}
