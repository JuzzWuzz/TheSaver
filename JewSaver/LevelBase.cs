using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class LevelBase:DrawableGameComponent
{
    protected enum LevelMode {EDIT, PLAY};
    protected float[] heightMap;
    protected LevelMode levelMode;
    protected float scrollX;
    float scrollSpeed;
    MenuButton play;
    MenuButton exit;
    MenuButton restart;
    Texture2D buttonTex;
    SpriteFont font;
    Texture2D star;
    Sprite[] stars;
    protected JewSaver jewSaver;
    Tree[] trees;

    int shekalim;

    // landscape sculpting brush
    Rectangle landscapeBrush;
    float brushSize;

    Stickfigure[] stickies = new Stickfigure[10];

    /// <summary>
    /// Base constructor for a level
    /// </summary>
    /// <param name="game">pointer to main game</param>
    /// <param name="levelLength">level length in pixels</param>
    public LevelBase(JewSaver game, int levelLength)
        : base(game)
    {
        heightMap = new float [levelLength];
        jewSaver = game;
    }

    public override void Initialize()
    {
        base.Initialize();
        for (int i = 0; i < heightMap.Length; i++)
            heightMap[i] = (float)(64 + 16 * Math.Sin(i / (float)heightMap.Length * Math.PI * 32));
        levelMode = LevelMode.EDIT;
        scrollX = 0;
        scrollSpeed = 300;
        brushSize = 224;
        landscapeBrush = new Rectangle(0, 0, (int)(brushSize*1.5f), (int)brushSize);
        play = new MenuButton(buttonTex, new Point(96, 96), new Point(0, 0), new Point(96, 96), new Point(8, 8), "PLAY");
        play.font = font;
        play.buttonPressed += OnPlayPressed;
        restart = new MenuButton(buttonTex, new Point(96, 96), new Point(0, 0), new Point(96, 96), new Point(8, 8), "CLEAR");
        restart.font = font;
        restart.buttonPressed += OnRestartPressed;
        exit = new MenuButton(buttonTex, new Point(96, 96), new Point(0, 0), new Point(96, 96), new Point(1024 - 8 - 96, 8), "QUIT");
        exit.font = font;
        exit.buttonPressed += OnQuitPressed;
        for (int i = 0; i < 10; i++)
            stickies[i] = new Stickfigure(new Vector2(50 + i * 30, i * 30));
        Random random = new Random();
        stars = new Sprite[384];
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i] = new Sprite(star, 8, 8, 0, 0, 8, 8, random.Next(heightMap.Length/2), random.Next(64));
            stars[i].Alpha = (64 - stars[i].screenRectangle.Top) / 96.0f;
        }


        int treeNum = random.Next(30, 60);
        trees = new Tree[treeNum];
        for (int i = 0; i < treeNum; i++)
            trees[i] = new Tree(new Vector2(random.Next(0, heightMap.Length), JewSaver.height - 20));


        shekalim = 0;
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        buttonTex = new Texture2D(Game.GraphicsDevice, 96, 96);
        Color[] data = new Color[96 * 96];
        for (int i = -48; i < 48; i++)
        {
            for (int j = -48; j < 48; j++)
            {
                float dist2 = i * i + j * j;
                if (dist2 <= 1024)
                    data[(i + 48) * 96 + j + 48] = Color.White;
                else if (dist2 < 1764)
                    data[(i + 48) * 96 + j + 48] = new Color(1, 1, 1, (1764 - dist2) / (1764 - 1024));
            }
        }
        star = new Texture2D(Game.GraphicsDevice, 8, 8);
        Color[] starData = new Color[8*8];
        for (int i = -4; i < 4; i++)
        {
            float frac = (float)((4 - Math.Abs(i)) / 4.0f);
            starData[4 * 8 + i + 4] = new Color(1,1,1,frac);
            starData[(i + 4) * 8 + 4] = new Color(1, 1, 1, frac);
        }
        star.SetData<Color>(starData);
        buttonTex.SetData<Color>(data);
        font = Game.Content.Load<SpriteFont>("LevelText");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        (exit as MenuInputElement).CheckInput();
        if (levelMode == LevelMode.EDIT)
        {
            (play as MenuInputElement).CheckInput();
            int mouseX = Input.mousePosition.X;
            brushSize += Input.deltaScroll / 240.0f * 16;
            if (brushSize < 128)
                brushSize = 128;
            if (brushSize > 224)
                brushSize = 224;
            landscapeBrush = new Rectangle((int)(mouseX - brushSize * 1.5f / 2), (int)(Input.mousePosition.Y - brushSize / 2), (int)(brushSize * 1.5f), (int)brushSize);
            if (mouseX < 0)
            {
                if (scrollX > 0)
                {
                    scrollX -= (float)gameTime.ElapsedGameTime.TotalSeconds * scrollSpeed;
                    if (scrollX < 0)
                        scrollX = 0;
                }
            }
            else if (mouseX > 1023)
            {
                if (scrollX < heightMap.Length - 1025)
                {
                    scrollX += (float)gameTime.ElapsedGameTime.TotalSeconds * scrollSpeed;
                    if (scrollX > heightMap.Length - 1025)
                        scrollX = heightMap.Length - 1025;
                }
            }
            else if (Input.leftMouseDown && !play.selected && !play.held && !exit.selected && !exit.held)
            {
                int startIndex = (int)(scrollX + landscapeBrush.Left < 0 ? 0 : scrollX + landscapeBrush.Left);
                int endIndex = (int)(scrollX + landscapeBrush.Right > heightMap.Length ? heightMap.Length : scrollX + landscapeBrush.Right);
                int midIndex = (startIndex + endIndex) / 2;
                float start = heightMap[startIndex];
                float end = heightMap[endIndex - 1];
                float target = 384 - landscapeBrush.Top;
                if (target > 256)
                    target = 256;
                else if (target < 16)
                    target = 16;
                for (int i = startIndex; i < midIndex; i++)
                {
                    float cubicValue = CosineInterpolate( start, target,(i - startIndex) / (float)(midIndex - startIndex));
                    heightMap[i] += (float)((cubicValue - heightMap[i]) * gameTime.ElapsedGameTime.TotalSeconds) * 4;
                }
                for (int i = midIndex; i < endIndex; i++)
                {
                    float cubicValue = CosineInterpolate(target, end,(i - midIndex) / (float)(endIndex - midIndex));
                    heightMap[i] += (float)((cubicValue - heightMap[i]) * gameTime.ElapsedGameTime.TotalSeconds) * 4;
                }
            }
        }
        else if (levelMode == LevelMode.PLAY)
        {
            (restart as MenuInputElement).CheckInput();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Stickfigure s in stickies)
            {
                s.update(dt, heightMap, scrollX);
            }
        }
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        JewSaver.spriteBatch.Begin();
        foreach (Sprite str in stars)
        {
            str.scrollXValue = 0.125f * scrollX;
            str.Draw(JewSaver.spriteBatch);
        }
        (exit as MenuInputElement).Draw(JewSaver.spriteBatch);
        JewSaver.spriteBatch.End();

        JewSaver.primitiveBatch.Begin(PrimitiveType.LineList);

        for (int i = (int)scrollX; i < (int)scrollX + 1024; i++)
        {
            JewSaver.primitiveBatch.AddVertex(new Vector2(i - (int)scrollX, 384 - heightMap[i]), Color.Gold);
            JewSaver.primitiveBatch.AddVertex(new Vector2(i - (int)scrollX, 384), Color.Sienna);
        }
        if (levelMode == LevelMode.EDIT)
        {
            JewSaver.primitiveBatch.AddVertex(new Vector2(landscapeBrush.Left, landscapeBrush.Top), Color.YellowGreen);
            JewSaver.primitiveBatch.AddVertex(new Vector2(landscapeBrush.Right, landscapeBrush.Top), Color.DarkGreen);
            JewSaver.primitiveBatch.AddVertex(new Vector2(landscapeBrush.Right, landscapeBrush.Top), Color.DarkGreen);
            JewSaver.primitiveBatch.AddVertex(new Vector2(landscapeBrush.Right, landscapeBrush.Bottom), Color.YellowGreen);
            JewSaver.primitiveBatch.AddVertex(new Vector2(landscapeBrush.Right, landscapeBrush.Bottom), Color.YellowGreen);
            JewSaver.primitiveBatch.AddVertex(new Vector2(landscapeBrush.Left, landscapeBrush.Bottom), Color.DarkGreen);
            JewSaver.primitiveBatch.AddVertex(new Vector2(landscapeBrush.Left, landscapeBrush.Bottom), Color.DarkGreen);
            JewSaver.primitiveBatch.AddVertex(new Vector2(landscapeBrush.Left, landscapeBrush.Top), Color.YellowGreen);
        }
        JewSaver.primitiveBatch.End();

        for (int i = 0; i < trees.Length; i++)
        {
            trees[i].draw();
        }

        if (levelMode == LevelMode.EDIT)
        {
            JewSaver.spriteBatch.Begin();
            (play as MenuInputElement).Draw(JewSaver.spriteBatch);
            JewSaver.spriteBatch.End();
        }

        else if (levelMode == LevelMode.PLAY)
        {
            // Draw stickies
            foreach (Stickfigure s in stickies)
            {
                s.draw();
            }
            JewSaver.spriteBatch.Begin();
            (restart as MenuInputElement).Draw(JewSaver.spriteBatch);
            JewSaver.spriteBatch.End();
        }
    }

    /// <summary>
    /// Find interpolated values between two points
    /// </summary>
    /// <param name="y0">leftmost</param>
    /// <param name="y1">start</param>
    /// <param name="y2">end</param>
    /// <param name="y3">rightmost</param>
    /// <param name="mu">fraction between start and end</param>
    /// <returns></returns>
    private float CubicInterpolate(float y0, float y1, float y2, float y3, float mu)
    {
        float mu2 = mu * mu;
        float a0 = y3 - y2 - y0 - y1;
        float a1 = y0 - y1 - a0;
        float a2 = y2 - y0;
        float a3 = y1;
        return a0 * mu * mu2 + a1 * mu2 + a2 * mu + a3;
    }

    private float CosineInterpolate(float y1, float y2, float mu)
    {
        float mu2 = (float)(1 - Math.Cos(mu * Math.PI)) / 2.0f;
        return y1 * (1 - mu2) + y2 * mu2;
    }

    private void OnPlayPressed()
    {
        (play as MenuInputElement).Enabled = false;
        levelMode = LevelMode.PLAY;
    }

    private void OnQuitPressed()
    {
        jewSaver.SwitchState(GameState.MAIN_MENU);
    }

    private void OnRestartPressed()
    {
        this.Initialize();
    }
}
