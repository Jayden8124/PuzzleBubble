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
            Velocity.X = (float)-Math.Sin(Angle) * Speed;
            Velocity.Y = (float)-Math.Cos(Angle) * Speed;
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Position.Y < 100)
            {
                Position.Y = 100;
                Position.X = (float)Math.Round(Position.X / Singleton.SizeBubbleWidth) * Singleton.SizeBubbleWidth;
            }

            if (Position.X < 559 || Position.X + Rectangle.Width > 1259)
            {
                Angle = -Angle;
            }
            foreach (var obj in GetNearbyBubbles(gameObjects))
            {
                if (obj is BubbleGrid bubble)
                {
                    if (this.IsTouching(bubble))
                    {
                        if (this.Viewport == bubble.Viewport)
                        {
                            List<BubbleGrid> connectedBubbles = FindConnectedBubbles(bubble, gameObjects);
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

        public override void Reset()
        {
            Speed = 300f;
            base.Reset();
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
                foreach (var neighbor in GetNeighbors(current, gameObjects))
                {
                    if (!visited.Contains(neighbor))
                    {
                        if (current.Viewport == neighbor.Viewport)
                        {
                            queue.Enqueue(neighbor);
                            visited.Add(neighbor);
                        }
                    }
                }
            }
            return connectedBubbles;
        }

        private void RemoveFloatingBubbles(List<GameObject> gameObjects)
        {
            List<BubbleGrid> allBubbles = new List<BubbleGrid>();
            foreach (var obj in gameObjects)
            {
                if (obj is BubbleGrid bg && bg.IsActive) allBubbles.Add(bg);
            }
            HashSet<BubbleGrid> connectedToTop = new HashSet<BubbleGrid>();
            foreach (var bubble in allBubbles)
            {
                if (bubble.Row == 0)
                {
                    Queue<BubbleGrid> queue = new Queue<BubbleGrid>();
                    queue.Enqueue(bubble);
                    connectedToTop.Add(bubble);
                    while (queue.Count > 0)
                    {
                        var current = queue.Dequeue();
                        foreach (var n in GetNeighbors(current, gameObjects))
                        {
                            if (n.IsActive && !connectedToTop.Contains(n))
                            {
                                connectedToTop.Add(n);
                                queue.Enqueue(n);
                            }
                        }
                    }
                }
            }
            foreach (var bubble in allBubbles)
            {
                if (!connectedToTop.Contains(bubble))
                {
                    Singleton.Instance.Score += 10;
                    bubble.IsActive = false;
                }
            }
        }

        private IEnumerable<BubbleGrid> GetNeighbors(BubbleGrid bubble, List<GameObject> gameObjects)
        {
            List<BubbleGrid> neighbors = new List<BubbleGrid>();
            int r = bubble.Row;
            int c = bubble.Col;
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
                    foreach (var off in offsets)
                    {
                        if (bg.Row == r + off[0] && bg.Col == c + off[1])
                        {
                            neighbors.Add(bg);
                        }
                    }
                }
            }
            return neighbors;
        }

        private void AttachToGrid(List<GameObject> gameObjects, BubbleGrid referenceBubble)
        {
            this.Velocity = Vector2.Zero;
            this.IsActive = false;
            (int newRow, int newCol) = FindGridSlot(referenceBubble, this.Position, gameObjects);
            foreach (var obj in gameObjects)
            {
                if (obj is BubbleGrid existingBubble && existingBubble.Row == newRow && existingBubble.Col == newCol)
                {
                    gameObjects.Remove(this);
                    return;
                }
            }
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
            for (int i = 0; i < offsets.Length; i++)
            {
                int nr = r + offsets[i][0];
                int nc = c + offsets[i][1];
                if (nr < 0 || nc < 0) continue;
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

        public bool IsTouching(Bubble other)
        {
            float distance = Vector2.Distance(this.Position, other.Position);
            return distance < Singleton.SizeBubbleHeight;
        }
    }
}
