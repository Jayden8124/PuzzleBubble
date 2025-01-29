using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PuzzleBubble.GameObjects;
using System.Collections.Generic;
using MonoGame.Extended;
using System;
using System;

namespace PuzzleBubble;

public class PuzzleBubble : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // 
    private List<Bubble> _bubbles; // เก็บฟองอากาศทั้งหมด
    private List<Texture2D> _bubbleTextures; // เก็บ Texture สำหรับสุ่ม

    Texture2D _gun; // Object
    Texture2D _b1, _b2, _b3, _br1, _br2, _br3, _r1, _r2, _r3, _y1, _y2, _y3; // Color Bubble
    Texture2D _h1, _h2; // Player
    Texture2D _rect; // Play Zone
    SpriteFont _font; // Font
    // Size Play Zone
    public const int _PlayWidth = 780;
    public const int _PlayHeight = 915;
    public const int _scoreboardWidth = 500;
    public const int _scoreboardHeight = 100;
    public const int _barNameWidth = 39;
    public const int _barNameHeight = 338;
    public const int _pictureBossWidth = 279;
    public const int _pictureBossHeight = 367;

    List<GameObject> _gameObjects;
    int _numObjects;

    // test
    int _score;
    long _timer;
    private Texture2D _lineTexture; // Texture for drawing lines

    // Status Game
    // bool _isGameEnded = false;

    // enum GameState
    // {
    //     Start,
    //     Playing,
    //     GameOver
    // }

    // GameState _currentGameState;

    public PuzzleBubble()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;

        _graphics.ApplyChanges();
        _gameObjects = new List<GameObject>();

        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent() // Auto Load When Initialize Run
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _gun = Content.Load<Texture2D>("gun");  // Gun Texture
        _font = Content.Load<SpriteFont>("GameFont"); // Font

        // Create a 1x1 pixel texture for drawing lines
        _lineTexture = new Texture2D(GraphicsDevice, 1, 1);
        _lineTexture.SetData(new[] { Color.White });
        Reset();


        // // Bubble Texture
        // {
        //     _b1 = Content.Load<Texture2D>("b1");
        //     _b2 = Content.Load<Texture2D>("b2");
        //     _b3 = Content.Load<Texture2D>("b3");
        //     _br1 = Content.Load<Texture2D>("br1");
        //     _br2 = Content.Load<Texture2D>("br2");
        //     _br3 = Content.Load<Texture2D>("br3");
        //     _r1 = Content.Load<Texture2D>("r1");
        //     _r2 = Content.Load<Texture2D>("r2");
        //     _r3 = Content.Load<Texture2D>("r3");
        //     _y1 = Content.Load<Texture2D>("y1");
        //     _y2 = Content.Load<Texture2D>("y2");
        //     _y3 = Content.Load<Texture2D>("y3");
        // }

        // // Player Texture
        // {
        //     _h1 = Content.Load<Texture2D>("h1");
        //     _h2 = Content.Load<Texture2D>("h2");
        // }

        // // Rectangle Texture
        // {
        //     _rect = new Texture2D(_graphics.GraphicsDevice, 1, 1);
        //     Color[] data = new Color[1];
        //     data[0] = Color.Black;
        //     _rect.SetData(data);
        // }

        // // โหลดไฟล์ภาพจาก Content Pipeline
        // _bubbleTextures = new List<Texture2D>
        // {
        //     Content.Load<Texture2D>("b1"),
        //     Content.Load<Texture2D>("b2"),
        //     Content.Load<Texture2D>("b3"),
        //     Content.Load<Texture2D>("br1"),
        //     Content.Load<Texture2D>("br2"),
        //     Content.Load<Texture2D>("br3"),
        //     Content.Load<Texture2D>("r1"),
        //     Content.Load<Texture2D>("r2"),
        //     Content.Load<Texture2D>("r3"),
        //     Content.Load<Texture2D>("y1"),
        //     Content.Load<Texture2D>("y2"),
        //     Content.Load<Texture2D>("y3"),
        // };

        // // สร้างฟองอากาศ
        // _bubbles = new List<Bubble>();
        // InitializeBubble();
    }

    protected override void Update(GameTime gameTime)
    {
        // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        //     Exit();

        MouseState state = Mouse.GetState();
        //update
        _numObjects = _gameObjects.Count;
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

        //     switch (_currentGameState)
        // {
        //     case GameState.Start:
        //         if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        //         {
        //             _currentGameState = GameState.Playing;
        //             InitializeBubble();
        //         }
        //         break;

        //     case GameState.Playing:
        //         // Handle Playing state logic
        //         // TODO: Add your game update logic here
        // time
        _timer += gameTime.ElapsedGameTime.Ticks;

        //         break;

        //     case GameState.GameOver:
        //         if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        //         {
        //             _currentGameState = GameState.Start;
        //         }
        //         break;
        // }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        _spriteBatch.Begin();
        _numObjects = _gameObjects.Count;

        for (int i = 0; i < _numObjects; i++)
        {
            _gameObjects[i].Draw(_spriteBatch);
        }

        // // Base Background
        // _spriteBatch.Draw(_rect, new Vector2(515, 65), null, Color.Black, 0f, Vector2.Zero, new Vector2(_PlayWidth, _PlayHeight), SpriteEffects.None, 0f);
        // _spriteBatch.Draw(_rect, new Vector2(1360, 100), null, Color.Black, 0f, Vector2.Zero, new Vector2(_scoreboardWidth, _scoreboardHeight), SpriteEffects.None, 0f);
        // _spriteBatch.Draw(_rect, new Vector2(1360, 220), null, Color.Black, 0f, Vector2.Zero, new Vector2(_scoreboardWidth, _scoreboardHeight), SpriteEffects.None, 0f);
        // _spriteBatch.Draw(_rect, new Vector2(80, 90), null, Color.Black, 0f, Vector2.Zero, new Vector2(_barNameHeight, _barNameWidth), SpriteEffects.None, 0f);
        // _spriteBatch.Draw(_rect, new Vector2(70, 165), null, Color.Black, 0f, Vector2.Zero, new Vector2(_pictureBossHeight, _pictureBossWidth), SpriteEffects.None, 0f);
        // _spriteBatch.DrawCircle(new Vector2(1449, 417), 50, 100, Color.Red, 50);

        // SCORE Drawing
        _spriteBatch.DrawString(_font, "SCORE: " + _score, new Vector2(1449, 266), Color.White);

        // TIMER Drawing
        _spriteBatch.DrawString(_font, "TIME: " + String.Format("{0}:{1:00}", _timer / 600000000, (_timer / 10000000) % 60),
            new Vector2(1449, 144), Color.White);

        // test mouse 
        // Get mouse state
        MouseState mouseState = Mouse.GetState();
        int mouseX = mouseState.X;
        int mouseY = mouseState.Y;

        // Draw horizontal and vertical lines
        _spriteBatch.Draw(_lineTexture, new Rectangle(0, mouseY, _graphics.PreferredBackBufferWidth, 1), Color.Red); // Horizontal line
        _spriteBatch.Draw(_lineTexture, new Rectangle(mouseX, 0, 1, _graphics.PreferredBackBufferHeight), Color.Red); // Vertical line

        // Draw mouse coordinates
        string coordinates = $"X: {mouseState.X}, Y: {mouseState.Y}";
        _spriteBatch.DrawString(_font, coordinates, new Vector2(10, 10), Color.Red);
        // switch (_currentGameState)
        // {
        //     case GameState.Start:
        //         // Draw Start state
        //         // _spriteBatch.DrawString(Content.Load<SpriteFont>("Font"), "Press Enter to Start", new Vector2(100, 100), Color.White);
        //         break;

        //     case GameState.Playing:
        //         // Draw Playing state
        //         // _spriteBatch.Draw(_rect, new Vector2(600, 600), Color.White);

        //         // Draw bubbles
        //         for (int row = 0; row < 8; row++)
        //         {
        //             for (int col = 0; col < 12; col++)
        //             {
        //                 // _spriteBatch.Draw(_bubble, new Vector2(100 + col * 50, 50 + row * 50), Color.White);
        //             }
        //         }

        //         // Draw gun
        // _spriteBatch.Draw(_gun, new Vector2(900, 950), Color.White);
        //         break;

        //     case GameState.GameOver:
        //         // Draw GameOver state
        //         // _spriteBatch.DrawString(Content.Load<SpriteFont>("Font"), "Game Over! Press Enter to Restart", new Vector2(100, 100), Color.White);
        //         break;
        // }

        _spriteBatch.End();
        _graphics.BeginDraw(); // Draw a buffer

        base.Draw(gameTime);
    }

    public void InitializeBubble()
    {
        Random random = new Random();

        for (int row = 0; row < 5; row++)
        {
            int columns = (row % 2 == 0) ? 10 : 9; // Switch Columns 10 - 9
            for (int col = 0; col < columns; col++)
            {
                Texture2D randomTexture = _bubbleTextures[random.Next(_bubbleTextures.Count)];
                Bubble newBubble = new Bubble(randomTexture)
                {
                    Position = new Vector2(65 + col * 70, 65 + row * 70) // Set Position
                };

                _bubbles.Add(newBubble); // add Bubble To List
            }
        }
    }

    protected void Reset()
    {
        // Singleton.Instance.Score = 0;
        // Singleton.Instance.Life = 3;

        // Singleton.Instance.CurrentGameState = Singleton.GameState.StartNewLife;

        // Singleton.Instance.Random = new System.Random();

        Texture2D spaceInvaderTexture = Content.Load<Texture2D>("Sprite");

        _gameObjects.Clear();
        _gameObjects.AddRange(new List<GameObject>
        {
            new bb(spaceInvaderTexture)
            {
                Name = "Player",
                Viewport = new Rectangle(22, 132, 70, 70),
                Position = new Vector2(50, 50),
                Left = Keys.Left,
                Right = Keys.Right,
                Fire = Keys.Space,
            },
            new bb(spaceInvaderTexture)
            {
                Name = "Player2",
                Viewport = new Rectangle(112, 136, 70, 70),
                Position = new Vector2(150, 50),
                Left = Keys.Left,
                Right = Keys.Right,
                Fire = Keys.Space,
            },
            new bb(spaceInvaderTexture)
            {
                Name = "Player3",
                Viewport = new Rectangle(204, 142, 70, 70),
                Position = new Vector2(250, 50),
                Left = Keys.Left,
                Right = Keys.Right,
                Fire = Keys.Space,
            },  
            new bb(spaceInvaderTexture)
            {
                Name = "Player",
                Viewport = new Rectangle(18, 350, 70, 70),
                Position = new Vector2(50, 150),
                Left = Keys.Left,
                Right = Keys.Right,
                Fire = Keys.Space,
            },
            new bb(spaceInvaderTexture)
            {
                Name = "Player2",
                Viewport = new Rectangle(110, 355, 70, 70),
                Position = new Vector2(150, 150),
                Left = Keys.Left,
                Right = Keys.Right,
                Fire = Keys.Space,
            },
            new bb(spaceInvaderTexture)
            {
                Name = "Player3",
                Viewport = new Rectangle(204, 359, 70, 70),
                Position = new Vector2(250, 150),
                Left = Keys.Left,
                Right = Keys.Right,
                Fire = Keys.Space,
            }, 
            new bb(spaceInvaderTexture)
            {
                Name = "Player",
                Viewport = new Rectangle(18, 241, 70, 70),
                Position = new Vector2(50, 350),
                Left = Keys.Left,
                Right = Keys.Right,
                Fire = Keys.Space,
            },
            new bb(spaceInvaderTexture)
            {
                Name = "Player2",
                Viewport = new Rectangle(110, 248, 70, 70),
                Position = new Vector2(450, 250),
                Left = Keys.Left,
                Right = Keys.Right,
                Fire = Keys.Space,
            },
            new bb(spaceInvaderTexture)
            {
                Name = "Player3",
                Viewport = new Rectangle(204, 248, 70, 70),
                Position = new Vector2(550, 250),
                Left = Keys.Left,
                Right = Keys.Right,
                Fire = Keys.Space,
            },
            new bb(spaceInvaderTexture)
            {
                Name = "gun",
                Viewport = new Rectangle(20, 798, 300, 150),
                Position = new Vector2(550, 550),
                Left = Keys.Left,
                Right = Keys.Right,
                Fire = Keys.Space,
            },
            new bb(spaceInvaderTexture)
            {
                Name = "gun2",
                Viewport = new Rectangle(20, 1478, 300, 150),
                Position = new Vector2(980, 750),
                Left = Keys.Left,
                Right = Keys.Right,
                Fire = Keys.Space,
            },
            new bb(spaceInvaderTexture)
            {
                Name = "gun3",
                Viewport = new Rectangle(20, 1140, 300, 150),
                Position = new Vector2(250, 800),
                Left = Keys.Left,
                Right = Keys.Right,
                Fire = Keys.Space,
            },       
            new bb(spaceInvaderTexture)
            {
                Name = "player",
                Viewport = new Rectangle(26, 573, 190, 180),
                Position = new Vector2(650, 850),
                Left = Keys.Left,
                Right = Keys.Right,
                Fire = Keys.Space,
            },       
        });

        foreach (GameObject s in _gameObjects)
        {
            s.Reset();
        }
    }



    public void CurrentDisplayMode()
    {
        var displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
        _graphics.PreferredBackBufferWidth = displayMode.Width;
        _graphics.PreferredBackBufferHeight = displayMode.Height;
        _graphics.IsFullScreen = true; // False for Windows Mode
    }

    // public void ShootBubble(){
    //     // Logic to shoot a bubble
    //     // Example: Create a new bubble and set its direction
    //     Bubble newBubble = new Bubble();
    //     newBubble.Position = new Vector2(900, 950); // Starting position of the gun
    //     newBubble.Direction = new Vector2(0, -1); // Shoot upwards
    //     _bubbles.Add(newBubble); // Add the new bubble to the list of bubbles
    // }

    // public void CheckCollision()
    // {
    //     // Logic to check for collisions between bubbles
    //     for (int i = 0; i < _bubbles.Count; i++)
    //     {
    //         for (int j = i + 1; j < _bubbles.Count; j++)
    //         {
    //             if (_bubbles[i].Bounds.Intersects(_bubbles[j].Bounds))
    //             {
    //                 // Handle collision
    //                 _bubbles[i].OnCollision(_bubbles[j]);
    //                 _bubbles[j].OnCollision(_bubbles[i]);
    //             }
    //         }
    //     }
    // }

    // public void UpdateGrid(){
    //     // Logic to update the grid with the current state of bubbles
    //     foreach (var bubble in _bubbles)
    //     {
    //         int gridX = (int)(bubble.Position.X / _gridCellSize);
    //         int gridY = (int)(bubble.Position.Y / _gridCellSize);
    //         _gameTable[gridX, gridY] = bubble.Color; // Assuming bubble has a Color property
    //     }
    // }
}