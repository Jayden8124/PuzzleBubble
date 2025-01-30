using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace PuzzleBubble;

public class MainScene : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    SpriteFont _font;
    Texture2D _rect;

    List<GameObject> _gameObjects;
    List<Bubble> _bubbles;

    private int _numObjects;

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
        // Singleton.Instance.CurrentGameState = Singleton.GameState.GameOver;

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
        _spriteBatch.Draw(_rect, new Vector2(515, 65), null, Color.Black, 0f, Vector2.Zero, new Vector2(Singleton.PlayWidth, Singleton.PlayHeight), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_rect, new Vector2(1360, 100), null, Color.Black, 0f, Vector2.Zero, new Vector2(Singleton.scoreboardHeight, Singleton.scoreboardWidth), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_rect, new Vector2(1360, 220), null, Color.Black, 0f, Vector2.Zero, new Vector2(Singleton.scoreboardHeight, Singleton.scoreboardWidth), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_rect, new Vector2(80, 90), null, Color.Black, 0f, Vector2.Zero, new Vector2(Singleton.barNameHeight, Singleton.barNameWidth), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_rect, new Vector2(70, 165), null, Color.Black, 0f, Vector2.Zero, new Vector2(Singleton.pictureBossHeight, Singleton.pictureBossWidth), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_rect, new Vector2(515, 980), null, Color.White, 0f, Vector2.Zero, new Vector2(Singleton.GunBaseWidth, Singleton.GunBaseHeight), SpriteEffects.None, 0f);

        _numObjects = _gameObjects.Count;

        for (int i = 0; i < _numObjects; i++)
        {
            _gameObjects[i].Draw(_spriteBatch);
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
        _spriteBatch.DrawString(_font, "Score: " + Singleton.Instance.Score.ToString(), new Vector2(1411, 137), Color.White);
        _spriteBatch.DrawString(_font, "TIME: " + String.Format("{0}:{1:00}", Singleton.Instance.Timer / 600000000, Singleton.Instance.Timer / 10000000 % 60), new Vector2(1411, 260), Color.White);


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


        Texture2D BubblePuzzleTexture = Content.Load<Texture2D>("SpriteSheet");

         // Reset the game objects
        _gameObjects.Clear();

        // Add Gun
        _gameObjects.Add(new Gun(BubblePuzzleTexture)
        {
            Name = "Gun",
            Viewport = new Rectangle(20, 798, 300, 150),
            Position = new Vector2((Singleton.PlayWidth / 2) + 515, Singleton.PlayHeight + 130),
            Left = Keys.Left,
            Right = Keys.Right,
            Fire = Keys.Space,
            Bubble = new Bubble(BubblePuzzleTexture) // Bubble has created when Gun is fired
            {
                Name = "BubblePlayer",
                Viewport = new Rectangle(22, 132, 70, 70),
                Velocity = new Vector2(0, -60f)
            }
        });

        ResetBubble();

        // Reset All Objects
        foreach (GameObject s in _gameObjects)
        {
            s.Reset();
        }

        Singleton.Instance.Score = 0;
        Singleton.Instance.Timer = 0;
    }
    private void RandomBubble()
    {
        _bubbles = new List<Bubble>();
        int numberOfBubbles = Singleton.Instance.Random.Next(5, 20); // Generate a Random number of bubbles between 5 and 20

        for (int i = 0; i < numberOfBubbles; i++)
        {
            int x = Singleton.Instance.Random.Next(0, 800); // Assuming the game width is 800
            int y = Singleton.Instance.Random.Next(0, 600); // Assuming the game height is 600
            Bubble bubble = new Bubble(Content.Load<Texture2D>("Bubble"))
            {
                Position = new Vector2(x, y),
                Velocity = new Vector2(0, 0)
            };
            _bubbles.Add(bubble);
        }
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

        // Initialize the Bubble Grid
        for (int row = 0; row < 5; row++)
        {
            int columns = (row % 2 == 0) ? 10 : 9; // Switch Columns 10 - 9
            for (int col = 0; col < columns; col++)
            {
                var clone = bg.Clone() as BubbleGrid;
                clone.Position = new Vector2(560 + (70 * col) + (35 * (row % 2)),60 * row); // Between Bubble
                _gameObjects.Add(clone);
            }
        }
    }

    // public void ResetEnemies()
    // {
    //     Texture2D spaceInvaderTexture = this.Content.Load<Texture2D>("SpaceInvaderSheet");

    //     Singleton.Instance.InvaderLeft = 55;

    //     Invader newInvader30 = new Invader(spaceInvaderTexture)
    //     {
    //         Name = "Enemy",
    //         Viewport = new Rectangle(78, 0, 30, 30),
    //         Score = 30,
    //         Bullet = new Bullet(spaceInvaderTexture)
    //         {
    //             Name = "BulletEnemy",
    //             Viewport = new Rectangle(231, 36, 9, 21),
    //             Velocity = new Vector2(0, 600f)
    //         }
    //     };
    //     Invader newInvader20 = new Invader(spaceInvaderTexture)
    //     {
    //         Name = "Enemy",
    //         Viewport = new Rectangle(0, 0, 39, 30),
    //         Score = 20,
    //         Bullet = new Bullet(spaceInvaderTexture)
    //         {
    //             Name = "BulletEnemy",
    //             Viewport = new Rectangle(231, 36, 9, 21),
    //             Velocity = new Vector2(0, 600f)
    //         }
    //     };
    //     Invader newInvader10 = new Invader(spaceInvaderTexture)
    //     {
    //         Name = "Enemy",
    //         Viewport = new Rectangle(138, 0, 42, 30),
    //         Score = 10,
    //         Bullet = new Bullet(spaceInvaderTexture)
    //         {
    //             Name = "BulletEnemy",
    //             Viewport = new Rectangle(231, 36, 9, 21),
    //             Velocity = new Vector2(0, 600f)
    //         }
    //     };


    //     for (int i = 0; i < 11; i++)
    //     {
    //         var clone = newInvader30.Clone() as Invader;
    //         clone.Position = new Vector2(Singleton.INVADERHORDEWIDTH / 11 * i +
    //          (Singleton.INVADERHORDEWIDTH / 11 - newInvader30.Rectangle.Width) / 2, 130);
    //         _gameObjects.Add(clone);
    //     }
    //     for (int i = 0; i < 11; i++)
    //     {
    //         var clone = newInvader20.Clone() as Invader;
    //         clone.Position = new Vector2(Singleton.INVADERHORDEWIDTH / 11 * i +
    //          (Singleton.INVADERHORDEWIDTH / 11 - newInvader20.Rectangle.Width) / 2, 160);
    //         _gameObjects.Add(clone);
    //     }
    //     for (int i = 0; i < 11; i++)
    //     {
    //         var clone = newInvader20.Clone() as Invader;
    //         clone.Position = new Vector2(Singleton.INVADERHORDEWIDTH / 11 * i +
    //          (Singleton.INVADERHORDEWIDTH / 11 - newInvader20.Rectangle.Width) / 2, 190);
    //         _gameObjects.Add(clone);
    //     }
    //     for (int i = 0; i < 11; i++)
    //     {
    //         var clone = newInvader10.Clone() as Invader;
    //         clone.Position = new Vector2(Singleton.INVADERHORDEWIDTH / 11 * i +
    //          (Singleton.INVADERHORDEWIDTH / 11 - newInvader10.Rectangle.Width) / 2, 220);
    //         _gameObjects.Add(clone);
    //     }
    //     for (int i = 0; i < 11; i++)
    //     {
    //         var clone = newInvader10.Clone() as Invader;
    //         clone.Position = new Vector2(Singleton.INVADERHORDEWIDTH / 11 * i +
    //          (Singleton.INVADERHORDEWIDTH / 11 - newInvader10.Rectangle.Width) / 2, 250);
    //         _gameObjects.Add(clone);
    //     }
    // }
}
