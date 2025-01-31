using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
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
    private int totalRows = 5;

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
        _spriteBatch.Draw(_background, new Vector2(1400, 511), new Rectangle(11, 322, 425, 100), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f); // Scoreboard
        _spriteBatch.Draw(_background, new Vector2(1400, 631), new Rectangle(9, 196, 425, 100), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f); // Time
        // _spriteBatch.Draw(_background, new Vector2(83, 92), new Rectangle(9, 196, 425, 100), Color.White, 0f, Vector2.Zero, new Vector2(0.8f, 0.39f), SpriteEffects.None, 0f); // Name Bar
        _spriteBatch.Draw(_background, new Vector2(40, 550), new Rectangle(26, 977, 426, 520), Color.White, 0f, Vector2.Zero, new Vector2(0.9f, 0.9f), SpriteEffects.None, 0f); // Island & Tree
        _spriteBatch.Draw(_background, new Vector2(1708, 999), new Rectangle(159, 20, 50, 50), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f); // Yellow Button
        _spriteBatch.Draw(_background, new Vector2(1770, 999), new Rectangle(89, 20, 50, 50), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f); // Red Button
        _spriteBatch.Draw(_background, new Vector2(1832, 999), new Rectangle(18, 20, 50, 50), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f); // Green Button
        _spriteBatch.Draw(_rect, new Vector2(515, 65), null, Color.LightGray /* * 0.5f for transparent*/, 0f, Vector2.Zero, new Vector2(Singleton.PlayWidth, Singleton.PlayHeight), SpriteEffects.None, 0f); // Play Area
        _spriteBatch.Draw(_rect, new Vector2(515, 980), null, Color.BurlyWood, 0f, Vector2.Zero, new Vector2(Singleton.PlayWidth, 100), SpriteEffects.None, 0f); // Under Play Area
        DrawRectangleWithOutline(_spriteBatch, _rect, new Rectangle(515, 65, Singleton.PlayWidth, Singleton.PlayHeight), Color.Black, 5); // Play Area Outline


        _numObjects = _gameObjects.Count;

        for (int i = 0; i < _numObjects; i++)
        {
            if (_gameObjects[i] is BubbleGrid)
            {
                Console.WriteLine($"Drawing BubbleGrid: {_gameObjects[i].Name}"); // Debug
            }
            _gameObjects[i].Draw(_spriteBatch);
        }

        { // Add new row of bubbles
            Singleton.Instance.Timer += gameTime.ElapsedGameTime.Ticks;

            if (Singleton.Instance.Timer >= TimeSpan.TicksPerSecond * 10)
            {
                Singleton.Instance.Timer = 0;
                MoveBubblesDown();
                AddNewRow();
            }
        }

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


        // Draw Gameover in the middle of the screen
        if (Singleton.Instance.CurrentGameState == Singleton.GameState.GameOver)
        {
            fontSize = _font.MeasureString("Game Over");
            _spriteBatch.DrawString(_font, "Game Over", new Vector2((Singleton.SCREENWIDTH - fontSize.X) / 2 - 2, (Singleton.SCREENHEIGHT - fontSize.Y) / 2 - 2), Color.White);
            _spriteBatch.DrawString(_font, "Game Over", new Vector2((Singleton.SCREENWIDTH - fontSize.X) / 2 + 2, (Singleton.SCREENHEIGHT - fontSize.Y) / 2 - 2), Color.White);
            _spriteBatch.DrawString(_font, "Game Over", new Vector2((Singleton.SCREENWIDTH - fontSize.X) / 2 + 2, (Singleton.SCREENHEIGHT - fontSize.Y) / 2 + 2), Color.White);
            _spriteBatch.DrawString(_font, "Game Over", new Vector2((Singleton.SCREENWIDTH - fontSize.X) / 2 - 2, (Singleton.SCREENHEIGHT - fontSize.Y) / 2 + 2), Color.White);
            _spriteBatch.DrawString(_font, "Game Over", new Vector2((Singleton.SCREENWIDTH - fontSize.X) / 2, (Singleton.SCREENHEIGHT - fontSize.Y) / 2), Color.Red);

        }

        _spriteBatch.End();
        _graphics.BeginDraw();

        base.Draw(gameTime);
    }

    protected void Reset()
    {
        Singleton.Instance.CurrentGameState = Singleton.GameState.Start;
        Singleton.Instance.Score = 0;
        Singleton.Instance.Random = new System.Random();


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
            }
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

    // private void ResetBubble()   # Old Version Keep It for Reference
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

    //     int totalRows = 5; // จำนวนแถวทั้งหมด
    //     for (int row = 0; row < totalRows; row++)  // ✅ วนลูปเพิ่มแถว
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

        for (int row = 0; row < totalRows; row++)
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
                float posY = 100 + (70 * row); // Increase height for each row

                clone.Position = new Vector2(posX, posY); // Place Bubble in the desired vertical row

                _gameObjects.Add(clone);
                Console.WriteLine($"Added Bubble at X:{posX}, Y:{posY}, Row:{row}, Col:{col}, Name:{clone.Name}");
            }
        }
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

    private void AddNewRow()
    {
        Texture2D BubblePuzzleTexture = Content.Load<Texture2D>("SpriteSheet");
        Rectangle[] bubbleColors = new Rectangle[]
        {
        new Rectangle(21, 21, 70, 70),   // Yellow
        new Rectangle(22, 132, 70, 70),  // Blue
        new Rectangle(20, 240, 70, 70),  // Red
        new Rectangle(20, 350, 70, 70),  // Brown
        new Rectangle(20, 460, 70, 70)   // Black
        };

        Console.WriteLine($"Adding New Row: {totalRows}");
        int columns = (totalRows % 2 == 0) ? 10 : 9;
        ++totalRows;
        for (int col = 0; col < columns; col++)
        {
            var bubble = new BubbleGrid(BubblePuzzleTexture)
            {
                Name = "BubbleGrid",
                Score = 30,
                Velocity = new Vector2(0, 0),
                Viewport = bubbleColors[Singleton.Instance.Random.Next(bubbleColors.Length)],
                Position = new Vector2(columns == 9 ? 595 + (70 * col): 560 + (70 * col), 100) // Position at the top (new row)
            };
            _gameObjects.Add(bubble);
        }
    }


    protected void DrawRectangleWithOutline(SpriteBatch spriteBatch, Texture2D pixel, Rectangle rect, Color outlineColor, int outlineThickness)
    {
        // Draw Outline
        spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, outlineThickness), outlineColor); // Up
        spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, outlineThickness, rect.Height), outlineColor); // Left
        spriteBatch.Draw(pixel, new Rectangle(rect.X + rect.Width - outlineThickness, rect.Y, outlineThickness, rect.Height), outlineColor); // Right
        spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y + rect.Height - outlineThickness, rect.Width, outlineThickness), outlineColor); // Down
    }
}