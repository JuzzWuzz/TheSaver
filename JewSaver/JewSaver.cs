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
    public static Sprite BG;
    Texture2D background;
    public static int height;
    public static int width;
    MenuJewSaver mainMenu;
    //LevelBase baseLevel;
    LevelBase currentLevel;
    Level1 level1;

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
        /*baseLevel = new LevelBase(this, 4096);
        baseLevel.Visible = false;
        baseLevel.Enabled = false;
        baseLevel.showFrameRate = false;*/
        level1 = new Level1(this);
        level1.Visible = false;
        level1.Enabled = false;
        this.Components.Add(level1);

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
        Color[] back = new Color[384];
        Color mix;
        background = new Texture2D(GraphicsDevice, 1, 384);
        for (int i = 0; i < 106; i++)
        {
            float frac = i / 106.0f;
            mix = new Color(frac * 36 / 255.0f, frac * 16 / 255.0f, frac * 63 / 255.0f);
            back[i] = mix;
        }
        for (int i = 106; i < 106 + 95; i++)
        {
            float frac1 = (i - 106) / 95.0f;
            float frac2 = 1 - frac1;
            mix = new Color((frac2 * 36 / 255.0f + frac1 * 99 / 255.0f), (frac2 * 16 / 255.0f + frac1 * 6 / 255.0f), (frac2 * 63 / 255.0f + frac1 * 42 / 255.0f));
            back[i] = mix;
        }
        for (int i = 106 + 95; i < 106 + 95 + 78; i++)
        {
            float frac1 = (i - 106 - 95) / 78.0f;
            float frac2 = 1 - frac1;
            mix = new Color((frac2 * 99 / 255.0f + frac1 * 186 / 255.0f), (frac2 * 6 / 255.0f), (frac2 * 42 / 255.0f));
            back[i] = mix;
        }
        for (int i = 106 + 95 + 78; i < 106 + 95 + 78 + 105; i++)
        {
            float frac1 = (i - 106 - 95 - 78) / 105.0f;
            float frac2 = 1 - frac1;
            mix = new Color((frac2 * 186 / 255.0f + frac1 * 239 / 255.0f), (frac1 * 98 / 255.0f), (frac1 * 10 / 255.0f));
            back[i] = mix;
        }
        background.SetData<Color>(back);
        BG = new Sprite(background, 1, 384, 0, 0, 1024, 384, 0, 0);

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
        spriteBatch.Begin();
        BG.Draw(spriteBatch);
        spriteBatch.End();
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
                level1.Initialize();
                currentLevel = level1;
                currentLevel.Visible = true;
                currentLevel.Enabled = true;
                break;
            case GameState.MAIN_MENU:
                mainMenu.Visible = true;
                mainMenu.Enabled = true;
                currentLevel.Visible = false;
                currentLevel.Enabled = false;
                break;
            case GameState.LEVEL_2:
                currentLevel.Visible = false;
                currentLevel.Enabled = false;
                break;
            default:
                break;
        }
    }
}



static class Program
{
    static volatile bool stop = false;
    static List<string> urls = new List<string>();

    public static void PlayMp3FromUrl()
    {
        return;
        /*royalty free african music. because we are africa.*/
        Random random = new Random();
        string url = urls[random.Next(urls.Count())];
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
                        if (stop)
                        {
                            waveOut.Stop();
                            while (waveOut.PlaybackState != PlaybackState.Stopped)
                                Console.Write("Exiting");
                            Environment.Exit(0);
                            return;
                        }

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
        urls.Add("http://music.incompetech.com/royalty-free/Old%20Road.mp3");
        urls.Add("http://music.incompetech.com/royalty-free/Ibn%20Al-Noor.mp3");
        urls.Add("http://music.incompetech.com/royalty-free/Desert%20City.mp3");
        urls.Add("http://music.incompetech.com/royalty-free/Lachaim.mp3");
        urls.Add("http://music.incompetech.com/royalty-free/Big%20Mojo.mp3");
        urls.Add("http://music.incompetech.com/royalty-free/East%20of%20Tunesia.mp3");
        urls.Add("http://music.incompetech.com/royalty-free/Balzan%20Groove.mp3");

        Thread oThread = new Thread(new ThreadStart(Program.PlayMp3FromUrl));
        oThread.Start();

        using (JewSaver game = new JewSaver())
        {
            game.Run();
        }
        stop = true;
        Console.WriteLine("Chilling");
        Thread.Sleep(100);
        oThread.Join(100);
        Console.WriteLine("Still here!");
        Environment.Exit(0);
    }
}
