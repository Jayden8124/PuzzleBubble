using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace PuzzleBubble
{
    public class Gun : GameObject
    {
        public Keys Left, Right, Fire;
        public BubbleBullet bubbleBulletYellow;
        public BubbleBullet bubbleBulletBlue;
        public BubbleBullet bubbleBulletBrown;
        public BubbleBullet bubbleBulletBlack;
        public BubbleBullet bubbleBulletRed;
        public SoundEffect fireSound;

        // Field for the loaded (preview) bubble.
        private BubbleBullet loadedBubble;

        // Limit gun rotation.
        float minRotation = MathHelper.ToRadians(-45);
        float maxRotation = MathHelper.ToRadians(45);

        private float fireCooldown = 1.0f;
        private float timeSinceLastShot = 0f;

        public Gun(Texture2D texture) : base(texture)
        {
        }

        public override void Reset()
        {
            loadedBubble = GetRandomBubble();
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            if (Singleton.Instance.CurrentGameState != Singleton.GameState.GamePlaying)
                return;

            var kstate = Singleton.Instance.CurrentKey;
            float speed = 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(Left))
                Rotation -= speed;
            if (kstate.IsKeyDown(Right))
                Rotation += speed;

            timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(Fire) && Singleton.Instance.PreviousKey.IsKeyUp(Fire)&& timeSinceLastShot >= fireCooldown)
            {
                FireBubble(gameObjects);
                timeSinceLastShot = 0f;
            }
            Rotation = MathHelper.Clamp(Rotation, minRotation, maxRotation);
            base.Update(gameTime, gameObjects);
        }

        private void FireBubble(List<GameObject> gameObjects)
        {
            BubbleBullet bullet = loadedBubble;
            Vector2 gunCenter = new Vector2(
                Position.X * (float)Math.Cos(Rotation - MathHelper.PiOver2) / 20 + 875,
                Position.Y + (float)Math.Sin(Rotation - MathHelper.PiOver2) * (Singleton.GunHeight - 65)
            );
            bullet.Position = gunCenter;
            bullet.Angle = -Rotation; // Fire in the direction the gun is facing.
            bullet.IsActive = true;
            bullet.Speed = 300f;
            gameObjects.Add(bullet);
            loadedBubble = GetRandomBubble();
            fireSound.Play();
        }

        private BubbleBullet GetRandomBubble()
        {
            int r = Singleton.Instance.Random.Next(5);
            switch (r)
            {
                case 0: return (BubbleBullet)bubbleBulletYellow.Clone();
                case 1: return (BubbleBullet)bubbleBulletBlue.Clone();
                case 2: return (BubbleBullet)bubbleBulletBrown.Clone();
                case 3: return (BubbleBullet)bubbleBulletBlack.Clone();
                default: return (BubbleBullet)bubbleBulletRed.Clone();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the gun.
            spriteBatch.Draw(_texture, Position, Viewport, Color.White, Rotation,
                new Vector2(Viewport.Width / 2, Viewport.Height), 1f, SpriteEffects.None, 0f);

            // Draw the preview (loaded) bubble.
            if (loadedBubble != null)
            {
                Vector2 previewPosition = new Vector2(
                    Position.X * (float)Math.Cos(Rotation - MathHelper.PiOver2) / 20 + 875,
                    Position.Y + 2f + (float)Math.Sin(Rotation - MathHelper.PiOver2) * (Singleton.GunHeight - 60));
                loadedBubble.Position = previewPosition;
                loadedBubble.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }
    }
}
