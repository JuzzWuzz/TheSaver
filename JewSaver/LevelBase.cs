using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class LevelBase : DrawableGameComponent
{
    protected enum LevelMode {EDIT, PLAY};
    public enum TerrainType { SAND, WATER, CANYON, OTHER, ROCK , PARCHED_LAND};
    public static float[] heightMap;
    public static TerrainType[] canSculpt;
    protected LevelMode levelMode;
    public static float scrollX;
    float scrollSpeed;
    MenuButton play;
    MenuButton exit;
    MenuButton restart;
    Texture2D buttonTex;
    public static SpriteFont font;
    Texture2D star;
    Sprite[] stars;
    protected JewSaver jewSaver;
    Tree[] trees;
    public static Random random = new Random();
    protected bool hasPlayed;
    public static int levelLength;
    public static List<float> jumpMarkers = new List<float>();
    public static List<float> sprintMarkers = new List<float>();
    int numberOfStickies;
    float levelTime;

    //Swarm stuff
    protected static List<Locust> locusts;
    protected TimeSpan locustTimeout;
    protected TimeSpan locustTime;
    protected int plagueLength;
    protected TimeSpan locustSpawner;
    protected bool hasLocusts;

    // End screen text
    protected List<string> finalTexts;
    float textTimer;
    protected bool showText;
    bool goToNextLevel;
    protected bool openingText;

    // For testing
    public bool showFrameRate;
    int frames;
    double fpsTime;

    // landscape sculpting brush
    Rectangle landscapeBrush;
    float brushSize;
    int deadStickies;
    int savedStickies;
    int savedFemales;
    int savedFatties;
    protected bool gameAllOver;

    protected Stickfigure[] stickies;
    public static Stickfigure moses;
    protected List<Stickfigure>[] collisionBuckets;

    //dirty little spot, but whatever. gold stuff:
    public static float[] hm;
    float shekelTimeout;
    List<Schekel> treasure;
    int sheckels;

    /// <summary>
    /// Base constructor for a level
    /// </summary>
    /// <param name="game">pointer to main game</param>
    /// <param name="levelLength">level length in pixels</param>
    public LevelBase(JewSaver game, int newLevelLength)
        : base(game)
    {
        levelLength = newLevelLength;
        heightMap = new float[levelLength];
        canSculpt = new TerrainType[levelLength];
        finalTexts = new List<string>();
        jewSaver = game;
        hasPlayed = false;
        numberOfStickies = 50;
        stickies = new Stickfigure[numberOfStickies];
    }

    public override void Initialize()
    {
        base.Initialize();
        levelTime = 0;
        if (!hasPlayed)
        {
            for (int i = 0; i < heightMap.Length; i++)
            {
                heightMap[i] = (float)(64 + 16 * Math.Sin(i / 4096.0f * Math.PI * 32));
                canSculpt[i] = TerrainType.SAND;
            }
        }
        levelMode = LevelMode.EDIT;
        scrollX = 0;
        scrollSpeed = 300;
        brushSize = 224;
        landscapeBrush = new Rectangle(0, 0, (int)(brushSize * 1.5f), (int)brushSize);
        play = new MenuButton(buttonTex, new Point(96, 96), new Point(0, 0), new Point(96, 96), new Point(8, 8), "PLAY");
        play.font = font;
        play.buttonPressed += OnPlayPressed;
        restart = new MenuButton(buttonTex, new Point(96, 96), new Point(0, 0), new Point(96, 96), new Point(8, 8), "RESET");
        restart.font = font;
        restart.buttonPressed += OnRestartPressed;
        exit = new MenuButton(buttonTex, new Point(96, 96), new Point(0, 0), new Point(96, 96), new Point(JewSaver.width - 8 - 96, 8), "QUIT");
        exit.font = font;
        exit.buttonPressed += OnQuitPressed;
        stickies = new Stickfigure[numberOfStickies];
        collisionBuckets = new List<Stickfigure>[128];
        for (int i = 0; i < 128; i++)
            collisionBuckets[i] = new List<Stickfigure>();
        for (int i = 0; i < numberOfStickies; i++)
            stickies[i] = new Stickfigure(new Vector2(-50.0f, 0), i + 1);
        moses = new Stickfigure(new Vector2(50, 200), 0);
        moses.SetIsPlayer();
        jumpMarkers.Clear();
        sprintMarkers.Clear();
        finalTexts.Clear();
        showText = false;
        openingText = false;
        gameAllOver = false;


        frames = 0;
        fpsTime = 0.0;

        locusts = new List<Locust>();
        locustTimeout = new TimeSpan(0, 0, 15);
        locustTime = new TimeSpan(0, 1, 0);
        plagueLength = 0;
        hasLocusts = false;

        if (!hasPlayed)
        {
            stars = new Sprite[JewSaver.height];
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i] = new Sprite(star, 8, 8, 0, 0, 8, 8, random.Next(2048), random.Next(64));
                stars[i].Alpha = (64 - stars[i].screenRectangle.Top) / 96.0f;
            }
            textTimer = 0;
            showText = false;
        }
        goToNextLevel = false;

        treasure = new List<Schekel>();
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
                if (dist2 <= JewSaver.width)
                    data[(i + 48) * 96 + j + 48] = Color.White;
                else if (dist2 < 1764)
                    data[(i + 48) * 96 + j + 48] = new Color(1, 1, 1, (1764 - dist2) / (1764 - JewSaver.width));
            }
        }
        star = new Texture2D(Game.GraphicsDevice, 8, 8);
        Color[] starData = new Color[8 * 8];
        for (int i = -4; i < 4; i++)
        {
            float frac = (float)((4 - Math.Abs(i)) / 4.0f);
            starData[4 * 8 + i + 4] = new Color(1, 1, 1, frac);
            starData[(i + 4) * 8 + 4] = new Color(1, 1, 1, frac);
        }
        star.SetData<Color>(starData);
        buttonTex.SetData<Color>(data);
        font = Game.Content.Load<SpriteFont>("LevelText");
    }

    public override void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        base.Update(gameTime);
        levelTime += (float)(gameTime.ElapsedGameTime.TotalSeconds);
        (exit as MenuInputElement).CheckInput();
        if (levelMode == LevelMode.EDIT)
        {
            if (openingText)
            {
                textTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (textTimer >= 4.0f)
                {
                    finalTexts.RemoveAt(0);
                    textTimer = 0.0f;
                    if (finalTexts.Count <= 0)
                    {
                        openingText = false;
                    }
                }
            }
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
            else if (mouseX > JewSaver.width - 1)
            {
                if (scrollX < heightMap.Length - JewSaver.width - 1)
                {
                    scrollX += (float)gameTime.ElapsedGameTime.TotalSeconds * scrollSpeed;
                    if (scrollX > heightMap.Length - JewSaver.width - 1)
                        scrollX = heightMap.Length - JewSaver.width - 1;
                }
            }
            else if (Input.leftMouseDown && !play.selected && !play.held && !exit.selected && !exit.held)
            {
                int startIndex = (int)(scrollX + landscapeBrush.Left < 0 ? 0 : scrollX + landscapeBrush.Left);
                int endIndex = (int)(scrollX + landscapeBrush.Right > heightMap.Length ? heightMap.Length : scrollX + landscapeBrush.Right);
                int midIndex = (startIndex + endIndex) / 2;

                List<int> intervals = new List<int>();
                bool lookingForStart = true;
                int lastStart = 0;
                for (int i = startIndex; i < endIndex; i++)
                {
                    if (lookingForStart)
                    {
                        if (canSculpt[i] == TerrainType.SAND || canSculpt[i] == TerrainType.PARCHED_LAND)
                        {
                            lookingForStart = false;
                            lastStart = i;
                            intervals.Add(i);
                        }
                    }
                    else if ((canSculpt[i] != TerrainType.SAND && canSculpt[i] != TerrainType.PARCHED_LAND) || (i == endIndex - 1 && !lookingForStart))
                    {
                        intervals.Add((int)((lastStart + i - 1) / 2.0f));
                        intervals.Add(i);
                        lookingForStart = true;
                    }
                }
                if (intervals.Count % 3 == 0)
                {
                    for (int i = 0; i < intervals.Count; i += 3)
                    {
                        int width = intervals[i + 2] - intervals[i];
                        if (width > 32)
                            Sculpt(intervals[i], intervals[i + 1], intervals[i + 2], gameTime, landscapeBrush.Center.Y - (int)(width / 3.0f));
                    }
                }
            }
        }
        else if (levelMode == LevelMode.PLAY)
        {
            if (!goToNextLevel && !gameAllOver)
                (restart as MenuInputElement).CheckInput();
            if (showText)
            {
                textTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (textTimer >= 4.0f)
                {
                    finalTexts.RemoveAt(0);
                    textTimer = 0.0f;
                    if (finalTexts.Count <= 0)
                    {
                        showText = false;
                        if (goToNextLevel)
                            NextLevel();
                        else if (gameAllOver)
                            return;
                        else
                            Initialize();
                    }
                }
                return;
            }
            if (gameAllOver)
            {
                // TO RAINER AD GOLD STUFF
                shekelTimeout -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (shekelTimeout < 0 && treasure.Count <= sheckels * 10)
                {
                    shekelTimeout += .05f;
                    int pos = 0;
                    for (int i = 0; i < 4; i++)
                        pos += random.Next(0, 1024);
                    pos /= 4;
                    treasure.Add(new Schekel(new Vector2(pos, 0)));
                }
                foreach (Schekel s in treasure)
                    s.update((float)gameTime.ElapsedGameTime.TotalSeconds);

                return;
            }
            deadStickies = 0;
            savedStickies = 0;
            savedFemales = 0;
            savedFatties = 0;

            if (!moses.inactive && !moses.jumping && Input.spaceBarPressed)
            {
                jumpMarkers.Add(moses.position.X + scrollX);
            }
            if (!(this is Level1))
            {
                if (!moses.inactive && !moses.sprinting && Input.shiftDown)
                {
                    sprintMarkers.Add(moses.position.X + scrollX);
                }
                if (!moses.inactive && moses.sprinting && Input.shiftUp)
                {
                    sprintMarkers.Add(moses.position.X + scrollX);
                }
            }
            for (int i = 0; i < 128; i++)
                collisionBuckets[i] = new List<Stickfigure>(); 
            foreach (Stickfigure s in stickies)
            {
                s.update(dt);
                int index = (int)((s.position.X + scrollX) / (heightMap.Length / 128));
                if (index > -1 && index < 128)
                    collisionBuckets[index].Add(s);
                if (s.dead)
                    deadStickies++;
                if (s.saved)
                {
                    savedStickies++;
                    if (s.isFemale)
                        savedFemales++;
                    if (s.isFattie)
                        savedFatties++;
                }
            }
            moses.update(dt);

            if (!goToNextLevel)
            {
                if (!moses.dead)
                {
                    if (moses.saved && numberOfStickies - deadStickies - savedStickies == 0)
                    {
                        showText = true;
                        textTimer = 0;
                        // Game is over

                        // Update the totals
                        JewSaver.finalSavedFatties += savedFatties;
                        JewSaver.finalSavedFemales += savedFemales;
                        JewSaver.finalSavedStickies += savedStickies;

                        // Gold gets it's own copy of the heightmap
                        hm = new float[1024];
                        for (int i = 0; i < 1024; i++)
                        {
                            hm[i] = LevelBase.heightMap[(int)LevelBase.scrollX + i];
                        }
                        sheckels = 10 * JewSaver.finalSavedStickies
            + 20 * JewSaver.finalSavedFemales
            + 50 * JewSaver.finalSavedFatties;


                        if (this is Level3)
                        {
                            gameAllOver = true;
                            finalTexts.Add("Finally you have reached the Promised Land!");
                            if (JewSaver.finalSavedFemales >= 1 && JewSaver.finalSavedStickies - JewSaver.finalSavedFemales >= 1)
                            {
                                finalTexts.Add("You have saved your people, congratulations!");

                                // Add ending stats
                                finalTexts.Add("Congratulations on completing the game");
                                finalTexts.Add("You managed to save " + JewSaver.finalSavedStickies.ToString() + " stickies in total");
                                finalTexts.Add("Of which " + JewSaver.finalSavedFemales.ToString() + " were female and " + (JewSaver.finalSavedStickies - JewSaver.finalSavedFemales).ToString() + " were male");
                                finalTexts.Add("There were also " + JewSaver.finalSavedFatties.ToString() + " fatties");

                                finalTexts.Add("...");
                                finalTexts.Add("Save the Jew Credits");
                                finalTexts.Add("Developed for the Global Game Jam 2011 by");
                                finalTexts.Add("Rizmari Versfeld");
                                finalTexts.Add("Rainer Dreyer");
                                finalTexts.Add("Justin Crause");

                                gameAllOver = true;
                                return;
                            }

                            if (JewSaver.finalSavedStickies == 0)
                                finalTexts.Add("But you did not bring any followers!");
                            else
                            {
                                if (JewSaver.finalSavedStickies - JewSaver.finalSavedFemales < 1)
                                    finalTexts.Add("But you do not have any males to repopulate!");
                                else if (JewSaver.finalSavedFemales < 1)
                                    finalTexts.Add("But you do not have any females to repopulate!");
                            }
                            finalTexts.Add("You have single-handedly destroyed a nation.");
                            return;
                        }
                        else
                        {
                            finalTexts.Add("You can see the Promised Land in the distance!");
                        }
                        goToNextLevel = true;
                        return;
                    }
                }
                else
                {
                    showText = true;
                    textTimer = 0;
                    finalTexts.Add("The nation is leaderless! Another 40 years in the desert...");
                    finalTexts.Add("You have single-handedly destroyed a nation.");
                    return;
                }
            }

            int mouseX = Input.mousePosition.X;
            // If Moses is saved allow mouse scrolling
            if (moses.saved)
            {
                //Console.WriteLine("Moses Saved!!!");
                if (mouseX < 0)
                {
                    if (scrollX > 0)
                    {
                        float change = (float)gameTime.ElapsedGameTime.TotalSeconds * scrollSpeed;
                        scrollX -= change;
                        foreach (Stickfigure s in stickies)
                            s.position.X += change;
                        foreach (Locust l in locusts)
                            l.pos.X += change;
                        if (scrollX < 0)
                            scrollX = 0;
                    }
                }
                else if (mouseX > JewSaver.width - 1)
                {
                    if (scrollX < heightMap.Length - JewSaver.width - 1)
                    {
                        float change = (float)gameTime.ElapsedGameTime.TotalSeconds * scrollSpeed;
                        scrollX += change;
                        foreach (Stickfigure s in stickies)
                            s.position.X -= change;
                        foreach (Locust l in locusts)
                            l.pos.X -= change;
                        if (scrollX > heightMap.Length - JewSaver.width - 1)
                            scrollX = heightMap.Length - JewSaver.width - 1;
                    }
                }
            }
            else
            {
                if (moses.position.X >= JewSaver.width / 2.0f)
                {
                    float changeX = moses.position.X - JewSaver.width / 2.0f;
                    if (scrollX + changeX + JewSaver.width >= levelLength)
                    {
                        changeX = levelLength - JewSaver.width - 1 - scrollX;
                        if (changeX > 0)
                        {
                            scrollX += changeX;
                            moses.position.X -= changeX;
                            foreach (Stickfigure s in stickies)
                                s.position.X -= changeX;
                            foreach (Locust l in locusts)
                                l.pos.X -= changeX;
                        }
                        else
                        {
                            // Possible segfault over here then uncomment the -1
                            scrollX = levelLength - JewSaver.width;// -1;
                        }
                    }
                    else
                    {

                        scrollX += changeX;
                        moses.position.X -= changeX;
                        foreach (Stickfigure s in stickies)
                            s.position.X -= changeX;
                        foreach (Locust l in locusts)
                            l.pos.X -= changeX;
                    }
                }
            }

            // Locusts are optional
            if (hasLocusts)
            {
                locustTimeout -= gameTime.ElapsedGameTime;
                if (locustTimeout.TotalMilliseconds <= 0)
                {
                    plagueLength = random.Next(8, 15);
                    Console.WriteLine("Adding locusts for " + plagueLength + " seconds");

                    locustTimeout += locustTime;
                }
                else
                {
                    if (plagueLength > 0)
                    {
                        locustSpawner -= gameTime.ElapsedGameTime;
                        if (locustSpawner.TotalMilliseconds <= 0)
                        {
                            locustSpawner = new TimeSpan(0, 0, 0, 0, random.Next(500));

                            int spawnZone = (JewSaver.height / 2) / 3;
                            double rand = random.NextDouble();
                            if (rand > 0.6)
                                locusts.Add(new Locust(new Vector2(levelLength, random.Next(0, spawnZone))));
                            else if (rand > 0.25)
                                locusts.Add(new Locust(new Vector2(levelLength, random.Next(0, spawnZone * 2))));
                            else
                                locusts.Add(new Locust(new Vector2(levelLength, random.Next(0, spawnZone * 3))));
                        }
                    }
                }
                for (int i = 0; i < locusts.Count; )
                {
                    if (locusts[i].dead)
                    {
                        locusts.Remove(locusts[i]);
                    }
                    else
                    {
                        locusts[i].update(dt);
                        i++;
                    }
                }
            }

            foreach (Stickfigure s in stickies)
            {
                if (s.dead || s.inactive)
                    continue;
                if (s.position.Y - 45 > (JewSaver.height / 2))
                    continue;

                foreach (Locust l in locusts)
                {
                    if (l.dead || l.splat)
                        continue;

                    if (l.pos.X - 4 > s.position.X)
                        continue;

                    if (l.pos.X + 4 < s.position.X)
                        continue;

                    if (s.lowestPoint().Y > l.pos.Y && (s.lowestPoint().Y - 45) < l.pos.Y)
                    {
                        l.dead = true;
                        s.dead = true;
                        break;
                    }
                }
            }
        }

        // Update the trees
        if (trees != null)
        {
            foreach (Tree tree in trees)
            {
                tree.update(heightMap);
            }
        }

        if (showFrameRate)
        {
            fpsTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (fpsTime >= 1.0)
            {
                Console.WriteLine(frames.ToString() + " FPS");
                frames = 0;
                fpsTime = 0.0;
            }
        }
    }

    private void Sculpt(int startIndex, int midIndex, int endIndex, GameTime gameTime, int top)
    {
        float start = heightMap[startIndex];
        float end = heightMap[endIndex - 1];
        float target = JewSaver.height - top;
        float timerMultiplier = 4;
        if (canSculpt[startIndex] == TerrainType.SAND)
        {
            if (target > 256)
                target = 256;
            else if (target < 16)
                target = 16;
            timerMultiplier = 4;
        }
        else if (canSculpt[startIndex] == TerrainType.PARCHED_LAND)
        {
            if (target > 192)
                target = 192;
            else if (target < 16)
                target = 16;
            timerMultiplier = 2;
        }
        for (int i = startIndex; i < midIndex; i++)
        {
            float cubicValue = CosineInterpolate(start, target, (i - startIndex) / (float)(midIndex - startIndex));
            heightMap[i] += (float)((cubicValue - heightMap[i]) * gameTime.ElapsedGameTime.TotalSeconds) * timerMultiplier;
        }
        for (int i = midIndex; i < endIndex; i++)
        {
            float cubicValue = CosineInterpolate(target, end, (i - midIndex) / (float)(endIndex - midIndex));
            heightMap[i] += (float)((cubicValue - heightMap[i]) * gameTime.ElapsedGameTime.TotalSeconds) * timerMultiplier;
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
        if (showFrameRate)
            JewSaver.spriteBatch.DrawString(font, 1 / gameTime.ElapsedGameTime.TotalSeconds + "", new Vector2(512, 8), Color.White);
        JewSaver.spriteBatch.End();

        JewSaver.primitiveBatch.Begin(PrimitiveType.LineList);

        for (int i = (int)scrollX; i < (int)scrollX + JewSaver.width; i++)
        {
            if (canSculpt[i] == TerrainType.WATER)
            {
                heightMap[i] = 16 + 4 * (float)Math.Sin(i / 64.0f * Math.PI * 2 + 3 * levelTime);
                JewSaver.primitiveBatch.AddVertex(new Vector2(i - (int)scrollX, JewSaver.height - heightMap[i]), Color.CadetBlue);
                JewSaver.primitiveBatch.AddVertex(new Vector2(i - (int)scrollX, JewSaver.height), Color.DarkBlue);
            }
            else if (canSculpt[i] == TerrainType.ROCK)
            {
                JewSaver.primitiveBatch.AddVertex(new Vector2(i - (int)scrollX, JewSaver.height - heightMap[i]), Color.DarkGray);
                JewSaver.primitiveBatch.AddVertex(new Vector2(i - (int)scrollX, JewSaver.height), Color.Black);
            }
            else if (canSculpt[i] == TerrainType.PARCHED_LAND)
            {
                JewSaver.primitiveBatch.AddVertex(new Vector2(i - (int)scrollX, JewSaver.height - heightMap[i]), Color.Khaki);
                JewSaver.primitiveBatch.AddVertex(new Vector2(i - (int)scrollX, JewSaver.height), Color.Peru);
            }
            else
            {
                JewSaver.primitiveBatch.AddVertex(new Vector2(i - (int)scrollX, JewSaver.height - heightMap[i]), Color.Gold);
                JewSaver.primitiveBatch.AddVertex(new Vector2(i - (int)scrollX, JewSaver.height), Color.Sienna);
            }
        }
        if (levelMode == LevelMode.EDIT)
        {
            JewSaver.primitiveBatch.AddLine(new Vector2(landscapeBrush.Left, landscapeBrush.Top), new Vector2(landscapeBrush.Right, landscapeBrush.Top), Color.YellowGreen, Color.DarkGreen, 3);
            JewSaver.primitiveBatch.AddLine(new Vector2(landscapeBrush.Right, landscapeBrush.Top), new Vector2(landscapeBrush.Right, landscapeBrush.Bottom), Color.DarkGreen, Color.YellowGreen, 3);
            JewSaver.primitiveBatch.AddLine(new Vector2(landscapeBrush.Right, landscapeBrush.Bottom), new Vector2(landscapeBrush.Left, landscapeBrush.Bottom), Color.YellowGreen, Color.DarkGreen, 3);
            JewSaver.primitiveBatch.AddLine(new Vector2(landscapeBrush.Left, landscapeBrush.Bottom), new Vector2(landscapeBrush.Left, landscapeBrush.Top), Color.DarkGreen, Color.YellowGreen, 3);
        }
        JewSaver.primitiveBatch.End();

        // Draw the trees
        if (trees != null)
        {
            for (int i = 0; i < trees.Length; i++)
            {
                trees[i].scrollXValue = scrollX;
                trees[i].draw();
            }
        }

        // Return here if end of game
        if (gameAllOver)
        {
            // TODO RAINER ADD GOLD RENDER HERE
            JewSaver.primitiveBatch.Begin(PrimitiveType.LineList);
            foreach (Schekel s in treasure)
                s.draw();
            JewSaver.primitiveBatch.End();

            //Debugging contour!
            /*JewSaver.primitiveBatch.Begin(PrimitiveType.PointList);
            for (int i = 0; i < 1024; i++)
                JewSaver.primitiveBatch.AddVertex(new Vector2(i, hm[i]), Color.Red);
            JewSaver.primitiveBatch.End();*/
        }
        else
        {
            // Edit mode options
            if (levelMode == LevelMode.EDIT)
            {
                JewSaver.spriteBatch.Begin();
                (play as MenuInputElement).Draw(JewSaver.spriteBatch);

                // Show text saying "Edit Mode"
                String text = "Edit Mode";
                Vector2 centre = new Vector2((JewSaver.width - font.MeasureString(text).X) / 2.0f, 10.0f);
                JewSaver.spriteBatch.DrawString(font, text, centre + new Vector2(-2.0f + 1.0f), Color.Black);
                JewSaver.spriteBatch.DrawString(font, text, centre, Color.White);

                JewSaver.spriteBatch.End();
            }
            else if (levelMode == LevelMode.PLAY)
            {
                // Draw the jump markers
                for (int i = 0; i < jumpMarkers.Count; i++)
                    JewSaver.primitiveBatch.DrawCircle(new Vector2(jumpMarkers[i] - scrollX, JewSaver.height - heightMap[Math.Min(Math.Max(0, (int)(jumpMarkers[i])), levelLength - 1)]), Color.Blue, 5);

                // Draw the sprint markers
                for (int i = 0; i < sprintMarkers.Count; i++)
                {
                    Color col = Color.Green;
                    if (i % 2 != 0)
                        col = Color.Red;

                    JewSaver.primitiveBatch.DrawCircle(new Vector2(sprintMarkers[i] - scrollX, JewSaver.height - heightMap[Math.Min(Math.Max(0, (int)(sprintMarkers[i])), levelLength - 1)]), col, 5);
                }

                // Draw stickies
                foreach (Stickfigure s in stickies)
                {
                    s.draw();
                }
                moses.draw();

                JewSaver.primitiveBatch.Begin(PrimitiveType.LineList);
                foreach (Locust l in locusts)
                    l.draw();
                JewSaver.primitiveBatch.End();

                JewSaver.spriteBatch.Begin();
                if (!goToNextLevel)
                    (restart as MenuInputElement).Draw(JewSaver.spriteBatch);

                if (!showText)
                {
                    // Show number of jews still alive
                    String text = "Jews Still Alive: " + (numberOfStickies - deadStickies).ToString();
                    Vector2 centre = new Vector2((JewSaver.width - font.MeasureString(text).X) / 2.0f, 10.0f);
                    JewSaver.spriteBatch.DrawString(font, text, centre + new Vector2(-2.0f + 1.0f), Color.Black);
                    JewSaver.spriteBatch.DrawString(font, text, centre, Color.White);

                    // Show number of jews that have been saved
                    text = "Jews Saved: " + savedStickies.ToString() + " (M: " + (savedStickies - savedFemales).ToString() + " F: " + savedFemales.ToString() + ")";
                    centre = new Vector2((JewSaver.width - font.MeasureString(text).X) / 2.0f, 10.0f + font.LineSpacing);
                    JewSaver.spriteBatch.DrawString(font, text, centre + new Vector2(-2.0f + 1.0f), Color.Black);
                    JewSaver.spriteBatch.DrawString(font, text, centre, Color.White);
                }
                else
                {
                    // Show text saying either "Level Complete" or "Game Over"
                    String text = "";
                    if (gameAllOver)
                        text = "Game Completed";
                    else if (goToNextLevel)
                        text = "Level Complete";
                    else
                        text = "Game Over";
                    Vector2 centre = new Vector2((JewSaver.width - MenuJewSaver.font.MeasureString(text).X) / 2.0f, 30.0f);
                    JewSaver.spriteBatch.DrawString(MenuJewSaver.font, text, centre + new Vector2(-2.0f + 1.0f), Color.Black);
                    JewSaver.spriteBatch.DrawString(MenuJewSaver.font, text, centre, Color.White);
                }
                JewSaver.spriteBatch.End();
            }
        }
        // Final text if applicable
        JewSaver.spriteBatch.Begin();
        if (showText || openingText)
        {
            if (finalTexts.Count <= 0)
            {
                showText = false;
                openingText = false;
                return;
            }
            Vector2 measure = MenuJewSaver.font.MeasureString(finalTexts[0]);
            Vector2 centre = new Vector2((JewSaver.width - measure.X) / 2.0f, (JewSaver.height - measure.Y) / 2.0f);
            JewSaver.spriteBatch.DrawString(MenuJewSaver.font, finalTexts[0], centre, new Color(1, 1, 1, (float)Math.Sin(textTimer / 8.0f * 2 * Math.PI)));
        }
        (exit as MenuInputElement).Draw(JewSaver.spriteBatch);
        JewSaver.spriteBatch.End();

        if (showFrameRate)
            frames++;
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
        scrollX = 0;
        openingText = false;
        finalTexts.Clear();
    }

    private void OnQuitPressed()
    {
        jewSaver.SwitchState(GameState.MAIN_MENU);
    }

    private void OnRestartPressed()
    {
        this.Initialize();
    }

    protected void AddCanyon(int start, int end)
    {
        float mid = (end - start) / 2.0f;
        for (int i = start; i < (end - start) / 2 + start; i++)
        {
            heightMap[i] = CosineInterpolate(heightMap[start], -256, (i - start) / mid);
            canSculpt[i] = heightMap[i] >= 16 ? TerrainType.SAND : TerrainType.CANYON;
        }
        for (int i = (end - start) / 2 + start; i <= end; i++)
        {
            heightMap[i] = CosineInterpolate(-256, heightMap[end], (i - ((int)mid + start)) / mid);
            canSculpt[i] = heightMap[i] >= 16 ? TerrainType.SAND : TerrainType.CANYON;
        }
    }

    /// <summary>
    /// Only after adding canyons!!!
    /// </summary>
    protected void AddTrees()
    {
        int treeNum = (int)random.Next((int)(30 * heightMap.Length / 4096.0f), (int)(60 * heightMap.Length / 4096.0f));
        trees = new Tree[treeNum];
        int i = 0;
        while (i < treeNum)
        {
            int xVal = random.Next(0, heightMap.Length);
            if (canSculpt[xVal] == TerrainType.SAND || canSculpt[xVal] == TerrainType.PARCHED_LAND)
            {
                trees[i] = new Tree(new Vector2(xVal, JewSaver.height - 20));
                i++;
            }
        }
    }

    protected void AddWater(int start, int end)
    {
        int length = end - start;
        for (int i = start; i < end; i++)
        {
            heightMap[i] = 16 + 4 * (float)Math.Sin(i / 64.0f * Math.PI * 2);
            canSculpt[i] = TerrainType.WATER;
        }
        float sandHeight = heightMap[start - 64];
        for (int i = start - 64; i < start; i++)
        {
            heightMap[i] = CosineInterpolate(sandHeight, 20, (i - start + 64) / 64.0f);
        }
        sandHeight = heightMap[end + 64];
        for (int i = end; i < end + 64; i++)
        {
            heightMap[i] = CosineInterpolate(20, sandHeight, (i - end) / 64.0f);
        }
    }

    protected void AddRocks(int start, int end)
    {
        for (int i = start; i < end; i++)
        {
            canSculpt[i] = TerrainType.ROCK;
        }
    }

    protected virtual void NextLevel()
    {
        // override in child classes
    }
}
