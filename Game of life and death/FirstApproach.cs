using System;

namespace GOLAD
{

    class Program
    {
        static void Main(string[] args)
        {

            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            bool[,] feld = new bool[Console.WindowWidth, Console.WindowHeight];
            Random rnd = new Random();

            // Randomize
            for (uint k = 0; k < feld.GetLength(1); k++)
            {
                for (uint l = 0; l < feld.GetLength(0); l++)
                {
                    int gfrast = rnd.Next(0, 2);
                    switch (gfrast)
                    {
                        case 0:
                            feld[l, k] = false;
                            break;
                        case 1:
                            feld[l, k] = true;
                            break;
                    }
                }
            }

            bool spiel = true;
            Console.Title = "Game of life and death";
            string zellZeichen = "⚫";
            string totZeichen = " ";

            Console.CursorVisible = false;

            while (spiel)
            {
                // Pausieren 
                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey(true).Key == ConsoleKey.P)
                    {
                        spiel = false;
                    }
                }
                //System.Threading.Thread.Sleep(200);
                Console.SetCursorPosition(0, 0);
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Console.Write(NewGen(feld, zellZeichen, totZeichen));
                Console.SetWindowPosition(0, 0);
            }
        }

        private static string NewGen(bool[,] feld, string zellZeichen, string totZeichen) // Generation berechnen und String zurückgeben
        {
            string frame = "";
            bool[,] copy = feld.Clone() as bool[,];

            for (int i = 0; i < feld.GetLength(1); i++)
            {
                for (int j = 0; j < feld.GetLength(0); j++)
                {
                    byte umgebend = 0;
                    int[,] surrounding = umgebung(j, i, Console.WindowWidth - 1, Console.WindowHeight - 1);

                    for (int e = 0; e < surrounding.GetLength(0); e++) // Umgebungsfelder durchzählen, ausgabe umgebend
                    {
                        if (copy[surrounding[e, 0], surrounding[e, 1]]) umgebend++;
                    }

                    if (umgebend < 2 || umgebend > 3)// Sicher Tod wenn Überbevölkerung oder Einsamkeit
                    {
                        feld[j, i] = false;
                        frame += totZeichen;
                    }

                    else if (umgebend == 3)// Neue Zelle Wenn genau 3 Umgebungszellen vorhanden sind
                    {
                        feld[j, i] = true;
                        frame += zellZeichen;
                    }
                    else // bei 2: Zustand bleibt gleich
                    {
                        switch (copy[j, i])
                        {
                            case true:
                                frame += zellZeichen;
                                break;
                            case false:
                                frame += totZeichen;
                                break;
                        }
                    }
                }
                frame += "\n";
            }
            return frame;
        }

        public static int[,] umgebung(int x, int y, int w, int h) // Alle 8 Ungebungsfelder einer Zelle zurückgeben (Ifs der Ecken auslagern) 
        {
            int[,] werte = new int[8, 2];
            int e = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue; // Mittelfeld auslassen

                    // Exception Ecke links oben:
                    else if (x + j < 0 && y + i < 0)
                    {
                        werte[e, 0] = w;
                        werte[e, 1] = h;
                    }
                    // Exception Ecke rechts oben:
                    else if (x + j > w && y + i < 0)
                    {
                        werte[e, 0] = 0;
                        werte[e, 1] = h;
                    }
                    // Exception Ecke rechts unten:
                    else if (x + j > w && y + i > h)
                    {
                        werte[e, 0] = 0;
                        werte[e, 1] = 0;
                    }
                    // Exception Ecke links unten:
                    else if (x + j < 0 && y + i > h)
                    {
                        werte[e, 0] = w;
                        werte[e, 1] = 0;
                    }

                    // Exception Seite links:
                    else if (x + j < 0)
                    {
                        werte[e, 0] = w;
                        werte[e, 1] = y + i;
                    }
                    // Exception Seite oben:
                    else if (y + i < 0)
                    {
                        werte[e, 0] = x + j;
                        werte[e, 1] = h;
                    }
                    // Exception Seite rechts:
                    else if (x + j > w)
                    {
                        werte[e, 0] = 0;
                        werte[e, 1] = y + i;
                    }
                    // Exception Seite unten:
                    else if (y + i > h)
                    {
                        werte[e, 0] = x + j;
                        werte[e, 1] = 0;
                    }
                    // Else Normales Umgebungsfeld
                    else
                    {
                        werte[e, 0] = x + j;
                        werte[e, 1] = y + i;
                    }

                    e++;
                }
            }
            return werte;
        }
    }
}

