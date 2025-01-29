using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using PuzzleBubble;

namespace PuzzleBubble;

public class MainScene : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    SpriteFont _font;

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


        _numObjects = _gameObjects.Count;

        for (int i = 0; i < _numObjects; i++)
        {
            _gameObjects[i].Draw(_spriteBatch);
        }

        Vector2 fontSize = _font.MeasureString("Score: " + Singleton.Instance.Score.ToString());
        _spriteBatch.DrawString(_font, "Score: " + Singleton.Instance.Score.ToString(), new Vector2((Singleton.SCREENWIDTH / 2 - fontSize.X) / 2, 30), Color.White);

        fontSize = _font.MeasureString("Life: " + Singleton.Instance.Life.ToString());
        _spriteBatch.DrawString(_font, "Life: " + Singleton.Instance.Life.ToString(), new Vector2((Singleton.SCREENWIDTH / 2 - fontSize.X) / 2 + Singleton.SCREENWIDTH / 2, 30), Color.White);

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
            Position = new Vector2(Singleton.SCREENWIDTH / 2, 500),
            Left = Keys.Left,
            Right = Keys.Right,
            Fire = Keys.Space,
            Bubble = new Bubble(BubblePuzzleTexture)
            {
                Name = "BubblePlayer",
                Viewport = new Rectangle(22, 132, 70, 70),
                Velocity = new Vector2(0, -600f)
            }
        });

        // ResetBubble();

        foreach (GameObject s in _gameObjects)
        {
            s.Reset();
        }
    }

    private void ResetBubble()
    {
        Texture2D BubblePuzzleTexture = Content.Load<Texture2D>("SpriteSheet");

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                _gameObjects.Add(new Bubble(BubblePuzzleTexture)
                {
                    Name = "Bubble",
                    Viewport = new Rectangle(0, 0, 50, 50),
                    Position = new Vector2(50 + i * 50, 50 + j * 50),
                    Velocity = new Vector2(0, 0)
                });
            }
        }
    }
}
