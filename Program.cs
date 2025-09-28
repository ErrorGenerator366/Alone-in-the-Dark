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
        private static int rowLength;
        private static int columnLength;
        private static (int, int) playerPos;
        private static (int, int) monsterPos;
        private static (int, int) exitPos;
        private static int monsterColumn;        
        private static int monsterRow;
        private static int monsterDirection;

        private static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Alone in the Dark!");
            Console.WriteLine("This is a simple text-based adventure game.");
            Console.WriteLine("You find yourself in a dark room with an exit and a monster.");
            Console.WriteLine("You can move one space at a time. Find the exit to win!");
            Console.WriteLine("Be careful not to run into the monster, or it's game over!");
            Console.WriteLine("Enter the width and height of the grid (e.g., 5 5, minimum of 4 4):");
            string[] dimensions = Console.ReadLine().Split(' ');
            while (dimensions.Length != 2 || !int.TryParse(dimensions[0], out _) || !int.TryParse(dimensions[1], out _))
            {
                Console.WriteLine("Invalid input. Please enter two integers separated by a space:");
                dimensions = Console.ReadLine().Split(' ');
            }
            //remember, arrays are [row, column] or [y,x], opposite of traditional x,y coordinates
            rowLength = Math.Max(4, int.Parse(dimensions[0]));
            columnLength = Math.Max(4, int.Parse(dimensions[1]));
            map = new int[columnLength, rowLength];
            Console.WriteLine($"Map size set to {rowLength}x{columnLength}.");
            monsterPos = InitializeMonster();
            exitPos = InitializeExit(monsterPos.Item1, monsterPos.Item2);
            playerPos = InitializePlayer(monsterPos.Item1, monsterPos.Item2, exitPos.Item1, exitPos.Item2);
            Console.WriteLine(Commands());
            while (true)
            {
                Console.WriteLine("--------------------------------------------------------------------");
                // Display current positions (for debugging purposes, can be removed later)
                Console.WriteLine($"Player: ({playerPos.Item1}, {playerPos.Item2}) | Monster: ({monsterPos.Item1}, {monsterPos.Item2}) | Exit: ({exitPos.Item1}, {exitPos.Item2})");
                if (NearExit(playerPos.Item1, playerPos.Item2, exitPos.Item1, exitPos.Item2))
                {
                    Console.WriteLine("You sense that the exit is very close!");
                }
                if (NearMonster(playerPos.Item1, playerPos.Item2, monsterPos.Item1, monsterPos.Item2))
                {
                    Console.WriteLine("You smell something foul.");
                }

                //Player move
                Console.Write("Enter your move: ");
                string input = Console.ReadLine().ToUpper();
                if (input == "HELP")
                {
                    Console.WriteLine(Commands());
                    continue;
                }
                else if (input == "Q")
                {
                    Console.WriteLine("Thanks for playing!");
                    break;
                }
                else if (input == "N" || input == "S" || input == "E" || input == "W")
                {
                    //array positions are [row, column] or [y,x]
                    int newY = playerPos.Item1;
                    int newX = playerPos.Item2;
                    switch (input)
                    {
                        case "N":
                            newY -= 1;
                            break;
                        case "S":
                            newY += 1;
                            break;
                        case "E":
                            newX += 1;
                            break;
                        case "W":
                            newX -= 1;
                            break;
                    } 
                    // arrays are [row, column] or [y,x]
                    if (newX < 0 || newX >= rowLength || newY < 0 || newY >= columnLength)
                    {
                        Console.WriteLine("You cannot move that direction.");
                        continue;
                    }
                    playerPos = (newY, newX);
                }
                else
                {
                    Console.WriteLine("Invalid command. Type 'HELP' for a list of commands.");
                    continue;
                }
                // Check for win condition
                if (playerPos == exitPos)
                {
                    Console.WriteLine("Congratulations! You've found the exit! You win!");
                    break;
                }

                // Move monster
                // monsterDirection is 0-3 to deterimine a direction, 4 means monster is on player
                monsterDirection = Monster(AttackPlayer(playerPos.Item1, playerPos.Item2, monsterPos.Item1, monsterPos.Item2));
                // monsterRow/Column used as temporary holders for new position to be updated for the monster
                // arrays are [row, column] or [y,x] in the 'traditional' x,y coordinates
                monsterRow = monsterPos.Item1;
                monsterColumn = monsterPos.Item2;
                // adjust monsterY/X coordinates according to the output of monster method
                switch (monsterDirection)
                {
                    case 0: // North
                        monsterRow -= 1;
                        break;
                    case 1: // South
                        monsterRow += 1;
                        break;
                    case 2: // East
                       monsterColumn += 1;
                        break;
                    case 3: // West
                        monsterColumn -= 1;
                        break;
                    case 4: // on player
                        //for debugging puporses
                        Console.WriteLine("switch mon-dir is case 4");
                        break;
                }
                monsterPos = (monsterRow, monsterColumn);
                if (playerPos == monsterPos)
                {
                    Console.WriteLine("The monster has caught you! Game over.");
                    break;

                }
            }
        }

        static (int, int) InitializeMonster()
        {
            //place monster on map
            int row = new Random().Next(0, map.GetLength(0));
            int column = new Random().Next(0, map.GetLength(1));
            return (row, column);

        }

        static (int, int) InitializeExit(int x, int y)
        {
            //place exit on map
            //xx,yy is the postion of the exit
            //x and y should be postion of the monster, so the exit is not on the monster
            int xx;
            int yy;
            do
            {
                yy = new Random().Next(0, map.GetLength(1));
                xx = new Random().Next(0, map.GetLength(0));
            } while (x == xx && y == yy);
            return (xx, yy);
        }

        static (int, int) InitializePlayer(int x, int y, int xx, int yy)
        {
            //place player on map
            //x and y should be postion of the monster
            //xx and yy should be postion of the exit
            int xxx;
            int yyy;
            do
            {
                xxx = new Random().Next(0, map.GetLength(0));
                yyy = new Random().Next(0, map.GetLength(1));
            } while (xxx == x && xxx == xx && yyy == y && yyy == yy);
            return (xxx, yyy);
        }

        static string Commands()
        {
            //returns a string of the commands available to the player when asking for HELP
            string ans = "Commands: N (North), S (South), E (East), W (West), Q (Quit), HELP";
            return ans;
        }

        static int Monster(bool x)
        {
            int direction = 4;
            //bool x is true if the monster is to move towards the player
            //monster moves randomly if x is false
            //returns an int representing the direction the monster will move
            //Directions N=0, S=1, E=2, W=3, X=4 (on player)
            if (!x)
            {
                do
                {
                    direction = new Random().Next(0, 4);
                } while (!ViableMove((direction), (monsterPos.Item1, monsterPos.Item2)));
                return direction;
            }
            Console.WriteLine("monster is chasing you");
            int diagY = playerPos.Item1 - monsterPos.Item1;
            int diagX = playerPos.Item2 - monsterPos.Item2;
            //if player is right, move E
            if (playerPos.Item1 == monsterPos.Item1 && playerPos.Item2 > monsterPos.Item2)
            {
                return 2;
            }
            //if player is left, move W
            else if (playerPos.Item1 == monsterPos.Item1 && playerPos.Item2 < monsterPos.Item2)
            {
                return 3;
            }
            //if player is below, move S
            else if (playerPos.Item2 == monsterPos.Item2 && playerPos.Item1 > monsterPos.Item1)
            {
                return 1;
            }
            //if player is above, move N
            else if (playerPos.Item2 == monsterPos.Item2 && playerPos.Item1 < monsterPos.Item1)
            {
                return 0;
            }
            //if player is in a diagonal
            //Directions N=0, S=1, E=2, W=3, S=4, NE=20, NW=30
            else if (Math.Abs(diagX) == 1 && Math.Abs(diagY) == 1)
            {
                if (diagY > 0 && diagX > 0) direction = 2; // E (right-down)
                if (diagY > 0 && diagX < 0) direction = 3; // W (left-down)
                if (diagY < 0 && diagX > 0) direction = 20; // N (up-right)
                if (diagY < 0 && diagX < 0) direction = 30; // N (up-left)
                if(direction == 2 || direction == 3 && !ViableMove(direction, monsterPos))
                {
                    //if can't move left or right towards player, move down
                    return 1;
                }
                if(direction == 20)
                {
                    //if can't move right, move up
                    if (!ViableMove(2, monsterPos)) return 0;
                    else return 2;

                }
                if(direction == 30)
                {
                    //if can't move left, move up
                    if (!ViableMove(3, monsterPos)) return 0;
                    else return 3;
                }
            }
            //assumes monster is on player
            return 4;
            

        }

        static bool ViableMove(int direction, (int, int) pos)
        {
            //Directions N=0, S=1, E=2, W=3
            switch (direction)
            {
                case 0: // North
                    if (pos.Item1 - 1 > 0) return true;
                    break;
                case 1: // South
                    if (pos.Item1 < columnLength) return true;
                    break;
                case 2: // East
                    if (pos.Item2 < rowLength) return true;
                    break;
                case 3: // West
                    if (pos.Item2 - 1 > 0) return true;
                    break;
            }
            return false;
        }

        static bool NearExit(int y, int x, int yy, int xx)
        {
            //check if player is near exit
            //x and y are the players current position
            //xx and yy are the exit position
            int diagY = x - xx;
            int diagX = y - yy;
            if (y == yy && Math.Abs(x-xx) < 2)
            {
                return true;
            }
            else if (x == xx && Math.Abs(y-yy) < 2)
            {
                return true;
            }
            else if (Math.Abs(diagY) <= 1 && Math.Abs(diagX) <= 1)
            {
                return true;
            }
            return false;
        }

        static bool NearMonster(int x, int y, int mx, int my)
        {
            //check if player is near monster within 3 spaces
            //x and y are the players current position
            //mx and my are the monster position
            int diagY = x - mx;
            int diagX = y - my;
            if (x == mx && Math.Abs(y - my) < 3)
            {
                //check if directly above/below withing 3 spaces
                return true;
            }
            else if (y == my && Math.Abs(x - mx) < 3)
            {
                //check if directly left/right withing 3 spaces
                return true;
            }
            else if (Math.Abs(diagY) <= 2 && Math.Abs(diagX) <= 2)
            {
                //check if within 2 diagonal spaces
                return true;
            }
                return false;

        }

        static bool AttackPlayer(int x, int y, int mx, int my)
        {
            //check if monster is near player
            //x and y are the players current position
            //mx and my are the monster position
            if (x == mx && Math.Abs(y-my) <= 2)
            {
                //check if within 2 of the same row
                return true;
            }
            else if (y == my && Math.Abs(x-mx) <= 2)
            {
                //check if within 2 of the same column
                return true;
            }
            else if(Math.Abs(x-mx) <= 1 && Math.Abs(y-my) <= 1)
            {
                //check if within 1 diagonal block
                return true;
            }
                return false;
        }
    }
}