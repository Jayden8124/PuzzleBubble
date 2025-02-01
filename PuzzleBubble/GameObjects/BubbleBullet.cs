using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleBubble
{
    class BubbleBullet : Bubble
    {
        public float Angle;
        public float Speed;
        public float DistanceMoved;

        public BubbleBullet(Texture2D texture) : base(texture)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Viewport, Color.White);
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            DistanceMoved += Math.Abs(Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (DistanceMoved >= Singleton.SCREENHEIGHT - 300)
            {
                IsActive = false;
            }

            Velocity.X = (float)-Math.Sin(Angle) * Speed;
            Velocity.Y = (float)-Math.Cos(Angle) * Speed;

            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Position.X < 559 || Position.X + Rectangle.Width > 1259)
            {
                Angle = -Angle;
            }

            foreach (var obj in GetNearbyBubbles(gameObjects))
            {
                if (obj is BubbleGrid bubble) // ตรวจสอบให้แน่ใจว่า obj เป็น BubbleGrid
                {
                    if (this.IsTouching(bubble))
                    {
                        if (this.Viewport == bubble.Viewport) // สีเดียวกัน
                        {
                            List<BubbleGrid> connectedBubbles = FindConnectedBubbles(bubble, gameObjects);

                            if (connectedBubbles.Count >= 3)
                            {
                                foreach (var b in connectedBubbles)
                                {
                                    b.IsActive = false;
                                }
                                this.IsActive = false;
                            }
                            else
                            {
                                AttachToGrid(gameObjects, bubble);
                            }
                        }
                        else
                        {
                            AttachToGrid(gameObjects, bubble);
                        }
                        break;
                    }
                }
            }
            base.Update(gameTime, gameObjects);
        }

        public override void Reset()
        {
            Speed = 300f;
            base.Reset();
        }

        private Vector2 SnapToGrid(BubbleGrid referenceBubble)
        {
            float bubbleSize = Singleton.SizeBubbleHeight; // ขนาด Bubble
            float offsetX = (referenceBubble.Position.Y % (2 * bubbleSize) == 0) ? bubbleSize / 2 : 0;

            float newX = referenceBubble.Position.X + offsetX;
            float newY = referenceBubble.Position.Y + bubbleSize;

            return new Vector2(newX, newY);
        }

        private List<BubbleGrid> FindConnectedBubbles(BubbleGrid startBubble, List<GameObject> gameObjects)
        {
            List<BubbleGrid> connectedBubbles = new List<BubbleGrid>();
            Queue<BubbleGrid> queue = new Queue<BubbleGrid>();
            HashSet<BubbleGrid> visited = new HashSet<BubbleGrid>();

            queue.Enqueue(startBubble);
            visited.Add(startBubble);

            while (queue.Count > 0)
            {
                BubbleGrid current = queue.Dequeue();
                connectedBubbles.Add(current);

                foreach (var obj in GetNearbyBubbles(gameObjects))
                {
                    if (obj is BubbleGrid neighbor && !visited.Contains(neighbor)) // ตรวจสอบว่าเป็น BubbleGrid ก่อน
                    {
                        if (current.Viewport == neighbor.Viewport && current.IsTouching(neighbor))
                        {
                            queue.Enqueue(neighbor);
                            visited.Add(neighbor);
                        }
                    }
                }
            }
            return connectedBubbles;
        }

        private void AttachToGrid(List<GameObject> gameObjects, BubbleGrid referenceBubble)
        {
            this.Velocity = Vector2.Zero;
            this.IsActive = false; // ปิดใช้งานกระสุน

            Console.WriteLine($"Before Snap: {Position}");
            Vector2 newPosition = SnapToGrid(referenceBubble);
            Console.WriteLine($"After Snap: {Position}");

            foreach (var obj in gameObjects)
            {
                if (obj is BubbleGrid existingBubble && existingBubble.Position == newPosition)
                {
                    return; // ถ้ามีอยู่แล้ว ไม่ต้องสร้างใหม่
                }
            }

            BubbleGrid newBubble = new BubbleGrid(this._texture)
            {
                Position = newPosition,
                Viewport = this.Viewport,
                IsActive = true
            };

            gameObjects.Add(newBubble);

            // **ลบตัวเองออกจากลิสต์**
            gameObjects.Remove(this);
        }


        private List<GameObject> GetNearbyBubbles(List<GameObject> gameObjects)
        {
            List<GameObject> nearbyBubbles = new List<GameObject>();
            float detectionRange = 100f; // ตรวจสอบเฉพาะ Bubble ในระยะ 100px

            foreach (var obj in gameObjects)
            {
                if (obj is BubbleGrid && Vector2.Distance(this.Position, obj.Position) < detectionRange)
                {
                    nearbyBubbles.Add(obj);
                }
            }
            return nearbyBubbles;
        }
        public bool IsTouching(Bubble other)
        {
            float distance = Vector2.Distance(this.Position, other.Position);
            return distance < Singleton.SizeBubbleHeight; // ถ้าระยะห่างน้อยกว่าขนาด Bubble แสดงว่าชนกัน
        }

    }
}