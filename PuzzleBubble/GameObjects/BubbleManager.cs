using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PuzzleBubble
{
    public static class BubbleManager
    {
        /// <summary>
        /// Attaches a fired bubble (bullet) to the grid by snapping it into place.
        /// Then checks for same–color clusters and triggers removal of any floating bubbles.
        /// </summary>
        public static void AttachBubble(BubbleBullet bullet, List<GameObject> gameObjects)
        {
            // Calculate grid coordinates.
            int row = (int)Math.Round((bullet.Position.Y - 100) / 70.0);
            if (row < 0) row = 0;
            int offset = (row % 2 == 1) ? 35 : 0;
            int col = (int)Math.Round((bullet.Position.X - 560 - offset) / 70.0);
            int maxCols = (row % 2 == 0) ? 10 : 9;
            if (col < 0) col = 0;
            if (col >= maxCols) col = maxCols - 1;
            float posX = 560 + col * 70 + offset;
            float posY = 100 + row * 70;

            // Create a new grid bubble using the bullet's texture and "color" (encoded in its Name).
            BubbleGrid newBubble = new BubbleGrid(bullet.Texture)
            {
                Name = bullet.Name.Replace("BubbleBullet", "BubbleGrid"),
                Viewport = bullet.Viewport,
                Row = row,
                Col = col,
                Position = new Vector2(posX, posY),
                IsActive = true
            };

            // Remove the fired bullet and add the new grid bubble.
            gameObjects.Remove(bullet);
            gameObjects.Add(newBubble);

            // Check for a same–color cluster and remove it if three or more bubbles are connected.
            CheckClusters(newBubble, gameObjects);
        }

        private static void CheckClusters(BubbleGrid bubble, List<GameObject> gameObjects)
        {
            // Build a dictionary of grid bubbles keyed by (row, col).
            Dictionary<(int, int), BubbleGrid> grid = new Dictionary<(int, int), BubbleGrid>();
            foreach (var obj in gameObjects)
            {
                if (obj is BubbleGrid b)
                    grid[(b.Row, b.Col)] = b;
            }

            string color = GetColorFromName(bubble.Name);
            List<(int, int)> cluster = new List<(int, int)>();
            Queue<(int, int)> queue = new Queue<(int, int)>();
            HashSet<(int, int)> visited = new HashSet<(int, int)>();

            var start = (bubble.Row, bubble.Col);
            queue.Enqueue(start);
            visited.Add(start);

            while (queue.Count > 0)
            {
                var cell = queue.Dequeue();
                cluster.Add(cell);
                foreach (var neighbor in GetNeighbors(cell.Item1, cell.Item2, grid))
                {
                    if (!visited.Contains(neighbor) && GetColorFromName(grid[neighbor].Name) == color)
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            // If three or more bubbles (including the shot bubble) form a cluster, remove them.
            if (cluster.Count >= 3)
            {
                foreach (var cell in cluster)
                {
                    if (grid.ContainsKey(cell))
                    {
                        gameObjects.Remove(grid[cell]);
                    }
                }
                // Award score: for example, if 3 bubbles disappear, score = (3-1)*10 = 20.
                Singleton.Instance.Score += (cluster.Count - 1) * 20;
                Singleton.Instance.BubbleLeft -= cluster.Count;

                // Remove any bubbles that are now floating.
                RemoveFloatingBubbles(gameObjects);
            }
        }

        /// <summary>
        /// Rebuilds the grid from the current game objects and removes any bubble that is not connected
        /// to the top row (row 0). For each removed floating bubble, an additional 10 points are awarded.
        /// </summary>
        private static void RemoveFloatingBubbles(List<GameObject> gameObjects)
        {
            // Rebuild the grid dictionary from the current game objects.
            Dictionary<(int, int), BubbleGrid> grid = new Dictionary<(int, int), BubbleGrid>();
            foreach (var obj in gameObjects)
            {
                if (obj is BubbleGrid b)
                {
                    grid[(b.Row, b.Col)] = b;
                }
            }

            // Use breadth-first search starting from all bubbles in the top row.
            HashSet<(int, int)> connected = new HashSet<(int, int)>();
            Queue<(int, int)> queue = new Queue<(int, int)>();
            foreach (var kvp in grid)
            {
                if (kvp.Key.Item1 == 0)
                {
                    queue.Enqueue(kvp.Key);
                    connected.Add(kvp.Key);
                }
            }

            while (queue.Count > 0)
            {
                var cell = queue.Dequeue();
                foreach (var neighbor in GetNeighbors(cell.Item1, cell.Item2, grid))
                {
                    if (!connected.Contains(neighbor))
                    {
                        connected.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            // Remove any bubble not connected to the top and update the score.
            foreach (var kvp in grid.ToList())
            {
                if (!connected.Contains(kvp.Key))
                {
                    gameObjects.Remove(kvp.Value);
                    Singleton.Instance.Score += 20;
                    if (grid.ContainsKey(kvp.Key)) // Check if the bubble is part of the grid
                    {
                        Singleton.Instance.BubbleLeft--;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the valid neighbor cell coordinates for a given (row, col) in the grid.
        /// The neighbors differ for even vs. odd rows.
        /// </summary>
        private static IEnumerable<(int, int)> GetNeighbors(int row, int col, Dictionary<(int, int), BubbleGrid> grid)
        {
            List<(int, int)> neighbors = new List<(int, int)>();
            if (row % 2 == 0)
            {
                neighbors.Add((row, col - 1));
                neighbors.Add((row, col + 1));
                neighbors.Add((row - 1, col - 1));
                neighbors.Add((row - 1, col));
                neighbors.Add((row + 1, col - 1));
                neighbors.Add((row + 1, col));
            }
            else
            {
                neighbors.Add((row, col - 1));
                neighbors.Add((row, col + 1));
                neighbors.Add((row - 1, col));
                neighbors.Add((row - 1, col + 1));
                neighbors.Add((row + 1, col));
                neighbors.Add((row + 1, col + 1));
            }

            foreach (var n in neighbors)
            {
                if (grid.ContainsKey(n))
                    yield return n;
            }
        }

        /// <summary>
        /// Extracts the bubble “color” from the object’s Name.
        /// (For example, if Name is "BubbleGridRed", it returns "Red".)
        /// </summary>
        private static string GetColorFromName(string name)
        {
            if (name.StartsWith("BubbleGrid"))
                return name.Substring("BubbleGrid".Length);
            return "";
        }
    }
}