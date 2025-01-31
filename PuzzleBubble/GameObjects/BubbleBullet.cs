using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleBubble
{
    class BubbleBullet : Bubble
    {
        public float Angle;
        public float Speed;
        public float DistanceMoved;

        public BubbleBullet(Texture2D texture) : base(texture)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Viewport, Color.White);
            base.Draw(spriteBatch);
        }

        
        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            DistanceMoved += Math.Abs(Velocity.Y * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond);
            
            if (DistanceMoved >= Singleton.SCREENHEIGHT - 300)
            {
                IsActive = false;
            }
            // if(Position.Y >= Singleton.SCREENHEIGHT -65)
            // {
            //     IsActive = false;
            // } 

            Console.WriteLine($"DistanceMoved: {DistanceMoved}, Position: {Position.Y}, IsActive: {IsActive}");

            Velocity.X = (float)-Math.Sin(Angle) * Speed;
            Velocity.Y = (float)-Math.Cos(Angle) * Speed;

            Position += Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

            if (Position.X < 515 || Position.X + Rectangle.Width > 1295)
            {
                Angle = -Angle;
            }
            // if (Position.Y < 0)
            // {
            //     Position.X = 0;
            //     Position.Y = 0;
            //     // Velocity = Vector2.Zero;
            // }
            // Velocity = Vector2.Zero;
            base.Update(gameTime, gameObjects);
        }
        public override void Reset()
        {
            Speed = 300f;
            // DistanceMoved = 0;
            base.Reset();
        }
    }
}