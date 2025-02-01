using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Audio; 
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace PuzzleBubble;

//_audioManager.Load(Content, "Name", "Location");

public class MainScene : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    SpriteFont _font;
    Texture2D _rect, _background;

    List<GameObject> _gameObjects;
    private int _numObjects;
    Texture2D lineTexture;
    // โหลดเสียงยิงปืน (SoundEffect)

// โหลดเพลงพื้นหลัง (Song)
    private Song backgroundMusic;
    public SoundEffect fireSound;

    public MainScene()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = Singleton.SCREENWIDTH;
        _graphics.PreferredBackBufferHeight = Singleton.SCREENHEIGHT;
        _graphics.ApplyChanges();

        _gameObjects = new List<GameObject>();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("GameFont");
        _background = Content.Load<Texture2D>("BG_Sprite");
        

        // Rectangle for drawing a background
        _rect = new Texture2D(GraphicsDevice, 1, 1);
        _rect.SetData(new Color[] { Color.White });
        lineTexture = new Texture2D(GraphicsDevice, 1, 1);
        lineTexture.SetData(new Color[] { Color.Red });

        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("GameFont");
        _background = Content.Load<Texture2D>("BG_Sprite");

        backgroundMusic = Content.Load<Song>("BGM"); 
        fireSound = Content.Load<SoundEffect>("FireShoot"); 
        MediaPlayer.Play(backgroundMusic);
        MediaPlayer.IsRepeating = true;


        // Call Reset to initialize the game
        Reset();
    }

    protected override void Update(GameTime gameTime)
    {
        Singleton.Instance.CurrentKey = Keyboard.GetState();

        //update
        _numObjects = _gameObjects.Count;
        // Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;

        switch (Singleton.Instance.CurrentGameState)
        {
            case Singleton.GameState.Start:
                Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                break;
            case Singleton.GameState.GamePlaying:
                Singleton.Instance.Timer += gameTime.ElapsedGameTime.Ticks;
                for (int i = 0; i < _numObjects; i++)
                {
                    if (_gameObjects[i].IsActive)
                        _gameObjects[i].Update(gameTime, _gameObjects);
                }
                for (int i = 0; i < _numObjects; i++)
                {
                    if (!_gameObjects[i].IsActive)
                    {
                        _gameObjects.RemoveAt(i);
                        i--;
                        _numObjects--;
                    }
                }
                if (Singleton.Instance.BubbleLeft <= 0)
                {
                    ResetBubble();

                    Console.WriteLine("Resetting Bubble123");
                    foreach (GameObject s in _gameObjects)
                    {
                        if (s is Bubble)
                        {
                            s.Reset();
                        }
                    }

                }
                foreach (GameObject g in _gameObjects)
                {
                    if (g is BubbleGrid bubble && bubble.Position.Y >= 870)
                    {
                        // Game over logic if bubble crosses red line
                        Singleton.Instance.CurrentGameState = Singleton.GameState.GameOver;
                    }
                }
                break;
            case Singleton.GameState.GameOver:
                if (!Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey) && Singleton.Instance.CurrentKey.GetPressedKeys().Length > 0)
                {
                    //     // any key to restart
                    Reset();
                    Singleton.Instance.CurrentGameState = Singleton.GameState.Start;
                }
                break;

        }


        Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;

        base.Update(gameTime);
    }


    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.YellowGreen);

        _spriteBatch.Begin();

        // Base Background
        _spriteBatch.Draw(_background, new Vector2(0, 0), new Rectangle(9, 2615, 1921, 1081), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f); // Background
        _spriteBatch.Draw(_background, new Vector2(1428, 550), new Rectangle(14, 506, 439, 449), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f); // Tree Coconut
        _spriteBatch.Draw(_background, new Vector2(1400, 511), new Rectangle(11, 322, 425, 100), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f); // Scoreboard
        _spriteBatch.Draw(_background, new Vector2(1400, 631), new Rectangle(9, 196, 425, 100), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f); // Time
        _spriteBatch.Draw(_background, new Vector2(40, 550), new Rectangle(26, 977, 426, 520), Color.White, 0f, Vector2.Zero, new Vector2(0.9f, 0.9f), SpriteEffects.None, 0f); // Island & Tree
        _spriteBatch.Draw(_background, new Vector2(1708, 999), new Rectangle(159, 20, 50, 50), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f); // Yellow Button
        _spriteBatch.Draw(_background, new Vector2(1770, 999), new Rectangle(89, 20, 50, 50), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f); // Red Button
        _spriteBatch.Draw(_background, new Vector2(1832, 999), new Rectangle(18, 20, 50, 50), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f); // Green Button
        _spriteBatch.Draw(_rect, new Vector2(519, 65), null, Color.LightGray /* * 0.5f for transparent*/, 0f, Vector2.Zero, new Vector2(Singleton.PlayWidth, Singleton.PlayHeight), SpriteEffects.None, 0f); // Play Area
        _spriteBatch.Draw(_rect, new Vector2(519, 980), null, Color.BurlyWood, 0f, Vector2.Zero, new Vector2(Singleton.PlayWidth, 100), SpriteEffects.None, 0f); // Under Play Area
        DrawRectangleWithOutline(_spriteBatch, _rect, new Rectangle(519, 60, Singleton.PlayWidth, Singleton.PlayHeight), Color.DimGray, 40); // Play Area Outline // Play Area Outline
        DrawLine(new Vector2(559, 870), new Vector2(1259, 870));
        // _spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, outlineThickness), outlineColor); // Up


        // Get Mouse State X,Y
        MouseState mouseState = Mouse.GetState();
        int mouseX = mouseState.X;
        int mouseY = mouseState.Y;

        // Draw mouse coordinates
        string coordinates = $"X: {mouseState.X}, Y: {mouseState.Y}";
        _spriteBatch.DrawString(_font, coordinates, new Vector2(10, 10), Color.Red);

        // Font Score & Time
        Vector2 fontSize = _font.MeasureString("Score: " + Singleton.Instance.Score.ToString());
        _spriteBatch.DrawString(_font, "Score: " + Singleton.Instance.Score.ToString(), new Vector2(1460, 545), Color.White);
        _spriteBatch.DrawString(_font, "TIME: " + String.Format("{0}:{1:00}", Singleton.Instance.Timer / 600000000, Singleton.Instance.Timer / 10000000 % 60), new Vector2(1460, 665), Color.White);

          _numObjects = _gameObjects.Count;

                for (int i = 0; i < _numObjects; i++)
                {
                    // if (_gameObjects[i] is BubbleGrid)
                    // {
                    //     Console.WriteLine($"Drawing BubbleGrid: {_gameObjects[i].Name}"); // Debug
                    // }
                    _gameObjects[i].Draw(_spriteBatch);
                }

        // Draw Gameover in the middle of the screen
        switch (Singleton.Instance.CurrentGameState)
        {
            case Singleton.GameState.GamePlaying:
                { // Add new row of bubbles
                    Singleton.Instance.TimeDown += gameTime.ElapsedGameTime.Ticks;

                    if (Singleton.Instance.TimeDown >= TimeSpan.TicksPerSecond * 10)
                    {
                        Singleton.Instance.TimeDown = 0;
                        MoveBubblesDown();
                        ResetBubble();
                        // AddNewRow();
                    }
                }
                break;

            case Singleton.GameState.GameOver:
                fontSize = _font.MeasureString("Game Over");
                _spriteBatch.DrawString(_font, "Game Over", new Vector2((965 - fontSize.X)  - 2, (555 - fontSize.Y) - 2), Color.White);
                _spriteBatch.DrawString(_font, "Game Over", new Vector2((965 - fontSize.X)  + 2, (555 - fontSize.Y)  - 2), Color.White);
                _spriteBatch.DrawString(_font, "Game Over", new Vector2((965 - fontSize.X)  + 2, (555 - fontSize.Y)  + 2), Color.White);
                _spriteBatch.DrawString(_font, "Game Over", new Vector2((965 - fontSize.X)  - 2, (555 - fontSize.Y)  + 2), Color.White);
                _spriteBatch.DrawString(_font, "Game Over", new Vector2((965 - fontSize.X) , (555 - fontSize.Y) ), Color.Red);
            break;
        }

        _spriteBatch.End();
        _graphics.BeginDraw();

        base.Draw(gameTime);
    }

    public void DrawLine(Vector2 start, Vector2 end)
    {
        // Calculate the length and angle of the line
        float length = Vector2.Distance(start, end);
        float angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);

        // Draw the line (stretch the 1x1 texture to the calculated length)
        _spriteBatch.Draw(lineTexture, start, null, Color.Red, angle, Vector2.Zero, new Vector2(length, 1), SpriteEffects.None, 0f);
    }

    protected void Reset()
    {
        Singleton.Instance.CurrentGameState = Singleton.GameState.Start;
        Singleton.Instance.Score = 0;
        Singleton.Instance.Random = new System.Random();
        Singleton.Instance.totalRows = 5;

        Texture2D BubblePuzzleTexture = Content.Load<Texture2D>("Sprite2");

        // Reset the game objects
        _gameObjects.Clear();

        // Add Gun
        _gameObjects.Add(new Gun(BubblePuzzleTexture)
        {
            Name = "Gun",
            Viewport = new Rectangle(8, 789, 104, 210),
            Position = new Vector2((Singleton.PlayWidth / 2) + 515, Singleton.PlayHeight + 130),
            Left = Keys.Left,
            Right = Keys.Right,
            Fire = Keys.Space,
            bubbleBulletYellow = new BubbleBullet(BubblePuzzleTexture)
            {
                Name = "BubbleBulletYellow",
                Viewport = new Rectangle(21, 21, 70, 70),
                Velocity = new Vector2(0, 60f),
            },
            bubbleBulletBlue = new BubbleBullet(BubblePuzzleTexture)
            {
                Name = "BubbleBulletBlue",
                Viewport = new Rectangle(22, 132, 70, 70),
                Velocity = new Vector2(0, 60f),
            },
            bubbleBulletBrown = new BubbleBullet(BubblePuzzleTexture)
            {
                Name = "BubbleBulletBrown",
                Viewport = new Rectangle(20, 350, 70, 70),
                Velocity = new Vector2(0, 60f),
            },
            bubbleBulletBlack = new BubbleBullet(BubblePuzzleTexture)
            {
                Name = "BubbleBulletBlack",
                Viewport = new Rectangle(20, 460, 70, 70),
                Velocity = new Vector2(0, 60f),
            },
            bubbleBulletRed = new BubbleBullet(BubblePuzzleTexture)
            {
                Name = "BubbleBulletRed",
                Viewport = new Rectangle(20, 240, 70, 70),
                Velocity = new Vector2(0, 60f),
            },
    
            fireSound = fireSound

        });

        ResetBubble();

        // Reset All Objects
        foreach (GameObject s in _gameObjects)
        {
            Console.WriteLine($"Resetting GameObject: {s.Name}");
            s.Reset();
        }

        Singleton.Instance.Score = 0;
        Singleton.Instance.Timer = 0;
    }

    // private void ResetBubble()   # Old Version Keep for Reference
    // {
    //     Texture2D BubblePuzzleTexture = Content.Load<Texture2D>("SpriteSheet");
    //     Singleton.Instance.BubbleLeft = 48;

    //     //  Each Color of Bubble in the SpriteSheet
    //     Rectangle[] bubbleColors = new Rectangle[]
    //     {
    //     new Rectangle(21, 21, 70, 70),   // Yellow
    //     new Rectangle(22, 132, 70, 70),  // Blue
    //     new Rectangle(20, 240, 70, 70),  // Red
    //     new Rectangle(20, 350, 70, 70),  // Brown
    //     new Rectangle(20, 460, 70, 70)   // Black
    //     };

    //     int Singleton.Instance.totalRowsg = 5; // จำนวนแถวทั้งหมด
    //     for (int row = 0; row < Singleton.Instance.totalRowsg; row++)  // ✅ วนลูปเพิ่มแถว
    //     {
    //         int columns = (row % 2 == 0) ? 10 : 9; // ✅ สลับจำนวนคอลัมน์ระหว่าง 10 และ 9
    //         for (int col = 0; col < columns; col++)
    //         {
    //             var clone = new BubbleGrid(BubblePuzzleTexture)
    //             {
    //                 Name = "BubbleGrid",
    //                 Score = 30,
    //                 Velocity = new Vector2(0, 0),
    //                 Viewport = bubbleColors[Singleton.Instance.Random.Next(bubbleColors.Length)]
    //             };

    //             // Random Color of Bubble
    //             // clone.Viewport = bubbleColors[Singleton.Instance.Random.Next(bubbleColors.Length)];

    //             // Set Position
    //             float posX = 560 + (70 * col) + (35 * (row % 2)); // ขยับแถวที่เป็นเลขคี่
    //             float posY = 100 + (70 * row); // ✅ เพิ่มค่าความสูงแต่ละแถว

    //             clone.Position = new Vector2(posX, posY); // ✅ แก้ให้วาง Bubble ในแนวตั้งตามแถวที่ต้องการ

    //             _gameObjects.Add(clone);
    //             Console.WriteLine($"Added Bubble at X:{posX}, Y:{posY}, Row:{row}, Col:{col}");
    //         }
    //     }
    // }

    private void ResetBubble()
    {
        Texture2D BubblePuzzleTexture = Content.Load<Texture2D>("SpriteSheet");
        Singleton.Instance.BubbleLeft = 48;

        // Each Color of Bubble in the SpriteSheet
        Rectangle[] bubbleColors = new Rectangle[]
        {
            new Rectangle(21, 21, 70, 70),   // Yellow
            new Rectangle(22, 132, 70, 70),  // Blue
            new Rectangle(20, 240, 70, 70),  // Red
            new Rectangle(20, 350, 70, 70),  // Brown
            new Rectangle(20, 460, 70, 70)   // Black
        };

        string[] bubbleColorNames = new string[]
        {
            "Yellow",
            "Blue",
            "Red",
            "Brown",
            "Black"
        };

        if (Singleton.Instance.totalRows - 1 < 5)
        {
            for (int row = 0; row < Singleton.Instance.totalRows; row++)
            {
                int columns = (row % 2 == 0) ? 10 : 9; // Alternate between 10 and 9 columns
                for (int col = 0; col < columns; col++)
                {
                    int colorIndex = Singleton.Instance.Random.Next(bubbleColors.Length);
                    var clone = new BubbleGrid(BubblePuzzleTexture)
                    {
                        Name = "BubbleGrid" + bubbleColorNames[colorIndex],
                        Score = 30,
                        Velocity = new Vector2(0, 0),
                        Viewport = bubbleColors[colorIndex]
                    };

                    // Set Position
                    float posX = 560 + (70 * col) + (35 * (row % 2)); // Adjust odd rows
                    float posY = Singleton.Instance.totalRows >= 5 ? 100 + (70 * row) : 100; // Increase height for each row

                    clone.Position = new Vector2(posX, posY); // Place Bubble in the desired vertical row

                    _gameObjects.Add(clone);
                    Console.WriteLine($"Added Bubble at X:{posX}, Y:{posY}, Row:{row}, Col:{col}, Name:{clone.Name}");
                }
            }
        }
        else if (Singleton.Instance.totalRows >= 5)
        {
            int columns = (Singleton.Instance.totalRows % 2 == 0) ? 9 : 10; // Alternate between 10 and 9 columns
            for (int col = 0; col < columns; col++)
            {
                int colorIndex = Singleton.Instance.Random.Next(bubbleColors.Length);
                var clone = new BubbleGrid(BubblePuzzleTexture)
                {
                    Name = "BubbleGrid" + bubbleColorNames[colorIndex],
                    Score = 30,
                    Velocity = new Vector2(0, 0),
                    Viewport = bubbleColors[colorIndex]
                };

                // Set Position
                float posX = columns == 9 ? 595 + (70 * col) : 560 + (70 * col); // Adjust odd rows
                float posY = 100; // Increase height for each row

                clone.Position = new Vector2(posX, posY); // Place Bubble in the desired vertical row
                Console.WriteLine($"Added Bubble at X:{posX}, Y:{posY}, Row:{Singleton.Instance.totalRows}, Col:{col}, Name:{clone.Name}");

                _gameObjects.Add(clone);
            }
        }
        ++Singleton.Instance.totalRows;
    }

    private void MoveBubblesDown()
    {
        foreach (var gameObject in _gameObjects)
        {
            if (gameObject is BubbleGrid bubble)
            {
                bubble.Position = new Vector2(bubble.Position.X, bubble.Position.Y + Singleton.SizeBubbleHeight);
            }
        }
    }

    // private void AddNewRow()
    // {
    //     Texture2D BubblePuzzleTexture = Content.Load<Texture2D>("SpriteSheet");
    //     Rectangle[] bubbleColors = new Rectangle[]
    //     {
    //     new Rectangle(21, 21, 70, 70),   // Yellow
    //     new Rectangle(22, 132, 70, 70),  // Blue
    //     new Rectangle(20, 240, 70, 70),  // Red
    //     new Rectangle(20, 350, 70, 70),  // Brown
    //     new Rectangle(20, 460, 70, 70)   // Black
    //     };

    //     string[] bubbleColorNames = new string[]
    //     {
    //         "Yellow",
    //         "Blue",
    //         "Red",
    //         "Brown",
    //         "Black"
    //     };

    //     Console.WriteLine($"Adding New Row: {Singleton.Instance.totalRowsg + 1}");
    //     int columns = (Singleton.Instance.totalRowsg % 2 == 0) ? 10 : 9; ++Singleton.Instance.totalRowsg;
    //     for (int col = 0; col < columns; col++)
    //     {
    //         int colorIndex = Singleton.Instance.Random.Next(bubbleColors.Length);
    //         var clone = new BubbleGrid(BubblePuzzleTexture)
    //         {
    //             Name = "BubbleGrid" + bubbleColorNames[colorIndex],
    //             Score = 30,
    //             Velocity = new Vector2(0, 0),
    //             Viewport = bubbleColors[colorIndex]
    //         };

    //         // Set Position
    //         float posX = columns == 9 ? 595 + (70 * col) : 560 + (70 * col); // Adjust odd rows
    //         float posY = 100; // Increase height for each row

    //         clone.Position = new Vector2(posX, posY); // Place Bubble in the desired vertical row

    //         _gameObjects.Add(clone);
    //         Console.WriteLine($"Added Bubble at X:{posX}, Y:{posY}, Row:{Singleton.Instance.totalRowsg}, Col:{col}, Name:{clone.Name}");
    //     }
    // }

    protected void DrawEx()
    {
        _spriteBatch.Draw(_background, new Vector2(0, 0), new Rectangle(9, 2615, 1921, 1081), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f); // Background
        _spriteBatch.Draw(_background, new Vector2(1428, 550), new Rectangle(14, 506, 439, 449), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f); // Tree Coconut
        _spriteBatch.Draw(_background, new Vector2(1400, 511), new Rectangle(11, 322, 425, 100), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f); // Scoreboard
        _spriteBatch.Draw(_background, new Vector2(1400, 631), new Rectangle(9, 196, 425, 100), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f); // Time
        _spriteBatch.Draw(_background, new Vector2(40, 550), new Rectangle(26, 977, 426, 520), Color.White, 0f, Vector2.Zero, new Vector2(0.9f, 0.9f), SpriteEffects.None, 0f); // Island & Tree
        _spriteBatch.Draw(_background, new Vector2(1708, 999), new Rectangle(159, 20, 50, 50), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f); // Yellow Button
        _spriteBatch.Draw(_background, new Vector2(1770, 999), new Rectangle(89, 20, 50, 50), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f); // Red Button
        _spriteBatch.Draw(_background, new Vector2(1832, 999), new Rectangle(18, 20, 50, 50), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f); // Green Button
        _spriteBatch.Draw(_rect, new Vector2(519, 65), null, Color.LightGray /* * 0.5f for transparent*/, 0f, Vector2.Zero, new Vector2(Singleton.PlayWidth, Singleton.PlayHeight), SpriteEffects.None, 0f); // Play Area
        _spriteBatch.Draw(_rect, new Vector2(519, 980), null, Color.BurlyWood, 0f, Vector2.Zero, new Vector2(Singleton.PlayWidth, 100), SpriteEffects.None, 0f); // Under Play Area
        DrawRectangleWithOutline(_spriteBatch, _rect, new Rectangle(519, 60, Singleton.PlayWidth, Singleton.PlayHeight), Color.DimGray, 40); // Play Area Outline // Play Area Outline
        DrawLine(new Vector2(559, 870), new Vector2(1259, 870));
    }

    protected void DrawRectangleWithOutline(SpriteBatch spriteBatch, Texture2D pixel, Rectangle rect, Color outlineColor, int outlineThickness)
    {
        // Draw Outline
        spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, outlineThickness), outlineColor); // Up
        spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y + 5, outlineThickness, rect.Height), outlineColor); // Left
        spriteBatch.Draw(pixel, new Rectangle(rect.X + rect.Width - outlineThickness, rect.Y + 5, outlineThickness, rect.Height), outlineColor); // Right
        // spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y + rect.Height - outlineThickness, rect.Width, outlineThickness), outlineColor); // Down
    }
}