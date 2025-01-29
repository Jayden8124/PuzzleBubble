using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleBubble
{
    class Singleton
    {
        public const int SCREENWIDTH = 1920;
        public const int SCREENHEIGHT = 1080;

        public const int GunWIDTH = 300;
        public const int GunHeight = 150;

        public int BubbleLeft;

        public int Score;
        public int Life;

        public Random Random;

        public enum GameState
        {
            StartNewLife,
            GamePlaying,
            GameOver
        }
        public GameState CurrentGameState;

        public KeyboardState PreviousKey, CurrentKey;

        private static Singleton instance;

        private Singleton() { }

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