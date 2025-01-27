using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
// using Microsoft.Xna.Framework.*;

namespace PuzzleBubble
{
    class Singleton
    {
        public Vector2 Diemensions = new Vector2(1280, 720);
        public int Score = 0;
        public bool Shooting = false;
        public List<Vector2> removeBubble = new List<Vector2>();
        public bool IsFullScreen;
        public string BestTime, BestScore;
        public MouseState MousePrevious, MouseCurrent;

        private static Singleton instance;
        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }
}