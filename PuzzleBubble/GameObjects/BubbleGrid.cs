using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleBubble
{
    class BubbleGrid : Bubble
    {
        public int Row;
        public int Col;
        public int Score;

        public BubbleGrid(Texture2D texture) : base(texture)
        {
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            base.Update(gameTime, gameObjects);
        }

        public static Vector2 GetPositionFromRowCol(int row, int col)
        {
            float x = 560 + (70 * col);
            if (row % 2 != 0) x += 35;
            float y = 100 + (70 * row);
            return new Vector2(x, y);
        }
    }
}