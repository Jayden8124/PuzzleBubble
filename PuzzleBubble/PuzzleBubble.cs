using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
// using PuzzleBubble.GameObjects;
using System.Collections.Generic;
using MonoGame.Extended;

namespace PuzzleBubble;

public class PuzzleBubble : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // 
    // private List<Bubble> _bubbles; // เก็บฟองอากาศทั้งหมด
    private List<Texture2D> _bubbleTextures; // เก็บ Texture สำหรับสุ่ม

    Texture2D _gun; // Object
    Texture2D _b1, _b2, _b3, _br1, _br2, _br3, _r1, _r2, _r3, _y1, _y2, _y3; // Color Bubble
    Texture2D _h1, _h2; // Player
    Texture2D _rect; // Play Zone

    // Size Play Zone
    public const int _PlayWidth = 780;
    public const int _PlayHeight = 915;
    public const int _scoreboardWidth = 100;
    public const int _scoreboardHeight = 300;
    public const int _barNameWidth = 39;
    public const int _barNameHeight = 338;
    public const int _pictureBossWidth = 279;
    public const int _pictureBossHeight = 367;

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

        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent() // Auto Load When Initialize Run
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _gun = Content.Load<Texture2D>("gun");  // Gun Texture

        // Bubble Texture
        {
            _b1 = Content.Load<Texture2D>("b1");
            _b2 = Content.Load<Texture2D>("b2");
            _b3 = Content.Load<Texture2D>("b3");
            _br1 = Content.Load<Texture2D>("br1");
            _br2 = Content.Load<Texture2D>("br2");
            _br3 = Content.Load<Texture2D>("br3");
            _r1 = Content.Load<Texture2D>("r1");
            _r2 = Content.Load<Texture2D>("r2");
            _r3 = Content.Load<Texture2D>("r3");
            _y1 = Content.Load<Texture2D>("y1");
            _y2 = Content.Load<Texture2D>("y2");
            _y3 = Content.Load<Texture2D>("y3");
        }

        // Player Texture
        {
            _h1 = Content.Load<Texture2D>("h1");
            _h2 = Content.Load<Texture2D>("h2");
        }

        // Rectangle Texture
        {
            _rect = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            Color[] data = new Color[1];
            data[0] = Color.Black;
            _rect.SetData(data);
        }
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        MouseState state = Mouse.GetState();

        // switch (_currentGameState)
        // {
        //     case GameState.Start:
        //         if (state.LeftButton == ButtonState.Pressed)
        //         {
        //             _currentGameState = GameState.Playing;
        //         }
        //         break;

        //     case GameState.Playing:
        //         // Handle Playing state logic
        //         // TODO: Add your game update logic here
        //         break;

        //     case GameState.GameOver:
        //         if (state.LeftButton == ButtonState.Pressed)
        //         {
        //             _currentGameState = GameState.Start;
        //             _isGameEnded = true;
        //         }
        //         break;
        // }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        _spriteBatch.Begin();

        // Base Background
        _spriteBatch.Draw(_rect, new Vector2(515, 65), null, Color.Black, 0f, Vector2.Zero, new Vector2(_PlayWidth, _PlayHeight), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_rect, new Vector2(1360, 100), null, Color.Black, 0f, Vector2.Zero, new Vector2(_scoreboardHeight, _scoreboardWidth), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_rect, new Vector2(1360, 220), null, Color.Black, 0f, Vector2.Zero, new Vector2(_scoreboardHeight, _scoreboardWidth), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_rect, new Vector2(80, 90), null, Color.Black, 0f, Vector2.Zero, new Vector2(_barNameHeight, _barNameWidth), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_rect, new Vector2(70, 165), null, Color.Black, 0f, Vector2.Zero, new Vector2(_pictureBossHeight, _pictureBossWidth), SpriteEffects.None, 0f);
        _spriteBatch.DrawCircle(new Vector2(1449, 417), 50, 100, Color.Red, 50);
        

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
        //         // _spriteBatch.Draw(_gun, new Vector2(900, 950), Color.White);
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

    // public void InitializeBubble(){
    //     for (int row = 0; row < 5; row++)
    //     {
    //         int columns = (row % 2 == 0) ? 10 : 9; // Alternate between 10 and 9 columns
    //         for (int col = 0; col < columns; col++)
    //         {
    //             Bubble newBubble = new Bubble();
    //             newBubble.Texture = _br1; // Assuming _br1 is the texture for the bubble
    //             newBubble.Position = new Vector2(100 + col * 50, 50 + row * 50);
    //             _bubbles.Add(newBubble);
    //         }
    //     }
    // }

    public void CurrentDisplayMode(){  
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