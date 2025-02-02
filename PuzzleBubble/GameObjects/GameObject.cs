using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace PuzzleBubble
{
    public class GameObject : ICloneable
    {
        protected Texture2D _texture;

        public Vector2 Position;
        public float Rotation;
        public Vector2 Scale;
        public Vector2 Velocity;
        public string Name;
        public bool IsActive;
        public Rectangle Viewport;

        public Rectangle Rectangle
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, Viewport.Width, Viewport.Height); }
        }

        // Expose the texture (useful for when a BubbleBullet becomes a BubbleGrid)
        public Texture2D Texture => _texture;

        public GameObject()
        {
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0f;
            IsActive = true;
        }

        public GameObject(Texture2D texture)
        {
            _texture = texture;
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0f;
            IsActive = true;
        }

        public virtual void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }

        public virtual void Reset()
        {
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
