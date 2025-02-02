using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

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

        // ฟิลด์สำหรับเก็บลูกที่กำลังจะยิง (preview/loaded bubble)
        private BubbleBullet loadedBubble;

        // จำกัดมุมหมุนของปืน
        float minRotation = MathHelper.ToRadians(-45);
        float maxRotation = MathHelper.ToRadians(45);

        public Gun(Texture2D texture) : base(texture)
        {
        }

        public override void Reset()
        {
            // กำหนดให้ loadedBubble เริ่มต้นเป็นลูกบอลแบบสุ่ม
            loadedBubble = GetRandomBubble();
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            if (Singleton.Instance.CurrentGameState != Singleton.GameState.GamePlaying) return;
            var kstate = Singleton.Instance.CurrentKey;
            float speed = 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
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

        /// <summary>
        /// ฟังก์ชันยิงลูกบอล โดยใช้ loadedBubble เป็นลูกที่ยิง จากนั้นสร้าง loadedBubble ใหม่
        /// </summary>
        private void FireBubble(List<GameObject> gameObjects)
        {
            BubbleBullet bullet = loadedBubble;
            Vector2 gunCenter = new Vector2(
                Position.X * (float)Math.Cos(Rotation - MathHelper.PiOver2) / 20 + 875, 
                Position.Y + (float)Math.Sin(Rotation - MathHelper.PiOver2) * (Singleton.GunHeight - 65) 
            );

            bullet.Position = gunCenter;
            bullet.Angle = -Rotation; // ไม่ให้หมุน
            bullet.IsActive = true; // แสดง preview เสมอ
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
                // หากต้องการใช้ลูกอื่นๆ ให้เปิด comment ได้ตามที่ต้องการ
                case 0: return (BubbleBullet)bubbleBulletYellow.Clone();
                case 1: return (BubbleBullet)bubbleBulletBlue.Clone();
                case 2: return (BubbleBullet)bubbleBulletBrown.Clone();
                case 3: return (BubbleBullet)bubbleBulletBlack.Clone();
                default: return (BubbleBullet)bubbleBulletRed.Clone();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // วาดปืน
            spriteBatch.Draw(_texture, Position, Viewport, Color.White, Rotation, new Vector2(Viewport.Width / 2, Viewport.Height), 1f, SpriteEffects.None, 0f);

            // วาด preview bubble (loadedBubble) ที่ตำแหน่งที่คุณต้องการ
            if (loadedBubble != null)
            {

                Vector2 previewPosition = new Vector2(Position.X, Position.Y - 50);
                // กำหนดตำแหน่งของ loadedBubble เพื่อให้วาดที่ตำแหน่ง preview
                loadedBubble.Position = previewPosition;
                loadedBubble.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }
    }
}
