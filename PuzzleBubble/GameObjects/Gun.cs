using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PuzzleBubble
{
    class Gun : GameObject
    {
        public BubbleBullet bubbleBulletYellow, bubbleBulletBlue, bubbleBulletBrown, bubbleBulletBlack, bubbleBulletRed, currentBubbleBullet, nextBubbleBullet;
        public Keys Left, Right, Fire;
        private float fireDelay = 0.5f;
        private float timeSinceLastShot = 0f;
        private BubbleBullet previewBullet;

        public Gun(Texture2D texture) : base(texture)
        {
            previewBullet = new BubbleBullet(texture);
            previewBullet.IsActive = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (previewBullet != null)
        {
            previewBullet.Draw(spriteBatch);
        }
            spriteBatch.Draw(_texture, Position, Viewport, Color.White, Rotation, new Vector2(Rectangle.Width / 2, Rectangle.Height), 1.0f, SpriteEffects.None, 0f);
            base.Draw(spriteBatch);
        }

        public override void Reset()        
        {
            timeSinceLastShot = 0f;

            base.Reset();
        }
        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            float rotationSpeed = 3f; // Rotation speed in radians per second
            float minRotation = MathHelper.ToRadians(-45); // Allow rotation to -45 degrees
            float maxRotation = MathHelper.ToRadians(45);  // Allow rotation to 45 degrees

            KeyboardState currentKeyState = Keyboard.GetState(); // Get All Keyboard State

            // Rotate left when Left arrow is pressed
            if (currentKeyState.IsKeyDown(Left))
            {
                Rotation -= rotationSpeed * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
            }
            // Rotate right when Right arrow is pressed
            if (currentKeyState.IsKeyDown(Right))
            {
                Rotation += rotationSpeed * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
            }

            // Clamp rotation within range
            Rotation = MathHelper.Clamp(Rotation, minRotation, maxRotation);

            // Fire bubble when Space key is pressed
            if (Singleton.Instance.CurrentKey.IsKeyDown(Fire) &&
                Singleton.Instance.PreviousKey != Singleton.Instance.CurrentKey &&
                timeSinceLastShot >= fireDelay)
            {
                ChangeBubbleBullet();
                FireBullet(gameObjects);
                timeSinceLastShot = 0f;
                //_audioManager.Play("BubbleShot");
                currentBubbleBullet = nextBubbleBullet;
            }

            timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update previous key state
            Singleton.Instance.PreviousKey = currentKeyState;

            UpdatePreviewBulletPosition();
    
            base.Update(gameTime, gameObjects);

            gameObjects.RemoveAll(obj => !obj.IsActive);

        }
        public void UpdatePreviewBulletPosition()
    {
        
        Vector2 gunTipPosition = new Vector2(
            Position.X + (float)Math.Cos(Rotation - MathHelper.PiOver2) * (Singleton.GunHeight + 60),
            Position.Y + (float)Math.Sin(Rotation - MathHelper.PiOver2) * (Singleton.GunHeight + 60)
        );

        previewBullet.Position = gunTipPosition;
        previewBullet.Velocity = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * 50;
        previewBullet.Angle = -Rotation;
    }
        public void FireBullet(List<GameObject> gameObjects)
        {
            // Clone currentBubbleBullet to create a new instance
            var newBubble = currentBubbleBullet.Clone() as BubbleBullet;

            Vector2 gunTipPosition = new Vector2(
                Position.X + (float)Math.Cos(Rotation - MathHelper.PiOver2) * (Singleton.GunHeight + 60),
                Position.Y + (float)Math.Sin(Rotation - MathHelper.PiOver2) * (Singleton.GunHeight + 60)
        );

            // ตั้งค่าตำแหน่งของลูกกระสุน
            newBubble.Position = gunTipPosition;
            newBubble.Velocity = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * 50;
            newBubble.Angle = -Rotation;

            newBubble.Reset();
            gameObjects.Add(newBubble);
            previewBullet.IsActive = false;
        }

        public void ChangeBubbleBullet()
        {
            // Randomly select a bubble type
            BubbleBullet[] bubbleBullets = { bubbleBulletYellow, bubbleBulletBlue, bubbleBulletBrown, bubbleBulletBlack, bubbleBulletRed };
            nextBubbleBullet = bubbleBullets[Singleton.Instance.Random.Next(bubbleBullets.Length)];

            if (currentBubbleBullet == null)
            {
                currentBubbleBullet = bubbleBulletYellow;
            }
            previewBullet.IsActive = true;
        }

    }
}
