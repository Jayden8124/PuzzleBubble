using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleBubble
{
    class BubbleBullet : Bubble
    {
        public float Angle;
        public float Speed;
        public bool CanCheckCollision { get; set; } = false;

        public BubbleBullet(Texture2D texture) : base(texture) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Viewport, Color.White);
            base.Draw(spriteBatch);
        }

        /// <summary>
        /// อัปเดตการเคลื่อนไหวและตรวจจับการชนของลูกกระสุน
        /// </summary>
        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            // คำนวณเวกเตอร์ความเร็วตามมุมและความเร็วที่กำหนด
            Velocity.X = (float)-Math.Sin(Angle) * Speed;
            Velocity.Y = (float)-Math.Cos(Angle) * Speed;
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // หากลูกกระสุนออกนอกขอบของพื้นที่เล่น ให้เปลี่ยนทิศ (กระเด้ง)
            if (Position.X < 559 || Position.X + Rectangle.Width > 1259)
            {
                Angle = -Angle;
            }

            // ตรวจจับการชนกับ BubbleGrid ที่อยู่ใกล้ ๆ
            foreach (var obj in GetNearbyBubbles(gameObjects))
            {
                if (obj is BubbleGrid bubble)
                {
                    if (this.IsTouching(bubble))
                    {
                        // หากสี (Viewport) ของลูกกระสุนตรงกับลูกในตาราง
                        if (this.Viewport == bubble.Viewport)
                        {
                            List<BubbleGrid> connectedBubbles = FindConnectedBubbles(bubble, gameObjects);
                            // หากลูกบอลที่เชื่อมต่อกันมีมากกว่า 2 ลูก
                            if (connectedBubbles.Count >= 2)
                            {
                                foreach (var b in connectedBubbles)
                                {
                                    b.IsActive = false;
                                }
                                Singleton.Instance.Score += connectedBubbles.Count * 10;
                                RemoveFloatingBubbles(gameObjects);
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

        /// <summary>
        /// รีเซ็ตสถานะของลูกกระสุน
        /// </summary>
        public override void Reset()
        {
            Speed = 300f;
            base.Reset();
        }

        /// <summary>
        /// ตรวจสอบว่าลูกนี้สัมผัสกับลูกอื่นหรือไม่
        /// </summary>
        public bool IsTouching(Bubble other)
        {
            float distance = Vector2.Distance(this.Position, other.Position);
            return distance < Singleton.SizeBubbleHeight;
        }

        #region Private Helper Methods

        /// <summary>
        /// ค้นหาลูกบอลที่เชื่อมต่อกัน (มีสีเดียวกัน) โดยใช้ BFS
        /// </summary>
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

                foreach (var neighbor in GetNeighbors(current, gameObjects))
                {
                    if (!visited.Contains(neighbor) && current.Viewport == neighbor.Viewport)
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }
            return connectedBubbles;
        }

        /// <summary>
        /// ลบลูกบอลที่ไม่เชื่อมต่อกับด้านบน (ลูกลอย) ออกจากตาราง
        /// </summary>
        // private void RemoveFloatingBubbles(List<GameObject> gameObjects)
        // {
        //     List<BubbleGrid> allBubbles = new List<BubbleGrid>();
        //     foreach (var obj in gameObjects)
        //     {
        //         if (obj is BubbleGrid bg && bg.IsActive)
        //             allBubbles.Add(bg);
        //     }

        //     HashSet<BubbleGrid> connectedToTop = new HashSet<BubbleGrid>();

        //     // เริ่มจากลูกบอลในแถวบนสุด (Row 0)
        //     foreach (var bubble in allBubbles)
        //     {
        //         if (bubble.Row == 0)
        //         {
        //             Queue<BubbleGrid> queue = new Queue<BubbleGrid>();
        //             queue.Enqueue(bubble);
        //             connectedToTop.Add(bubble);

        //             while (queue.Count > 0)
        //             {
        //                 var current = queue.Dequeue();
        //                 foreach (var neighbor in GetNeighbors(current, gameObjects))
        //                 {
        //                     if (neighbor.IsActive && !connectedToTop.Contains(neighbor))
        //                     {
        //                         connectedToTop.Add(neighbor);
        //                         queue.Enqueue(neighbor);
        //                     }
        //                 }
        //             }
        //         }
        //     }

        //     // ลูกที่ไม่ได้เชื่อมต่อกับด้านบนให้ลบออกและเพิ่มคะแนน
        //     foreach (var bubble in allBubbles)
        //     {
        //         if (!connectedToTop.Contains(bubble))
        //         {
        //             Singleton.Instance.Score += 10;
        //             bubble.IsActive = false;
        //         }
        //     }
        // }

       private void RemoveFloatingBubbles(List<GameObject> gameObjects)
{
    List<BubbleGrid> allBubbles = new List<BubbleGrid>();
    foreach (var obj in gameObjects)
    {
        if (obj is BubbleGrid bg && bg.IsActive)
            allBubbles.Add(bg);
    }

    // If there are no bubbles, simply return.
    if (!allBubbles.Any())
        return;

    // Now it is safe to call Min()
    float topY = allBubbles.Min(b => b.Position.Y);
    HashSet<BubbleGrid> connectedToTop = new HashSet<BubbleGrid>();

    // Identify bubbles that are at the top.
    var topBubbles = allBubbles.Where(b => Math.Abs(b.Position.Y - topY) < 1f);
    Queue<BubbleGrid> queue = new Queue<BubbleGrid>();
    foreach (var bubble in topBubbles)
    {
        queue.Enqueue(bubble);
        connectedToTop.Add(bubble);
    }

    // Continue with BFS to mark all bubbles connected to the top.
    while (queue.Count > 0)
    {
        var current = queue.Dequeue();
        foreach (var neighbor in GetNeighbors(current, gameObjects))
        {
            if (neighbor.IsActive && !connectedToTop.Contains(neighbor))
            {
                connectedToTop.Add(neighbor);
                queue.Enqueue(neighbor);
            }
        }
    }

    // Remove (deactivate) bubbles not connected to the top.
    foreach (var bubble in allBubbles)
    {
        if (!connectedToTop.Contains(bubble))
        {
            Singleton.Instance.Score += 10;
            bubble.IsActive = false;
        }
    }
}



        /// <summary>
        /// ดึงเพื่อนบ้านของ BubbleGrid ที่กำหนด (ใช้ตำแหน่ง Row และ Col)
        /// </summary>
        private IEnumerable<BubbleGrid> GetNeighbors(BubbleGrid bubble, List<GameObject> gameObjects)
        {
            List<BubbleGrid> neighbors = new List<BubbleGrid>();
            int r = bubble.Row;
            int c = bubble.Col;

            // Offsets สำหรับแถวคู่และคี่
            int[][] evenOffsets = new int[][]
            {
                new int[]{ 0, -1}, new int[]{ 0, 1}, new int[]{ -1, 0}, new int[]{ -1, -1},
                new int[]{ 1, 0}, new int[]{ 1, -1}
            };

            int[][] oddOffsets = new int[][]
            {
                new int[]{ 0, -1}, new int[]{ 0, 1}, new int[]{ -1, 0}, new int[]{ -1, 1},
                new int[]{ 1, 0}, new int[]{ 1, 1}
            };

            int[][] offsets = (r % 2 == 0) ? evenOffsets : oddOffsets;

            foreach (var obj in gameObjects)
            {
                if (obj is BubbleGrid bg && bg.IsActive)
                {
                    foreach (var offset in offsets)
                    {
                        if (bg.Row == r + offset[0] && bg.Col == c + offset[1])
                        {
                            neighbors.Add(bg);
                        }
                    }
                }
            }

            // Console.WriteLine($"Bubble {bubble.Row},{bubble.Col} has {neighbors.Count} neighbors");
            return neighbors;
        }

        /// <summary>
        /// แนบลูกกระสุนเข้ากับตารางโดยการค้นหาตำแหน่งที่เหมาะสม
        /// </summary>
        private void AttachToGrid(List<GameObject> gameObjects, BubbleGrid referenceBubble)
        {
            // หยุดการเคลื่อนไหวและทำให้ bullet ไม่ active อีกต่อไป
            this.Velocity = Vector2.Zero;
            this.IsActive = false;

            (int newRow, int newCol) = FindGridSlot(referenceBubble, this.Position, gameObjects);

            // หากตำแหน่งที่เลือกมีลูกอยู่แล้ว ให้ลบ bullet นี้ออก
            foreach (var obj in gameObjects)
            {
                if (obj is BubbleGrid existingBubble && existingBubble.Row == newRow && existingBubble.Col == newCol)
                {
                    gameObjects.Remove(this);
                    return;
                }
            }

            // สร้าง BubbleGrid ใหม่จากลูกกระสุนและเพิ่มเข้าไปใน gameObjects
            BubbleGrid newBubble = new BubbleGrid(this._texture)
            {
                Row = newRow,
                Col = newCol,
                Position = BubbleGrid.GetPositionFromRowCol(newRow, newCol),
                Viewport = this.Viewport,
                IsActive = true
            };

            gameObjects.Add(newBubble);
            gameObjects.Remove(this);
        }

        /// <summary>
        /// ค้นหาตำแหน่ง Grid Slot ที่เหมาะสมสำหรับแนบลูกกระสุน
        /// </summary>
        private (int, int) FindGridSlot(BubbleGrid referenceBubble, Vector2 bulletPos, List<GameObject> gameObjects)
        {
            int r = referenceBubble.Row;
            int c = referenceBubble.Col;

            int[][] evenOffsets = new int[][]
            {
                new int[]{ 0, -1}, new int[]{ 0, 1}, new int[]{ -1, 0}, new int[]{ -1, -1},
                new int[]{ 1, 0}, new int[]{ 1, -1}
            };

            int[][] oddOffsets = new int[][]
            {
                new int[]{ 0, -1}, new int[]{ 0, 1}, new int[]{ -1, 0}, new int[]{ -1, 1},
                new int[]{ 1, 0}, new int[]{ 1, 1}
            };

            int[][] offsets = (r % 2 == 0) ? evenOffsets : oddOffsets;

            float minDist = float.MaxValue;
            int bestRow = r;
            int bestCol = c;

            // ค้นหาตำแหน่งที่ใกล้ bullet มากที่สุดและยังไม่ถูกครอบครอง
            for (int i = 0; i < offsets.Length; i++)
            {
                int nr = r + offsets[i][0];
                int nc = c + offsets[i][1];

                if (nr < 0 || nc < 0)
                    continue;

                bool occupied = false;
                foreach (var obj in gameObjects)
                {
                    if (obj is BubbleGrid bg && bg.IsActive && bg.Row == nr && bg.Col == nc)
                    {
                        occupied = true;
                        break;
                    }
                }

                if (!occupied)
                {
                    Vector2 pos = BubbleGrid.GetPositionFromRowCol(nr, nc);
                    float dist = Vector2.Distance(pos, bulletPos);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        bestRow = nr;
                        bestCol = nc;
                    }
                }
            }
            return (bestRow, bestCol);
        }

        /// <summary>
        /// ค้นหาลูกบอลที่อยู่ใกล้กับตำแหน่งของลูกกระสุน
        /// </summary>
        private List<GameObject> GetNearbyBubbles(List<GameObject> gameObjects)
        {
            List<GameObject> nearbyBubbles = new List<GameObject>();
            float detectionRange = 100f;

            foreach (var obj in gameObjects)
            {
                if (obj is BubbleGrid && Vector2.Distance(this.Position, obj.Position) < detectionRange)
                {
                    nearbyBubbles.Add(obj);
                }
            }
            return nearbyBubbles;
        }

        #endregion
    }
}
