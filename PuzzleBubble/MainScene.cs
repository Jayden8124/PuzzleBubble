using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;


namespace PuzzleBubble
{
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
        public SoundEffect GameOverVFX;
        Keys increaseVolumeKey = Keys.Up;
        Keys decreaseVolumeKey = Keys.Down;
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
            GameOverVFX = Content.Load<SoundEffect>("GameOver");
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;
            Reset();
        }

        protected override void Update(GameTime gameTime)
        {
            Singleton.Instance.CurrentKey = Keyboard.GetState();
            _numObjects = _gameObjects.Count;

            KeyboardState currentKeyState = Keyboard.GetState();

            if (currentKeyState.IsKeyDown(Keys.P))
            {
                MediaPlayer.Pause();
            }
            if (currentKeyState.IsKeyDown(Keys.R))
            {
                MediaPlayer.Resume();
            }

            if (currentKeyState.IsKeyDown(increaseVolumeKey))
            {
            // Increase the volume, ensuring it does not exceed 1.0
                MediaPlayer.Volume = Math.Min(MediaPlayer.Volume + 0.01f, 1.0f);
            }


            if (currentKeyState.IsKeyDown(decreaseVolumeKey))
            {

                MediaPlayer.Volume = Math.Max(MediaPlayer.Volume - 0.01f, 0.0f);
            }   


            switch (Singleton.Instance.CurrentGameState)
            {
                case Singleton.GameState.Start:
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                    MediaPlayer.Play(backgroundMusic);
                    MediaPlayer.IsRepeating = true;
                    break;
                case Singleton.GameState.GamePlaying:
                    Singleton.Instance.Timer += gameTime.ElapsedGameTime.Ticks;
                    // Update every object.
                    foreach (var obj in _gameObjects.ToList())
                    {
                        obj.Update(gameTime, _gameObjects);
                    }
                    // Remove any inactive objects.
                    foreach (var obj in _gameObjects.ToList())
                    {
                        if (!obj.IsActive)
                            _gameObjects.Remove(obj);
                    }

                    if (Singleton.Instance.BubbleLeft <= 0)
                    {
                        Singleton.Instance.CurrentGameState = Singleton.GameState.Winstage;
                    }

                     if (Singleton.Instance.Score >= 1000)
                    {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.Winstage;
                    }
                    foreach (GameObject g in _gameObjects)
                    {
                    if (g is BubbleGrid bubble && bubble.Position.Y >= 870)
                        {
                        MediaPlayer.Stop();
                        GameOverVFX.Play();
                        Singleton.Instance.CurrentGameState = Singleton.GameState.GameOver;
                        }
                    }

                    break;
                    
                case Singleton.GameState.GameOver:
                    if (!Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey) &&
                        Singleton.Instance.CurrentKey.GetPressedKeys().Length > 0)
                    {
                        Reset();
                        Singleton.Instance.CurrentGameState = Singleton.GameState.Start;
                    }
                    break;
                    case Singleton.GameState.Winstage:
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

            // Draw background elements.
            _spriteBatch.Draw(_background, new Vector2(0, 0), new Rectangle(9, 2615, 1921, 1081), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(_background, new Vector2(1428, 550), new Rectangle(14, 506, 439, 449), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(_background, new Vector2(1400, 391), new Rectangle(11, 322, 425, 100), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(_background, new Vector2(1400, 511), new Rectangle(11, 322, 425, 100), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(_background, new Vector2(1400, 631), new Rectangle(9, 196, 425, 100), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(_background, new Vector2(40, 550), new Rectangle(26, 977, 426, 520), Color.White, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(_background, new Vector2(1708, 999), new Rectangle(159, 20, 50, 50), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(_background, new Vector2(1770, 999), new Rectangle(89, 20, 50, 50), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(_background, new Vector2(1832, 999), new Rectangle(18, 20, 50, 50), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(_rect, new Vector2(519, 65), null, Color.LightGray, 0f, Vector2.Zero, new Vector2(Singleton.PlayWidth, Singleton.PlayHeight), SpriteEffects.None, 0f);
            _spriteBatch.Draw(_rect, new Vector2(519, 980), null, Color.BurlyWood, 0f, Vector2.Zero, new Vector2(Singleton.PlayWidth, 100), SpriteEffects.None, 0f);
            DrawRectangleWithOutline(_spriteBatch, _rect, new Rectangle(519, 60, Singleton.PlayWidth, Singleton.PlayHeight), Color.DimGray, 40);
            DrawLine(new Vector2(559, 870), new Vector2(1259, 870));

            // Draw mouse coordinates.
            MouseState mouseState = Mouse.GetState();
            string coordinates = $"X: {mouseState.X}, Y: {mouseState.Y}";
            _spriteBatch.DrawString(_font, coordinates, new Vector2(10, 10), Color.Red);

            // Draw score and time.
            _spriteBatch.DrawString(_font,"Bubble Left: " + Singleton.Instance.BubbleLeft.ToString(), new Vector2(1460, 425), Color.White);
            _spriteBatch.DrawString(_font, "Score: " + Singleton.Instance.Score.ToString(), new Vector2(1460, 545), Color.White);
            _spriteBatch.DrawString(_font, "TIME: " +
                String.Format("{0}:{1:00}",
                Singleton.Instance.Timer / 600000000,
                Singleton.Instance.Timer / 10000000 % 60), new Vector2(1460, 665), Color.White);

            // Draw all game objects.
            foreach (var obj in _gameObjects)
                obj.Draw(_spriteBatch);

            // Every 10 seconds, move all bubbles down and insert a new row at the top.
            if (Singleton.Instance.TimeDown >= TimeSpan.TicksPerSecond * 30)
            {
                Singleton.Instance.TimeDown = 0;
                InsertNewRowAtTop();
            }
            else
            {
                Singleton.Instance.TimeDown += gameTime.ElapsedGameTime.Ticks;
            }

            // Game Over overlay.
            if (Singleton.Instance.CurrentGameState == Singleton.GameState.GameOver)
            {
                Vector2 fontSize = _font.MeasureString("Game Over");
                _spriteBatch.DrawString(_font, "Game Over", new Vector2((965 - fontSize.X) - 2, (555 - fontSize.Y) - 2), Color.White);
                _spriteBatch.DrawString(_font, "Game Over", new Vector2((965 - fontSize.X) + 2, (555 - fontSize.Y) - 2), Color.White);
                _spriteBatch.DrawString(_font, "Game Over", new Vector2((965 - fontSize.X) + 2, (555 - fontSize.Y) + 2), Color.White);
                _spriteBatch.DrawString(_font, "Game Over", new Vector2((965 - fontSize.X) - 2, (555 - fontSize.Y) + 2), Color.White);
                _spriteBatch.DrawString(_font, "Game Over", new Vector2((965 - fontSize.X), (555 - fontSize.Y)), Color.Red);
                
            }
            if (Singleton.Instance.CurrentGameState == Singleton.GameState.Winstage)
            {
                Vector2 fontSize = _font.MeasureString("You Win!");
                _spriteBatch.DrawString(_font, "You Win!", new Vector2((965 - fontSize.X) - 2, (555 - fontSize.Y) - 2), Color.White);
                _spriteBatch.DrawString(_font, "You Win!", new Vector2((965 - fontSize.X) + 2, (555 - fontSize.Y) - 2), Color.White);
                _spriteBatch.DrawString(_font, "You Win!", new Vector2((965 - fontSize.X), (555 - fontSize.Y)), Color.Green);
            }

            _spriteBatch.End();
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
            Singleton.Instance.Random = new Random();
            // Start with an initial total row count (for example, 5).
            Singleton.Instance.totalRows = 5;

            Texture2D BubblePuzzleTexture = Content.Load<Texture2D>("Sprite2");
            _gameObjects.Clear();

            // Create the gun.
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

            // Create the initial grid.
            CreateInitialGrid();
            foreach (GameObject s in _gameObjects)
                s.Reset();

            Singleton.Instance.Score = 0;
            Singleton.Instance.Timer = 0;
            Singleton.Instance.TimeDown = 0;
        }

        /// <summary>
        /// Creates the initial grid of bubbles.
        /// </summary>
        private void CreateInitialGrid()
        {
            Singleton.Instance.BubbleLeft = 50;
            Texture2D BubblePuzzleTexture = Content.Load<Texture2D>("SpriteSheet");
            Rectangle[] bubbleColors = new Rectangle[]
            {
                new Rectangle(21, 21, 70, 70),
                new Rectangle(22, 132, 70, 70),
                new Rectangle(20, 240, 70, 70),
                new Rectangle(20, 350, 70, 70),
                new Rectangle(20, 460, 70, 70)
            };
            string[] bubbleColorNames = new string[]
            {
                "Yellow",
                "Blue",
                "Red",
                "Brown",
                "Black"
            };

            // For initial grid, create rows 0 to totalRows-1.
            for (int row = 0; row < Singleton.Instance.totalRows; row++)
            {
                int columns = (row % 2 == 0) ? 10 : 9;
                for (int col = 0; col < columns; col++)
                {
                    int colorIndex = Singleton.Instance.Random.Next(bubbleColors.Length);
                    BubbleGrid clone = new BubbleGrid(BubblePuzzleTexture)
                    {
                        Name = "BubbleGrid" + bubbleColorNames[colorIndex],
                        Viewport = bubbleColors[colorIndex],
                        Row = row,
                        Col = col,
                        Position = new Vector2(560 + (70 * col) + (35 * (row % 2)), 100 + (70 * row))
                    };
                    _gameObjects.Add(clone);
                }
            }
            Singleton.Instance.totalRows++;
        }

        /// <summary>
        /// Moves all grid bubbles down by one row and then inserts a new row at the top.
        /// The new row’s number of columns alternates (for example, if the current total row count is odd, use 9; if even, use 10).
        /// </summary>
        private void InsertNewRowAtTop()
        {
            // First, shift every BubbleGrid one row down.
            foreach (var obj in _gameObjects)
            {
                if (obj is BubbleGrid bubble)
                {
                    bubble.Row += 1;
                    // Recalculate X position based on the new row’s offset.
                    // int offset = (bubble.Row % 2 == 1) ? 35 : 0;
                    // bubble.Position = new Vector2(560 + (70 * bubble.Col) + offset, 100 + (70 * bubble.Row));
                    bubble.Position = new Vector2(bubble.Position.X, bubble.Position.Y + 70);

                }
            }

            // Decide number of columns for the new top row.
            // (For example, if totalRows is odd, new row gets 9 bubbles; if even, 10 bubbles.)
            int columns = (Singleton.Instance.totalRows % 2 == 0) ? 9 : 10;
            Texture2D bubbleTexture = Content.Load<Texture2D>("SpriteSheet");
            Rectangle[] bubbleColors = new Rectangle[]
            {
                new Rectangle(21, 21, 70, 70),
                new Rectangle(22, 132, 70, 70),
                new Rectangle(20, 240, 70, 70),
                new Rectangle(20, 350, 70, 70),
                new Rectangle(20, 460, 70, 70)
            };
            string[] bubbleColorNames = new string[]
            {
                "Yellow",
                "Blue",
                "Red",
                "Brown",
                "Black"
            };

            int newRow = 0;
            for (int col = 0; col < columns; col++)
            {
                int colorIndex = Singleton.Instance.Random.Next(bubbleColors.Length);
                BubbleGrid newBubble = new BubbleGrid(bubbleTexture)
                {
                    Name = "BubbleGrid" + bubbleColorNames[colorIndex],
                    Viewport = bubbleColors[colorIndex],
                    Row = newRow,
                    Col = col,
                    Position = new Vector2(Singleton.Instance.totalRows % 2 == 0 ? 595 + (70 * col) : 560 + (70 * col), 100 + (70 * newRow)),
                    IsActive = true
                };
                _gameObjects.Add(newBubble);
            }
            Singleton.Instance.totalRows++;
        }

        protected void DrawRectangleWithOutline(SpriteBatch spriteBatch, Texture2D pixel, Rectangle rect, Color outlineColor, int outlineThickness)
        {
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, outlineThickness), outlineColor);
            spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y + 5, outlineThickness, rect.Height), outlineColor);
            spriteBatch.Draw(pixel, new Rectangle(rect.X + rect.Width - outlineThickness, rect.Y + 5, outlineThickness, rect.Height), outlineColor);
        }
    }
}
