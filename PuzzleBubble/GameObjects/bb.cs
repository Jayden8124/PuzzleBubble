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
    class bb : GameObject
    {
        public Keys Left, Right, Fire;
        public bb(Texture2D texture) : base(texture)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Viewport, Color.White);
            // spriteBatch.Draw(_texture, Position, Color.White);
            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            base.Reset();
        }
        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            

            base.Update(gameTime, gameObjects);
        }

    }
}