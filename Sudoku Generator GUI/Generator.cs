using System;
using Sudoku_Solver;
using System.Collections.Generic;

namespace Sudoku_Generator
{
    class Generator
    {
        private const int GRID_LENGTH = 9;
        public static int[,] board;

        public static int[,] Generate(bool symmetry)
        {
            Random r = new Random();
            List<Solver.Point> untouchedSlots = new List<Solver.Point>();
            board = new int[GRID_LENGTH, GRID_LENGTH];
            int[,] solution;

            Console.WriteLine("\nGenerating...");

            Solver.Setup();
            Solver.RandomSolve(board);                      //finds a random solved (completed) board
            board = (int[,])Solver.solution.Clone();        //copying the solution
            solution = (int[,])Solver.solution.Clone();    //copying the solution again to display later




            //now we want to remove some values

            //populate untouchedSlots with all slots in board

            for (int x = 0; x < GRID_LENGTH; x++)
            {
                for (int y = 0; y < GRID_LENGTH; y++)
                {

                    untouchedSlots.Add(new Solver.Point(x, y));
                }
            }

            /*
             * for each untouched slot, we want to remove a value and see if we still have 1 solution
             *      keep the number removed if we do have only 1 solution
             *      place the number back if we have more than 1 solution
             */
            while (untouchedSlots.Count > 0)
            {
                int rand = r.Next(untouchedSlots.Count);    //pick a random untouched slot
                Solver.Point p = untouchedSlots[rand];
                untouchedSlots.RemoveAt(rand);

                int numSave = board[p.y, p.x];              //save the number stored at that slot

                //remove the number at position
                board[p.y, p.x] = 0;                        //remove the number at that position
                Solver.Solve(board);            //find all solutions

                if (Solver.numOfSols > 1)                    //if there is more than 1 solution, put the number back
                {
                    board[p.y, p.x] = numSave;
                }

            }

            //make it symmetrical
            if (symmetry)
            {
                for (int x = 0; x < GRID_LENGTH; x++)
                {
                    for (int y = 0; y < GRID_LENGTH; y++)
                    {
                        if (board[y, x] != 0)
                        {
                            board[8 - y, 8 - x] = solution[8 - y, 8 - x];
                        }
                    }
                }
            }


            //print puzzle and solution


            Console.WriteLine("\nPUZZLE:");
            PrintGridNoZeroes(board);

            Console.WriteLine("\n\nSOLUTION");
            PrintGrid(solution);

            return board;
        }

        public static void PrintGrid(int[,] board)
        {
            string row;
            Console.WriteLine("|----------|----------|----------|");
            for (int x = 0; x < GRID_LENGTH; x++)
            {
                row = "| ";
                for (int y = 0; y < GRID_LENGTH; y++)
                {
                    row += board[x, y] + "  ";

                    if ((y + 1) % 3 == 0)
                    {
                        row += "| ";
                    }

                }
                Console.WriteLine(row);
                if ((x + 1) % 3 == 0)
                {
                    Console.WriteLine("|----------|----------|----------|");
                }
            }
        }

        public static void PrintGridNoZeroes(int[,] board)
        {
            string row;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("|----------|----------|----------|");
            for (int x = 0; x < GRID_LENGTH; x++)
            {
                row = "| ";
                for (int y = 0; y < GRID_LENGTH; y++)
                {

                    string num = (board[x, y] == 0) ? "•" : board[x, y].ToString();

                    row += num + "  ";

                    if ((y + 1) % 3 == 0)
                    {
                        row += "| ";
                    }

                }
                Console.WriteLine(row);
                if ((x + 1) % 3 == 0)
                {
                    Console.WriteLine("|----------|----------|----------|");
                }
            }
        }

    }



}
