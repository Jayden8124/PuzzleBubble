using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Runtime.ExceptionServices;

namespace PuzzleBubble
{
    class Gun : GameObject
    {
        public BubbleBullet bubbleBulletYellow, bubbleBulletBlue, bubbleBulletBrown, bubbleBulletBlack, bubbleBulletRed;
        public BubbleBullet currentBubbleBullet, previewBullet;
        public Keys Left, Right, Fire;
        private float fireDelay = 0.5f;
        private float timeSinceLastShot = 0f;
        public SoundEffect fireSound;

        public Gun(Texture2D texture) : base(texture)
        {
            previewBullet = new BubbleBullet(texture);
            previewBullet.IsActive = false;
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw preview bullet if it's active
            if (previewBullet.IsActive)
            {
                previewBullet.Draw(spriteBatch);  // Preview Bullet
            }

            // Draw the gun
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
            float rotationSpeed = 3f; // Speed of gun rotation
            float minRotation = MathHelper.ToRadians(-45); // Min rotation
            float maxRotation = MathHelper.ToRadians(45);  // Max rotation

            KeyboardState currentKeyState = Keyboard.GetState(); // Get the current keyboard state

            // Rotate gun to the left or right
            if (currentKeyState.IsKeyDown(Left))
            {
                Rotation -= rotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (currentKeyState.IsKeyDown(Right))
            {
                Rotation += rotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Clamp the rotation to a specific range
            Rotation = MathHelper.Clamp(Rotation, minRotation, maxRotation);

            // Fire bubble if the Spacebar is pressed and the delay is met
            if (Singleton.Instance.CurrentKey.IsKeyDown(Fire) && Singleton.Instance.PreviousKey != Singleton.Instance.CurrentKey && timeSinceLastShot >= fireDelay)
            {
                FireBullet(gameObjects);
                timeSinceLastShot = 0f;
            }

            // Update time since the last shot
            timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update preview bubble position
            UpdatePreviewBulletPosition();

            base.Update(gameTime, gameObjects);

            // Remove inactive game objects
            gameObjects.RemoveAll(obj => !obj.IsActive);
        }

        // Update the position of the preview bullet based on the gun's rotation
        // public void UpdatePreviewBulletPosition()
        // {
        //     Vector2 gunTipPosition = new Vector2(
        //         Position.X * (float)Math.Cos(Rotation - MathHelper.PiOver2)/20 + 875,
        //         Position.Y + (float)Math.Sin(Rotation - MathHelper.PiOver2) * (Singleton.GunHeight - 60)
        //     );
        //     Console.WriteLine($"GunTipPosition: {Position.X}, {Position.Y}");
        //     Console.WriteLine($"GunTipPosition: Math.Cos{MathHelper.PiOver2})");
        //     Console.WriteLine($"{Rotation}");

        //     // Set preview bullet position and velocity
        //     previewBullet.Position = gunTipPosition;
        //     previewBullet.Velocity = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * 50;
        //     previewBullet.Angle = -Rotation;
        //     previewBullet.IsActive = true; // Make sure preview bullet is visible
        // }

        public void UpdatePreviewBulletPosition()
        {
            // หาตำแหน่งศูนย์กลางของปืน
            Vector2 gunCenter = new Vector2(
                Position.X * (float)Math.Cos(Rotation - MathHelper.PiOver2)/20 + 875, // หาตำแหน่ง X ของปืน 875
                // Rotation >= 0 ? Position.Y + (float)Math.Sin(Rotation - MathHelper.PiOver2) * (Singleton.GunHeight - 65) // ขยับขึ้นให้เป็นจุดกลางของปืน}
                // : Position.Y + (float)Math.Sin(Rotation - MathHelper.PiOver2) * (Singleton.GunHeight - 60)
                Position.Y + (float)Math.Sin(Rotation - MathHelper.PiOver2) * (Singleton.GunHeight - 65) // ขยับขึ้นให้เป็นจุดกลางของปืน
            );

            // ตั้งค่า preview bullet ให้อยู่ตรงกลางของปืน
            previewBullet.Position = gunCenter;
            previewBullet.Velocity = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * 50;
            previewBullet.Angle = -Rotation; // ไม่ให้หมุน
            previewBullet.IsActive = true; // แสดง preview เสมอ
        }

        // Fire the current preview bullet
        public void FireBullet(List<GameObject> gameObjects)
        {
            // Clone the preview bullet as a new bubble
            var newBubble = previewBullet.Clone() as BubbleBullet;
            
            fireSound.Play();

            // Set the position and velocity of the fired bubble
            newBubble.Position = previewBullet.Position;
            newBubble.Velocity = previewBullet.Velocity;
            newBubble.Angle = previewBullet.Angle;

            // Reset the fired bubble
            newBubble.Reset();

            // Add the new bubble to the game objects
            gameObjects.Add(newBubble);

            // Deactivate the preview bullet after firing
            previewBullet.IsActive = false;

            // Switch the preview bullet to the next bubble
            ChangeBubbleBullet();
        }

        // Randomly change the bubble type to the next bubble
        public void ChangeBubbleBullet()
        {
            BubbleBullet[] bubbleBullets = { bubbleBulletYellow, bubbleBulletBlue, bubbleBulletBrown, bubbleBulletBlack, bubbleBulletRed };
            int randomIndex = Singleton.Instance.Random.Next(bubbleBullets.Length);

            // Set the current bubble to a new random bubble type
            currentBubbleBullet = bubbleBullets[randomIndex];

            // Set the preview bubble to match the current bubble
            previewBullet = currentBubbleBullet;
            previewBullet.IsActive = true; // Make sure it's active
        }
    }
}
