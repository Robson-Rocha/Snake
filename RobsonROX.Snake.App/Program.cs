using System;

namespace RobsonROX.Snake.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter your player name: ");
            string playerName = Console.ReadLine();
            Snake game = new Snake {  PlayerName = playerName };
            game.Start();
        }
    }
}
