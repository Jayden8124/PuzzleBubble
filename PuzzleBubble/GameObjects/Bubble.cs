using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleBubble.GameObjects
{
    public class Bubble : _GameObject
    {
        public float Speed { get; set; }
        public float Angle { get; set; }
        public Texture2D Texture { get; set; } // สำหรับเก็บ Texture ของ Bubble
        public int GridSizeX { get; set; } = 80; // ความกว้างของช่องกริด
        public int GridSizeY { get; set; } = 70; // ความสูงของช่องกริด
        public int OffsetX { get; set; } = 320; // Offset สำหรับแกน X
        public int OffsetY { get; set; } = 40;  // Offset สำหรับแกน Y

        public Bubble(Texture2D texture) : base(texture)
        {
        }

        public override void Update(GameTime gameTime, Bubble[,] gameObjects)
        {
            if (!IsActive) return;

            // คำนวณความเร็วตามมุม
            Velocity.X = (float)Math.Cos(Angle) * Speed;
            Velocity.Y = (float)Math.Sin(Angle) * Speed;
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            HandleBoundaryCollision();
            DetectCollision(gameObjects);
        }

        private void HandleBoundaryCollision()
        {
            // ชนขอบซ้าย
            if (Position.X <= OffsetX)
            {
                ReflectAngle();
            }

            // ชนขอบขวา
            if (Position.X + _texture.Width >= OffsetX + (GridSizeX * 8))
            {
                ReflectAngle();
            }

            // ชนขอบบน
            if (Position.Y <= OffsetY)
            {
                IsActive = false;
                AlignToGrid();
            }
        }

        private void ReflectAngle()
        {
            Angle = -Angle;
            Angle += MathHelper.ToRadians(180); // สะท้อนมุม
        }

        private void AlignToGrid()
        {
            int column = (int)((Position.X - OffsetX) / GridSizeX);
            int row = 0; // กริดแถวบนสุด

            Position = GetGridPosition(row, column);
            Singleton.Instance.Shooting = false;
        }

        private Vector2 GetGridPosition(int row, int column)
        {
            return new Vector2(
                (column * GridSizeX) + (row % 2 == 0 ? OffsetX : OffsetX + GridSizeX / 2),
                (row * GridSizeY) + OffsetY
            );
        }

        private void DetectCollision(Bubble[,] gameObjects)
        {
            for (int i = 0; i < gameObjects.GetLength(0); i++)
            {
                for (int j = 0; j < gameObjects.GetLength(1); j++)
                {
                    Bubble targetBubble = gameObjects[i, j];
                    if (targetBubble != null && !targetBubble.IsActive)
                    {
                        if (CheckCollision(targetBubble) <= GridSizeY)
                        {
                            PlaceInGrid(gameObjects, i, j);
                            return;
                        }
                    }
                }
            }
        }

        private void PlaceInGrid(Bubble[,] gameObjects, int i, int j)
        {
            IsActive = false;

            int newRow = i + 1;
            int newCol = (Position.X >= gameObjects[i, j].Position.X)
                ? (j + (i % 2 == 0 ? 0 : 1))
                : (j - (i % 2 == 0 ? 1 : 0));

            newCol = Math.Clamp(newCol, 0, gameObjects.GetLength(1) - 1);
            gameObjects[newRow, newCol] = this;
            Position = GetGridPosition(newRow, newCol);

            CheckRemoveBubble(gameObjects, color, new Vector2(newCol, newRow));
        }

        public int CheckCollision(Bubble other)
        {
            return (int)Vector2.Distance(Position, other.Position);
        }

        private void CheckRemoveBubble(Bubble[,] gameObjects, Color targetColor, Vector2 gridPosition)
        {
            if (!IsValidGridPosition(gridPosition, gameObjects)) return;

            Singleton.Instance.removeBubble.Add(gridPosition);
            gameObjects[(int)gridPosition.Y, (int)gridPosition.X] = null;

            CheckRemoveBubble(gameObjects, targetColor, gridPosition + new Vector2(1, 0)); // Right
            CheckRemoveBubble(gameObjects, targetColor, gridPosition + new Vector2(-1, 0)); // Left
            CheckRemoveBubble(gameObjects, targetColor, gridPosition + new Vector2(0, -1)); // Top
            CheckRemoveBubble(gameObjects, targetColor, gridPosition + new Vector2(0, 1)); // Bottom
        }

        private bool IsValidGridPosition(Vector2 gridPosition, Bubble[,] gameObjects)
        {
            int x = (int)gridPosition.X;
            int y = (int)gridPosition.Y;

            return x >= 0 && y >= 0 &&
                   x < gameObjects.GetLength(1) &&
                   y < gameObjects.GetLength(0) &&
                   gameObjects[y, x]?.color == color;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, color);
            base.Draw(spriteBatch);
        }
    }
}
