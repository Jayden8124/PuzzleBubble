using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PuzzleBubble;

public class MainScene : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    SpriteFont _font;
    Texture2D _rect, _background;
    List<GameObject> _gameObjects;
    private int _numObjects;
    Texture2D lineTexture;
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

        _rect = new Texture2D(GraphicsDevice, 1, 1);
        _rect.SetData(new Color[] { Color.White });

        lineTexture = new Texture2D(GraphicsDevice, 1, 1);
        lineTexture.SetData(new Color[] { Color.Red });

        backgroundMusic = Content.Load<Song>("BGM");
        fireSound = Content.Load<SoundEffect>("FireShoot");
        MediaPlayer.Play(backgroundMusic);
        MediaPlayer.IsRepeating = true;

        Reset();
    }

    protected override void Update(GameTime gameTime)
    {
        Singleton.Instance.CurrentKey = Keyboard.GetState();

        _numObjects = _gameObjects.Count;

        switch (Singleton.Instance.CurrentGameState)
        {
            case Singleton.GameState.Start:
                Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                break;
            case Singleton.GameState.GamePlaying:
                Singleton.Instance.Timer += gameTime.ElapsedGameTime.Ticks;

                // for (int i = 0; i < _numObjects; i++)
                // {
                //     if (_gameObjects[i].IsActive)
                //         _gameObjects[i].Update(gameTime, _gameObjects);
                // }
                // for (int i = 0; i < _numObjects; i++)
                // {
                //     if (!_gameObjects[i].IsActive)
                //     {
                //         _gameObjects.RemoveAt(i);
                //         i--;
                //         _numObjects--;
                //     }
                // }

                // Update ทุกออบเจ็กต์ โดยวนลูปผ่านสำเนาของ _gameObjects
                foreach (var obj in _gameObjects.ToList())
                {
                    obj.Update(gameTime, _gameObjects);
                }

                // หลังจากอัปเดตแล้ว ลบออบเจ็กต์ที่ไม่ active โดยใช้สำเนาอีกครั้ง
                foreach (var obj in _gameObjects.ToList())
                {
                    if (!obj.IsActive)
                    {
                        _gameObjects.Remove(obj);
                    }
                }


                // if (Singleton.Instance.BubbleLeft <= 0)
                // {
                //     foreach (GameObject s in _gameObjects)
                //     {
                //         if (s is Bubble)
                //         {
                //             s.Reset();
                //         }
                //     }
                // }

                foreach (GameObject g in _gameObjects)
                {
                    if (g is BubbleGrid bubble && bubble.Position.Y >= 870)
                    {
                        Singleton.Instance.CurrentGameState = Singleton.GameState.GameOver;
                    }
                }

                break;
            case Singleton.GameState.GameOver:
                if (!Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey) && Singleton.Instance.CurrentKey.GetPressedKeys().Length > 0)
                {
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
        GraphicsDevice.Clear(Color.White);

        _spriteBatch.Begin();

        // Base Background
        _spriteBatch.Draw(_background, new Vector2(0, 0), new Rectangle(9, 2615, 1921, 1081), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_background, new Vector2(1428, 550), new Rectangle(14, 506, 439, 449), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_background, new Vector2(1400, 511), new Rectangle(11, 322, 425, 100), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_background, new Vector2(1400, 631), new Rectangle(9, 196, 425, 100), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_background, new Vector2(40, 550), new Rectangle(26, 977, 426, 520), Color.White, 0f, Vector2.Zero, new Vector2(0.9f, 0.9f), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_background, new Vector2(1708, 999), new Rectangle(159, 20, 50, 50), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_background, new Vector2(1770, 999), new Rectangle(89, 20, 50, 50), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_background, new Vector2(1832, 999), new Rectangle(18, 20, 50, 50), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_rect, new Vector2(519, 65), null, Color.LightGray, 0f, Vector2.Zero, new Vector2(Singleton.PlayWidth, Singleton.PlayHeight), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_rect, new Vector2(519, 980), null, Color.BurlyWood, 0f, Vector2.Zero, new Vector2(Singleton.PlayWidth, 100), SpriteEffects.None, 0f);
        DrawRectangleWithOutline(_spriteBatch, _rect, new Rectangle(519, 60, Singleton.PlayWidth, Singleton.PlayHeight), Color.DimGray, 40);
        DrawLine(new Vector2(559, 870), new Vector2(1259, 870));

        // Get Mouse Position
        MouseState mouseState = Mouse.GetState();
        int mouseX = mouseState.X;
        int mouseY = mouseState.Y;
        string coordinates = $"X: {mouseState.X}, Y: {mouseState.Y}";
        _spriteBatch.DrawString(_font, coordinates, new Vector2(10, 10), Color.Red);

        // Draw Score and Time
        Vector2 fontSize = _font.MeasureString("Score: " + Singleton.Instance.Score.ToString());
        _spriteBatch.DrawString(_font, "Score: " + Singleton.Instance.Score.ToString(), new Vector2(1460, 545), Color.White);
        _spriteBatch.DrawString(_font, "TIME: " + String.Format("{0}:{1:00}", Singleton.Instance.Timer / 600000000, Singleton.Instance.Timer / 10000000 % 60), new Vector2(1460, 665), Color.White);

        _numObjects = _gameObjects.Count;

        for (int i = 0; i < _numObjects; i++)
        {
            _gameObjects[i].Draw(_spriteBatch);
        }

        // Draw each game state
        switch (Singleton.Instance.CurrentGameState)
        {
            case Singleton.GameState.GamePlaying:
                {
                    // { // Add new row of bubblesbubbles
                    Singleton.Instance.TimeDown += gameTime.ElapsedGameTime.Ticks;

                    // if (Singleton.Instance.TimeDown >= TimeSpan.TicksPerSecond * 10)
                    // {
                    //     Singleton.Instance.TimeDown = 0;
                    //     // MoveBubblesDown();
                    //     ResetBubble();
                    // }
                    // }
                }
                break;
            case Singleton.GameState.GameOver:
                fontSize = _font.MeasureString("Game Over");
                _spriteBatch.DrawString(_font, "Game Over", new Vector2((965 - fontSize.X) - 2, (555 - fontSize.Y) - 2), Color.White);
                _spriteBatch.DrawString(_font, "Game Over", new Vector2((965 - fontSize.X) + 2, (555 - fontSize.Y) - 2), Color.White);
                _spriteBatch.DrawString(_font, "Game Over", new Vector2((965 - fontSize.X) + 2, (555 - fontSize.Y) + 2), Color.White);
                _spriteBatch.DrawString(_font, "Game Over", new Vector2((965 - fontSize.X) - 2, (555 - fontSize.Y) + 2), Color.White);
                _spriteBatch.DrawString(_font, "Game Over", new Vector2((965 - fontSize.X), (555 - fontSize.Y)), Color.Red);
                break;
        }

        _spriteBatch.End();
        _graphics.BeginDraw();
        base.Draw(gameTime);
    }

    public void DrawLine(Vector2 start, Vector2 end)
    {
        float length = Vector2.Distance(start, end);
        float angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
        _spriteBatch.Draw(lineTexture, start, null, Color.Red, angle, Vector2.Zero, new Vector2(length, 1), SpriteEffects.None, 0f);
    }

    protected void Reset()
    {
        Singleton.Instance.CurrentGameState = Singleton.GameState.Start;
        Singleton.Instance.Score = 0;
        Singleton.Instance.Random = new System.Random();
        Singleton.Instance.totalRows = 5;

        Texture2D BubblePuzzleTexture = Content.Load<Texture2D>("Sprite2");

        _gameObjects.Clear();

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

        foreach (GameObject s in _gameObjects)
        {
            s.Reset();
        }
        Singleton.Instance.Score = 0;
        Singleton.Instance.Timer = 0;
    }

    private void ResetBubble()
    {
        Texture2D BubblePuzzleTexture = Content.Load<Texture2D>("SpriteSheet");
        Rectangle[] bubbleColors = new Rectangle[]
        {
            new Rectangle(21, 21, 70, 70),
            new Rectangle(22, 132, 70, 70),
            // new Rectangle(20, 240, 70, 70),
            // new Rectangle(20, 350, 70, 70),
            // new Rectangle(20, 460, 70, 70)
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
                int columns = (row % 2 == 0) ? 10 : 9;
                for (int col = 0; col < columns; col++)
                {
                    int colorIndex = Singleton.Instance.Random.Next(bubbleColors.Length);
                    var clone = new BubbleGrid(BubblePuzzleTexture)
                    {
                        Name = "BubbleGrid" + bubbleColorNames[colorIndex],
                        Viewport = bubbleColors[colorIndex],
                        Row = row,
                        Col = col
                    };
                    float posX = 560 + (70 * col) + (35 * (row % 2));
                    float posY = 100 + (70 * row);
                    clone.Position = new Vector2(posX, posY);
                    _gameObjects.Add(clone);
                }
            }
        }
        else if (Singleton.Instance.totalRows >= 5)
        {
            int columns = (Singleton.Instance.totalRows % 2 == 0) ? 9 : 10;
            for (int col = 0; col < columns; col++)
            {
                int colorIndex = Singleton.Instance.Random.Next(bubbleColors.Length);
                var clone = new BubbleGrid(BubblePuzzleTexture)
                {
                    Name = "BubbleGrid" + bubbleColorNames[colorIndex],
                    Viewport = bubbleColors[colorIndex],
                    Row = Singleton.Instance.totalRows,
                    Col = col
                };
                float posX = columns == 9 ? 595 + (70 * col) : 560 + (70 * col);
                float posY = 100;
                clone.Position = new Vector2(posX, posY);
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
                bubble.Row++;
            }
        }
    }

    protected void DrawEx()
    {
        _spriteBatch.Draw(_background, new Vector2(0, 0), new Rectangle(9, 2615, 1921, 1081), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_background, new Vector2(1428, 550), new Rectangle(14, 506, 439, 449), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_background, new Vector2(1400, 511), new Rectangle(11, 322, 425, 100), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_background, new Vector2(1400, 631), new Rectangle(9, 196, 425, 100), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_background, new Vector2(40, 550), new Rectangle(26, 977, 426, 520), Color.White, 0f, Vector2.Zero, new Vector2(0.9f, 0.9f), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_background, new Vector2(1708, 999), new Rectangle(159, 20, 50, 50), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_background, new Vector2(1770, 999), new Rectangle(89, 20, 50, 50), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_background, new Vector2(1832, 999), new Rectangle(18, 20, 50, 50), Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_rect, new Vector2(519, 65), null, Color.LightGray, 0f, Vector2.Zero, new Vector2(Singleton.PlayWidth, Singleton.PlayHeight), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_rect, new Vector2(519, 980), null, Color.BurlyWood, 0f, Vector2.Zero, new Vector2(Singleton.PlayWidth, 100), SpriteEffects.None, 0f);
        DrawRectangleWithOutline(_spriteBatch, _rect, new Rectangle(519, 60, Singleton.PlayWidth, Singleton.PlayHeight), Color.DimGray, 40);
        DrawLine(new Vector2(559, 870), new Vector2(1259, 870));
    }

    protected void DrawRectangleWithOutline(SpriteBatch spriteBatch, Texture2D pixel, Rectangle rect, Color outlineColor, int outlineThickness)
    {
        spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, outlineThickness), outlineColor);
        spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y + 5, outlineThickness, rect.Height), outlineColor);
        spriteBatch.Draw(pixel, new Rectangle(rect.X + rect.Width - outlineThickness, rect.Y + 5, outlineThickness, rect.Height), outlineColor);
    }
}
