using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AloneInTheDark
{
    public class Program
    {
        //Directions N=0, S=1, E=2, W=3
        private static int[,] map;
        private static int width;
        private static int height;
        private static void Main(string[] args)
        {
            System.Console.WriteLine("Welcome to Alone in the Dark!");
            System.Console.WriteLine("This is a simple text-based adventure game.");
            System.Console.WriteLine("You find yourself in a dark room with an exit and a monster.");
            System.Console.WriteLine("You can move one space at a time. Find the exit to win!");
            System.Console.WriteLine("Be careful not to run into the monster, or it's game over!");
            System.Console.WriteLine("Enter the width and height of the grid (e.g., 5 5, minimum of 4 4):");
            string[] dimensions = System.Console.ReadLine().Split(' ');
            while (dimensions.Length != 2 || !int.TryParse(dimensions[0], out _) || !int.TryParse(dimensions[1], out _))
            {
                System.Console.WriteLine("Invalid input. Please enter two integers separated by a space:");
                dimensions = System.Console.ReadLine().Split(' ');
            }
            width = Math.Max(4, int.Parse(dimensions[0]));
            height = Math.Max(4, int.Parse(dimensions[1]));
            map = new int[width, height];
            System.Console.WriteLine($"Map size set to {width}x{height}.");
            var monsterPos = initializeMosnster();
            var exitPos = initializeExit(monsterPos.Item1, monsterPos.Item2);
            var playerPos = initializePlayer(monsterPos.Item1, monsterPos.Item2, exitPos.Item1, exitPos.Item2);
            System.Console.WriteLine(commands());
            while (true)
            {
                // Display current positions (for debugging purposes, can be removed later)
                // System.Console.WriteLine($"Player: ({playerPos.Item1}, {playerPos.Item2}) | Monster: ({monsterPos.Item1}, {monsterPos.Item2}) | Exit: ({exitPos.Item1}, {exitPos.Item2})");
                if (nearExit(playerPos.Item1, playerPos.Item2, exitPos.Item1, exitPos.Item2))
                {
                    System.Console.WriteLine("You sense that the exit is very close!");
                }
                System.Console.Write("Enter your move: ");
                string input = System.Console.ReadLine().ToUpper();
                if (input == "HELP")
                {
                    System.Console.WriteLine(commands());
                    continue;
                }
                else if (input == "Q")
                {
                    System.Console.WriteLine("Thanks for playing!");
                    break;
                }
                else if (input == "N" || input == "S" || input == "E" || input == "W")
                {
                    int newX = playerPos.Item1;
                    int newY = playerPos.Item2;
                    switch (input)
                    {
                        case "N":
                            newX -= 1;
                            break;
                        case "S":
                            newX += 1;
                            break;
                        case "E":
                            newY += 1;
                            break;
                        case "W":
                            newY -= 1;
                            break;
                    }
                    if (newX < 0 || newX >= width || newY < 0 || newY >= height)
                    {
                        System.Console.WriteLine("You cannot move that direction.");
                        continue;
                    }
                    playerPos = (newX, newY);
                }
                else
                {
                    System.Console.WriteLine("Invalid command. Type 'HELP' for a list of commands.");
                    continue;
                }
                // Move monster
                int monsterDirection = monster(false);
                int monsterX = monsterPos.Item1;
                int monsterY = monsterPos.Item2;
                switch (monsterDirection)
                {
                    case 0: // North
                        if (monsterX > 0) monsterX -= 1;
                        break;
                    case 1: // South
                        if (monsterX < width - 1) monsterX += 1;
                        break;
                    case 2: // East
                        if (monsterY < height - 1) monsterY += 1;
                        break;
                    case 3: // West
                        if (monsterY > 0) monsterY -= 1;
                        break;
                }
                monsterPos = (monsterX, monsterY);
                // Check for win/loss conditions
                if (playerPos == exitPos)
                {
                    System.Console.WriteLine("Congratulations! You've found the exit! You win!");
                    break;
                }

            }
        }

        static (int, int) initializeMosnster()
        {
            //place monster on map
            int x = new Random().Next(0, map.GetLength(0) - 1);
            int y = new Random().Next(0, map.GetLength(1) - 1);
            return (x, y);

        }

        static (int, int) initializeExit(int x, int y)
        {
            //place exit on map
            //xx,yy is the returned postion of the exit
            //x and y should be postion of the monster, so the exit is not on the monster
            int xx;
            do
            {
                xx = new Random().Next(0, map.GetLength(0) - 1);
            } while (xx == x);
            int yy;
            do
            {
                yy = new Random().Next(0, map.GetLength(1) - 1);
            } while (yy == y);
            return (xx, yy);
        }

        static (int, int) initializePlayer(int x, int y, int xx, int yy)
        {
            //place player on map
            //x and y should be postion of the monster
            //xx and yy should be postion of the exit
            int xxx;
            do
            {
                xxx = new Random().Next(0, map.GetLength(0) - 1);
            } while (xxx == x || xxx == xx);
            int yyy;
            do
            {
                yyy = new Random().Next(0, map.GetLength(1) - 1);
            } while (yyy == y || yyy == yy);
            return (xxx, yyy);
        }

        static string commands()
        {
            //returns a string of the commands available to the player when asking for HELP
            string ans = "Commands: N (North), S (South), E (East), W (West), Q (Quit)";
            return ans;
        }

        static int monster(bool x)
        {
            //bool x is true if the monster is to move towards the player
            //monster direction
            if (!x)
            {
                int direction;
                do
                {
                    direction = new Random().Next(0, 4);
                } while (!viableMove(direction));
                return direction;
            }
            return 0;

        }

        static bool viableMove(int x)
        {
            //Directions N=0, S=1, E=2, W=3
            switch (x)
            {
                case 0:
                    if (map.GetLength(0) + 1 > width)
                    {
                        return false;
                    }
                    break;
                case 1:
                    if (map.GetLength(0) - 1 < 0)
                    {
                        return false;
                    }
                    break;
                case 2:
                    if (map.GetLength(1) + 1 > height)
                    {
                        return false;
                    }
                    break;
                case 3:
                    if (map.GetLength(1) - 1 < 0)
                    {
                        return false;
                    }
                    break;

            }
            return true;
        }

        static bool nearExit(int x, int y, int xx, int yy)
        {
            //check if player is near exit
            //x and y are the players current position
            //xx and yy are the exit position
            if (y == yy + 1 || y == yy - 1 || x == xx + 1 || x == xx - 1)
            {
                return true;
            }
            return false;
        }

    }
}