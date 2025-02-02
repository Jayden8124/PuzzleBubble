// using var game = new PuzzleBubble.MainScene();
// game.Run();

using System;

namespace PuzzleBubble
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MainScene())
                game.Run();
        }
    }
}
