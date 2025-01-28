using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleBubble.GameObjects {
	public class _GameObject {
		public Texture2D _texture;

		public Vector2 Position;
		public float Rotation;
		public Vector2 Scale;
		public Color color = Color.White;
		public Vector2 Velocity;

		public string Name;

		public bool IsActive; // Check if the object is active or not

		public Rectangle Rectangle {
			get {
				return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
			}
		}

		public _GameObject(Texture2D texture) {
			_texture = texture;
			Position = Vector2.Zero;
			Scale = Vector2.One;
			Rotation = 0f;
			IsActive = true;
		}

		public virtual void Update(GameTime gameTime, Bubble[,] gameObjects) {
		}
		public virtual void Draw(SpriteBatch spriteBatch) {
		}
	}
}