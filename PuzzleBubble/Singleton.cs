using Microsoft.Xna.Framework.Input;
using System;

namespace PuzzleBubble
{
    class Singleton
    {
        public const int SCREENWIDTH = 1920;
        public const int SCREENHEIGHT = 1080;
        public const int BubbleWidth = 700;
        public const int BubbleHeight = 415;
        public const int GunWIDTH = 300;
        public const int GunHeight = 150;
        public const int SizeBubbleWidth = 70;
        public const int SizeBubbleHeight = 70;
        public int BubbleLeft;
        public int Score;
        public long Timer;
        public long TimeDown;
        public const int PlayWidth = 780;
        public const int PlayHeight = 915;
        public Random Random;
        public enum GameState
        {
            Start,
            GamePlaying,
            GameOver
        }
        public GameState CurrentGameState;
        public int totalRows;
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
