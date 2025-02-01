using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using System;

namespace PuzzleBubble
{
    class Gun : GameObject
    {
        public Keys Left, Right, Fire;
        public BubbleBullet bubbleBulletYellow;
        public BubbleBullet bubbleBulletBlue;
        public BubbleBullet bubbleBulletBrown;
        public BubbleBullet bubbleBulletBlack;
        public BubbleBullet bubbleBulletRed;
        public SoundEffect fireSound;
        // float rotationSpeed = 3f; // Speed of gun rotation
        float minRotation = MathHelper.ToRadians(-45); // Min rotation
        float maxRotation = MathHelper.ToRadians(45);  // Max rotation
                                                       // Clamp the rotation to a specific range

        public Gun(Texture2D texture) : base(texture)
        {
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            if (Singleton.Instance.CurrentGameState != Singleton.GameState.GamePlaying) return;
            var kstate = Singleton.Instance.CurrentKey;
            float speed = 2f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(Left))
            {
                Rotation -= speed;
            }
            if (kstate.IsKeyDown(Right))
            {
                Rotation += speed;
            }
            if (kstate.IsKeyDown(Fire) && Singleton.Instance.PreviousKey.IsKeyUp(Fire))
            {
                FireBubble(gameObjects);
            }
            Rotation = MathHelper.Clamp(Rotation, minRotation, maxRotation);

            base.Update(gameTime, gameObjects);
        }

        private void FireBubble(List<GameObject> gameObjects)
        {

            BubbleBullet bullet = GetRandomBubble();
            Vector2 gunCenter = new Vector2(
                Position.X * (float)Math.Cos(Rotation - MathHelper.PiOver2) / 20 + 875, // หาตำแหน่ง X ของปืน 875
                                                                                        // Rotation >= 0 ? Position.Y + (float)Math.Sin(Rotation - MathHelper.PiOver2) * (Singleton.GunHeight - 65) // ขยับขึ้นให้เป็นจุดกลางของปืน}
                                                                                        // : Position.Y + (float)Math.Sin(Rotation - MathHelper.PiOver2) * (Singleton.GunHeight - 60)
                Position.Y + (float)Math.Sin(Rotation - MathHelper.PiOver2) * (Singleton.GunHeight - 65) // ขยับขึ้นให้เป็นจุดกลางของปืน
            );

            // ตั้งค่า preview bullet ให้อยู่ตรงกลางของปืน
            bullet.Position = gunCenter;
            // bullet.Velocity = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * 50;
            bullet.Angle = -Rotation; // ไม่ให้หมุน
            bullet.IsActive = true; // แสดง preview เสมอ
            // bullet.Position = new Vector2(this.Position.X + (Viewport.Width / 2), this.Position.Y - 200);
            // float angle = Rotation;
            // bullet.Angle = -angle;
            bullet.Speed = 300f;
            // bullet.IsActive = true;
            gameObjects.Add(bullet);
            fireSound.Play();
        }

        private BubbleBullet GetRandomBubble()
        {
            int r = Singleton.Instance.Random.Next(5);
            switch (r)
            {
                // case 0: return (BubbleBullet)bubbleBulletYellow.Clone();
                // case 1: return (BubbleBullet)bubbleBulletBlue.Clone();
                // case 2: return (BubbleBullet)bubbleBulletBrown.Clone();
                case 3: return (BubbleBullet)bubbleBulletBlack.Clone();
                default: return (BubbleBullet)bubbleBulletRed.Clone();
                    // default: return (BubbleBullet)bubbleBulletYellow.Clone();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Viewport, Color.White, Rotation, new Vector2(Viewport.Width / 2, Viewport.Height), 1f, SpriteEffects.None, 0f);
            base.Draw(spriteBatch);
        }
    }
}