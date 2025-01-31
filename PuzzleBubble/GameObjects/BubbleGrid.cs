using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleBubble
{
    class BubbleGrid : Bubble
    {
        public int Score;
        public float Speed;
        public float DistanceMoved;

        public BubbleGrid(Texture2D texture) : base(texture)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Viewport, Color.White);
            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            Speed = 20;
            DistanceMoved = 0;
            base.Reset();
        }
        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            // if (gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond == 3)
            // {
            // Velocity.Y = Speed;
            // }

            DistanceMoved += Math.Abs(Velocity.X * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond);
            float newX = Position.X + Velocity.X * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
            float newY = Position.Y;
            if (DistanceMoved >= Singleton.SCREENWIDTH - Singleton.BubbleHeight)
            {
                DistanceMoved = 0;

                newY += 70;
                Speed *= 1.2f;

                if (newY >= 900)
                {
                    // Game Over
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GameOver;
                }
            }

            Position = new Vector2(newX, newY);

            base.Update(gameTime, gameObjects);
        }

        // public void MoveDown()
        // {
        //     foreach (var gameObject in Singleton.Instance.GameObjects)
        //     {
        //         if (gameObject is BubbleGrid bubble)
        //         {
        //             bubble.Position = new Vector2(bubble.Position.X, bubble.Position.Y + Singleton.SizeBubbleHeight);
        //         }
        //     }
        // }
    }
}