using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using PuzzleBubble;
using System.Threading;

namespace PuzzleBubble;

public class MainScene : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    SpriteFont _font;

    // Size Play Zone
    public const int _PlayWidth = 780;
    public const int _PlayHeight = 915;
    public const int _scoreboardWidth = 100;
    public const int _scoreboardHeight = 300;
    public const int _barNameWidth = 39;
    public const int _barNameHeight = 338;
    public const int _pictureBossWidth = 279;
    public const int _pictureBossHeight = 367;

    Texture2D _rect;

    List<GameObject> _gameObjects;
    int _numObjects;
    
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
        _rect = new Texture2D(GraphicsDevice, 1, 1);
        _rect.SetData(new Color[] { Color.White });

        Reset();
    }

    protected override void Update(GameTime gameTime)
    {
        Singleton.Instance.CurrentKey = Keyboard.GetState();

        //update
        _numObjects = _gameObjects.Count;

        switch (Singleton.Instance.CurrentGameState)
        {
            case Singleton.GameState.StartNewLife:
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
                    Singleton.Instance.CurrentGameState = Singleton.GameState.StartNewLife;
                }
                break;
        }


        Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;

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



        _numObjects = _gameObjects.Count;

        for (int i = 0; i < _numObjects; i++)
        {
            _gameObjects[i].Draw(_spriteBatch);
        }
        MouseState mouseState = Mouse.GetState();
        int mouseX = mouseState.X;
        int mouseY = mouseState.Y;

        // Draw mouse coordinates
        string coordinates = $"X: {mouseState.X}, Y: {mouseState.Y}";
        _spriteBatch.DrawString(_font, coordinates, new Vector2(10, 10), Color.Red);

        // font
        Vector2 fontSize = _font.MeasureString("Score: " + Singleton.Instance.Score.ToString());
        _spriteBatch.DrawString(_font, "Score: " + Singleton.Instance.Score.ToString(), new Vector2(1411, 137), Color.White);

        _spriteBatch.DrawString(_font, "TIME: " + String.Format("{0}:{1:00}", Singleton.Instance.Timer / 600000000, (Singleton.Instance.Timer / 10000000) % 60),
            new Vector2(1411, 260), Color.White);

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
        Singleton.Instance.CurrentGameState = Singleton.GameState.StartNewLife;

        Singleton.Instance.Random = new System.Random();

        Texture2D BubblePuzzleTexture = Content.Load<Texture2D>("SpriteSheet");

        _gameObjects.Clear();
        _gameObjects.Add(new Gun(BubblePuzzleTexture)
        {
            Name = "Gun",
            Viewport = new Rectangle(20, 798, 300, 150),
            Position = new Vector2(Singleton.SCREENWIDTH / 2, Singleton.SCREENHEIGHT - 10),
            Left = Keys.Left,
            Right = Keys.Right,
            Fire = Keys.Space,
            Bubble = new Bubble(BubblePuzzleTexture)
            {
                Name = "BubblePlayer",
                Viewport = new Rectangle(22, 132, 70, 70),
                Velocity = new Vector2(0, -60f)
            }
        });

        ResetBubble();

        foreach (GameObject s in _gameObjects)
        {
            s.Reset();
        }
        
        Singleton.Instance.Score = 0;
        Singleton.Instance.Timer = 0;
    }
    private void RandomBubble()
    {
        List<Bubble> bubbles = new List<Bubble>();

    }
    private void ResetBubble()
    {
        Texture2D BubblePuzzleTexture = Content.Load<Texture2D>("SpriteSheet");

        BubbleGrid bg = new BubbleGrid(BubblePuzzleTexture)
        {
            Name = "Bubble",
            Score = 30,
            Viewport = new Rectangle(22, 132, 70, 70),
            Velocity = new Vector2(0, 0)
        };

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                var clone = bg.Clone() as BubbleGrid;
                clone.Position = new Vector2(Singleton.PlayAreaWidth / 11 * i + (Singleton.PlayAreaWidth / 11 - bg.Rectangle.Width) / 2, 130);
                _gameObjects.Add(clone);
            }
        }
    }
}
