using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using GameOfLife;

namespace Prog
{

    class Prog
    {
        static void Main(string[] args)
        {
            Console.Title = "Conway's Game of Life";

            Console.ReadKey();

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Initialisation
            Universe universe = new Universe(40,40);
            universe.Randomize(3, 10);

            bool running = true;
            bool update = true;
            bool singleStep = false;

            // Disable cursor
            Console.CursorVisible = false;

            while (running)
            {
                // Parse key-input
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        // Exit game and program
                        case ConsoleKey.Escape:
                            running = false;
                            break;

                        case ConsoleKey.P:
                            update = !update;
                            Console.CursorVisible = false;
                            break;

                        case ConsoleKey.R:
                            universe.Randomize(1,10);
                            singleStep = true;
                            break;

                        case ConsoleKey.S:
                            singleStep = true;
                            break;
                    }
                }

                if (update || singleStep)
                {
                    // Console.Clear(); // Very slow
                    Console.SetCursorPosition(0, 0);
                    Console.Write(universe.ToString());
                    universe.Propagate();

                    singleStep = false;
                }

                System.Threading.Thread.Sleep(50);
            }
            // End of program
        }
    }
}