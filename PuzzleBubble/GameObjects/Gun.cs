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
    class Gun : GameObject
    {
        public Bubble Bubble;
        public Keys Left, Right, Fire;

        public Gun(Texture2D texture) : base(texture)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Viewport, Color.White, Rotation, new Vector2(Rectangle.Width / 2, Rectangle.Height), 1.0f, SpriteEffects.None, 0f);
            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            base.Reset();
        }
        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            float rotationSpeed = 3f; // Rotation speed in radians per second
            float minRotation = MathHelper.ToRadians(-45); // Allow rotation to -90 degrees
            float maxRotation = MathHelper.ToRadians(45);  // Allow rotation to 90 degrees

            KeyboardState currentKeyState = Keyboard.GetState();

            // Rotate left when Left arrow is pressed
            if (currentKeyState.IsKeyDown(Keys.Left))
            {
                Rotation -= rotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            // Rotate right when Right arrow is pressed
            if (currentKeyState.IsKeyDown(Keys.Right))
            {
                Rotation += rotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Clamp rotation within range
            Rotation = MathHelper.Clamp(Rotation, minRotation, maxRotation);

            // Fire bubble when Space key is pressed
            if (Singleton.Instance.CurrentKey.IsKeyDown(Fire) &&
                Singleton.Instance.PreviousKey != Singleton.Instance.CurrentKey)
            {
                var newBubble = Bubble.Clone() as Bubble;
                Vector2 gunTipPosition = new Vector2(
            Position.X + (float)Math.Cos(Rotation - MathHelper.PiOver2) * (Singleton.GunHeight + 60),
            Position.Y + (float)Math.Sin(Rotation - MathHelper.PiOver2) * (Singleton.GunHeight + 60));

                // Set bubble's initial position and angle
                newBubble.Position = gunTipPosition;
                newBubble.Velocity = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * 50;
                newBubble.Angle = -Rotation;
                newBubble.Reset();
                gameObjects.Add(newBubble);
            }

            // Update previous key state
            Singleton.Instance.PreviousKey = currentKeyState;

            base.Update(gameTime, gameObjects);
        }

    }
}