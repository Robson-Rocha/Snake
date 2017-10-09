namespace RobsonROX.Snake
{
    using System;
    using System.Collections.Generic;
    using System.Timers;

    public class Snake
    {
        private struct XY
        {
            public int x { get; set; }
            public int y { get; set; }
        }

        private const string square = "\u2588";
        private int px = 10;
        private int py = 10;
        private int tcy = 25;
        private int tcx = 80;
        private int ax = 15, ay = 15;
        private int xv = 1, yv = 0;
        private List<XY> trail = new List<XY>();
        private int tail = 5;
        private Random rnd = new Random(DateTime.Now.Millisecond);
        private Timer timer;

        private void write(ConsoleColor color, string text, int x, int y)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(text);
        }

        private void GameTimer(object sender, ElapsedEventArgs e)
        {
            px += xv;
            py += yv;
            if (px < 0)
            {
                px = tcx - 1;
            }
            if (px > tcx - 1)
            {
                px = 0;
            }
            if (py < 0)
            {
                py = tcy - 1;
            }
            if (py > tcy - 1)
            {
                py = 0;
            }

            for (var i = 0; i < trail.Count; i++)
            {
                write(ConsoleColor.Green, square, trail[i].x, trail[i].y);
                if (trail[i].x == px && trail[i].y == py)
                {
                    timer.Stop();
                    tail = 5;
                    for (var x = 0; x < trail.Count; x++)
                    {
                        write(ConsoleColor.Red, "X", trail[x].x, trail[x].y);
                    }
                    System.Threading.Thread.Sleep(1000);
                    timer.Start();
                }
            }
            trail.Add(new XY { x = px, y = py });
            while (trail.Count > tail)
            {
                write(ConsoleColor.Black, " ", trail[0].x, trail[0].y);
                trail.RemoveAt(0);
            }

            if (ax == px && ay == py)
            {
                write(ConsoleColor.Black, " ", ax, ay);
                tail++;
                ax = rnd.Next(1, tcx);
                ay = rnd.Next(1, tcy);
            }
            write(ConsoleColor.Red, square, ax, ay);
        }

        public void Start()
        {
            Console.CursorVisible = false;
            Console.Clear();
            timer = new Timer(1000 / 15);
            timer.Elapsed += GameTimer;
            timer.Start();
            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        if (yv != 1)
                        {
                            xv = 0;
                            yv = -1;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (yv != -1)
                        {
                            xv = 0;
                            yv = 1;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (xv != 1)
                        {
                            xv = -1;
                            yv = 0;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (xv != -1)
                        {
                            xv = 1;
                            yv = 0;
                        }
                        break;
                    case ConsoleKey.Escape:
                        goto End;
                }
            }

        End:
            timer.Stop();
            timer.Elapsed -= GameTimer;
            Console.Clear();
        }
    }
}
