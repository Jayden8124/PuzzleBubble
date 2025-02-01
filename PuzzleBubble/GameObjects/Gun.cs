using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

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
            base.Update(gameTime, gameObjects);
        }

        private void FireBubble(List<GameObject> gameObjects)
        {
            BubbleBullet bullet = GetRandomBubble();
            bullet.Position = new Vector2(this.Position.X + (Viewport.Width / 2), this.Position.Y);
            float angle = Rotation;
            bullet.Angle = angle;
            bullet.Speed = 300f;
            bullet.IsActive = true;
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
