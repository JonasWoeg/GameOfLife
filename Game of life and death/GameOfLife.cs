using System;

namespace GameOfLife
{
    class Cell
    {
        /*
         * Modelling a single Cell in Conway's Game of Life
        */

        // ---- Attributes ----
        public static bool alive = true;
        public static bool dead = false;

        public bool state; // Either alive or dead
        private char skin = '●'; // Character for living cell
        private char back = ' '; // Character for dead cell

        // ---- Constructors ----
        public Cell()
        {
            this.state = dead;
        }
        public Cell(char skin, char back)
        {
            this.state = dead;
            this.skin = skin;
            this.back = back;
        }
        public Cell(bool state)
        {
            this.state = state;
        }
        public Cell(bool state, char skin, char back)
        {
            this.state = state;
            this.skin = skin;
            this.back = back;
        }

        // ---- State-changing methods ----

        // Set cell status to "alive"
        public void Wake()
        {
            this.state = alive;
        }

        // Set cell status to "dead"
        public void Kill()
        {
            this.state = dead;
        }

        // Toggle the state of the cell
        public void Toggle()
        {
            this.state = !this.state;
        }

        // Return the cell as string
        public override string ToString()
        {
            if (this.state)
                return this.skin.ToString();
            else
                return this.back.ToString();
        }
    }

    class Universe
    {
        /*
         * A Class implementing Conway's Game of Life
         */

        // ---- Attributes ----
        // 2D-Array of Cells 
        private Cell[,] map;
        private uint width;
        private uint height;

        // ---- Constructor ----
        public Universe(uint width, uint heigth)
        {
            // Constructor
            this.width = width;
            this.height = heigth;
            this.map = new Cell[width, heigth];

            // Initalize universe as empty
            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                {
                    this.map[i, j] = new Cell(false);
                }
            }
        }

        // ---- Methods manipulating map ----
        // Fill the map with random cell-states
        // (enumerator / denominator) is the chance for a cell to be alive
        public void Randomize(uint enumerator, int denominator)
        {
            Random rnd = new Random();

            foreach (Cell c in this.map)
            {
                if (rnd.Next(denominator) < enumerator)
                    c.Wake();
                else
                    c.Kill();
            }
        }

        // apply rules to map
        public void Propagate()
        {   /* 
              Create a map holding the neighborcount for every cell
              Apply game rules to universe according to neighbor count
             */

            uint[,] neighborMap = new uint[this.width, this.height];

            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                { 
                    neighborMap[i, j] = this.Count(i, j);
                }
            }

            // Update map
            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                {
                    // Apply Conway's Rules to all cells
                    switch (neighborMap[i, j])
                    {
                        // Two neighbors and the cell remains in its state
                        case 2:
                            break;

                        // Three neighbors and it will be alive in the next round
                        case 3:
                            this.map[i, j].Wake();
                            break;

                        // Else: Cell dies from loneliness or overpopulation
                        default:
                            this.map[i, j].Kill();
                            break;
                    }
                }
            }
        }


        // Returns number of cells that are alive
        public uint CountAlive()
        {
            uint counter = 0;
            foreach (Cell c in this.map)
            {
                if (c.state == Cell.alive)
                    counter++;
            }
            return counter;
        }


        // Count all neighbor cells to cell map[i, j]
        public uint Count(int i, int j)
        {
            uint counter = 0;

            for (int k = -1; k <= 1; k++)
            {
                for (int l = -1; l <= 1; l++)
                {
                    // Ignore central cell
                    if (k == 0 && l == 0) continue;

                    // Increment counter when surrounding cell is alive
                    int x = Mod((i + k), (int)this.width);
                    int y = Mod((j + l), (int)this.height);

                    if (this.map[x, y].state == Cell.alive)
                        counter++;
                }
            }

            return counter;
        }

        // Print the map of the universe
        override public string ToString()
        {
            string s = "";

            for (int j = 0; j < this.height; j++)
            {
                // Append all cells from one row
                for (int i = 0; i < this.width; i++)
                {
                    s += this.map[i, j].ToString();
                    s += " ";
                }
                s += "\n";
            }
            return s;
        }

        // custom modulo-function
        public static int Mod(int x, int m)
        {
            // Exclude divison by 0
            if (m == 0) return 0;

            return (x % m + m) % m;
        }
    }
}