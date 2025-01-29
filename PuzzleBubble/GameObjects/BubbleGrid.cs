using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PuzzleBubble;

namespace PuzzleBubble
{
    class BubbleGrid : GameObject
    {
        public int Score;
        public float Speed;
        public float DistanceMoved = 0;

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

                newY += 30;
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
    }
}