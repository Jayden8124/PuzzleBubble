using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
// using GameObjects.Bubble;

namespace PuzzleBubble;

public class PuzzleBubble : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    Texture2D _bubble, _gun, _rect;

    enum GameState
    {
        Start,
        Playing,
        GameOver
    }

    GameState _currentGameState;

    public PuzzleBubble()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Size Screen
        // var displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
        // _graphics.PreferredBackBufferWidth = displayMode.Width;
        // _graphics.PreferredBackBufferHeight = displayMode.Height;
        // _graphics.IsFullScreen = true; // False for Windows Mode

        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;

        _graphics.ApplyChanges();

        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent() // Auto Load When Initialize Run
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // _bubble = Content.Load<Texture2D>("bubble"); // Bubble Texture
        // _gun = Content.Load<Texture2D>("gun");  // gun Texture

        _rect = new Texture2D(GraphicsDevice, 1200, 830);

        Color[] data = new Color[1200 * 830];
        for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
        _rect.SetData(data);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        MouseState state = Mouse.GetState();

            // if (state.LeftButton == ButtonState.Pressed && !_isGameEnded)
            // {
            //     int iPos = state.X / 200;
            //     int jPos = state.Y / 200;

            //     if (iPos >= 0 && iPos < 3 && jPos >= 0 && jPos < 3)
            //     {
            //         //check feasibility
            //         if (_gameTable[jPos, iPos] == 0)
            //         {
            //             if (_isCircleTurn)
            //             {
            //                 _gameTable[jPos, iPos] = 1;
            //             }
            //             else
            //             {
            //                 _gameTable[jPos, iPos] = -1;
            //             }

            //             //flip turn
            //             _isCircleTurn = !_isCircleTurn;
            //         }
            //     }
            // }    

        switch (_currentGameState)
        {
            case GameState.Start:
                // Handle Start state logic
                if (state.LeftButton == ButtonState.Pressed)
                {
                    _currentGameState = GameState.Playing;
                }
                break;

            case GameState.Playing:
                // Handle Playing state logic
                // TODO: Add your game update logic here
                break;

            case GameState.GameOver:
                // Handle GameOver state logic
                if (state.LeftButton == ButtonState.Pressed )
                {
                    _currentGameState = GameState.Start;
                }
                break;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DimGray);

        _spriteBatch.Begin();

        switch (_currentGameState)
        {
            case GameState.Start:
                // Draw Start state
                // _spriteBatch.DrawString(Content.Load<SpriteFont>("Font"), "Press Enter to Start", new Vector2(100, 100), Color.White);
                break;

            case GameState.Playing:
                // Draw Playing state
                // _spriteBatch.Draw(_rect, new Vector2(600, 600), Color.White);

                // Draw bubbles
                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 12; col++)
                    {
                        // _spriteBatch.Draw(_bubble, new Vector2(100 + col * 50, 50 + row * 50), Color.White);
                    }
                }

                // Draw gun
                // _spriteBatch.Draw(_gun, new Vector2(900, 950), Color.White);
                break;

            case GameState.GameOver:
                // Draw GameOver state
                // _spriteBatch.DrawString(Content.Load<SpriteFont>("Font"), "Game Over! Press Enter to Restart", new Vector2(100, 100), Color.White);
                break;
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    // public void InitializeBubble(){
    //     // Logic to initialize the game with bubbles
    //     // Example: Create a grid of bubbles
    //     for (int row = 0; row < 8; row++)
    //     {
    //         for (int col = 0; col < 12; col++)
    //         {
    //             Bubble newBubble = new Bubble();
    //             newBubble.Position = new Vector2(100 + col * 50, 50 + row * 50);
    //             _bubbles.Add(newBubble);
    //         }
    //     }
    // }

    // public void ShootBubble(){
    //     // Logic to shoot a bubble
    //     // Example: Create a new bubble and set its direction
    //     Bubble newBubble = new Bubble();
    //     newBubble.Position = new Vector2(900, 950); // Starting position of the gun
    //     newBubble.Direction = new Vector2(0, -1); // Shoot upwards
    //     _bubbles.Add(newBubble); // Add the new bubble to the list of bubbles
    // }

    // public void CheckCollision(){
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