namespace RobsonROX.Snake
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Timers;

    public class Snake
    {
        private struct XY
        {
            public int x { get; set; }
            public int y { get; set; }
        }

        private const string squareChar = "\u2588";
        private const string spaceChar = " ";
        private const string deadChar = "X";
        private Random rnd = new Random(DateTime.Now.Millisecond);
        private HttpClient httpClient = new HttpClient();
        private Timer timer = new Timer(1000 / 15);
        private List<XY> trail = new List<XY>();

        private XY arena;
        private XY apple;
        private XY head;
        private XY direction;
        private int tail;
        private int score;

        /// <summary>
        /// Gets or sets the Player Name. Used to write the score at the leaderboards.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// Gets or sets the Leaderboards Service Uri. Override to set an alternative or local service.
        /// </summary>
        public string LeaderboardsServiceUri { get; set; }

        public Snake()
        {
            LeaderboardsServiceUri = "http://snakeleaderboards.azurewebsites.net/api/";
        }

        private void Reset()
        {
            head.x = 10;
            head.y = 10;
            arena.y = Console.WindowHeight;
            arena.x = Console.WindowWidth;
            apple.x = rnd.Next(1, arena.x);
            apple.y = rnd.Next(1, arena.y);
            direction.x = 1;
            direction.y = 0;
            trail.Clear();
            tail = 5;
            score = 0;
        }

        private void Write(ConsoleColor color, string text, int x, int y)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(text);
        }

        private void GameTimer(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            head.x += direction.x;
            head.y += direction.y;
            if (head.x < 0)
            {
                head.x = arena.x - 1;
            }
            else if (head.x > arena.x - 1)
            {
                head.x = 0;
            }
            else if (head.y < 0)
            {
                head.y = arena.y - 1;
            }
            else if (head.y > arena.y - 1)
            {
                head.y = 0;
            }

            for (var i = 0; i < trail.Count; i++)
            {
                Write(ConsoleColor.Green, squareChar, trail[i].x, trail[i].y);
                if (trail[i].x == head.x && trail[i].y == head.y)
                {
                    GameOver();
                    return;
                }
            }
            trail.Add(new XY { x = head.x, y = head.y });
            while (trail.Count > tail)
            {
                Write(ConsoleColor.Black, spaceChar, trail[0].x, trail[0].y);
                trail.RemoveAt(0);
            }

            if (apple.x == head.x && apple.y == head.y)
            {
                Write(ConsoleColor.Black, spaceChar, apple.x, apple.y);
                tail++;
                score++;
                apple.x = rnd.Next(1, arena.x);
                apple.y = rnd.Next(1, arena.y);
            }
            Write(ConsoleColor.Red, squareChar, apple.x, apple.y);
            timer.Enabled = true;
        }

        private void GameOver()
        {
            timer.Stop();
            if(!string.IsNullOrWhiteSpace(PlayerName) && !string.IsNullOrWhiteSpace(LeaderboardsServiceUri))
                Task.Run(() => httpClient.PostAsync($"{LeaderboardsServiceUri}Leaderboards/{PlayerName}", new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("score", score.ToString()) })));

            for (var x = 0; x < trail.Count; x++)
            {
                Write(ConsoleColor.Red, deadChar, trail[x].x, trail[x].y);
            }
            Write(ConsoleColor.Cyan, $"Game over! Your score is {score}!", 1, 1);
            Write(ConsoleColor.DarkCyan, $"Press any key to restart, or <ESC> to exit", 1, 2);

            Reset();
        }

        /// <summary>
        /// Start a new game
        /// </summary>
        public void Start()
        {
            Reset();
            Console.CursorVisible = false;
            Console.Clear();
            timer.Elapsed += GameTimer;
            timer.Start();

            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        if (direction.y != 1)
                        {
                            direction.x = 0;
                            direction.y = -1;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (direction.y != -1)
                        {
                            direction.x = 0;
                            direction.y = 1;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (direction.x != 1)
                        {
                            direction.x = -1;
                            direction.y = 0;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (direction.x != -1)
                        {
                            direction.x = 1;
                            direction.y = 0;
                        }
                        break;
                    case ConsoleKey.Escape:
                        goto End;
                }
                if(!timer.Enabled)
                {
                    Console.Clear();
                    timer.Start();
                }
            }

            End:
            timer.Stop();
            timer.Elapsed -= GameTimer;
            Console.Clear();
        }
    }
}
