using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PuzzleBubble
{
    public class BubbleBullet : Bubble
    {
        public float Angle;
        public float Speed;

        public BubbleBullet(Texture2D texture) : base(texture) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Viewport, Color.White);
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            // Compute velocity based on angle and speed.
            Velocity.X = (float)-Math.Sin(Angle) * Speed;
            Velocity.Y = (float)-Math.Cos(Angle) * Speed;
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Bounce off left/right walls.
            if (Position.X < 559 || Position.X + Rectangle.Width > 1259)
                Angle = -Angle;

            // If the top is reached, attach the bubble to the grid.
            if (Position.Y <= 60)
            {
                BubbleManager.AttachBubble(this, gameObjects);
                return;
            }

            // Check collision with any grid bubble.
            Vector2 bulletCenter = Position + new Vector2(Rectangle.Width / 2, Rectangle.Height / 2);
            foreach (var obj in gameObjects.ToList())
            {
                if (obj is BubbleGrid bubble)
                {
                    Vector2 bubbleCenter = bubble.Position + new Vector2(bubble.Viewport.Width / 2, bubble.Viewport.Height / 2);
                    float distance = Vector2.Distance(bulletCenter, bubbleCenter);
                    if (distance < 70) // Rough collision threshold.
                    {
                        BubbleManager.AttachBubble(this, gameObjects);
                        return;
                    }
                }
            }
            base.Update(gameTime, gameObjects);
        }

        public override void Reset()
        {
            Speed = 300f;
            base.Reset();
        }
    }
}
