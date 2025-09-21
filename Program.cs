using System;
System.Console.WriteLine("Welcome to Alone in the Dark!");
System.Console.WriteLine("This is a simple text-based adventure game.");
System.Console.WriteLine("You find yourself in a dark room with an exit and a monster.");
System.Console.WriteLine("You can move one space at a time. Find the exit to win!");
System.Console.WriteLine("Be careful not to run into the monster, or it's game over!");
//use the below line to show commands
//System.Console.WriteLine("Commands: N (North), S (South), E (East), W (West), Q (Quit)");
System.Console.WriteLine("Enter the width and height of the grid (e.g., 5 5, minimum of 4 4):");
string[] dimensions = System.Console.ReadLine().Split(' ');
int width = Math.Max(4, int.Parse(dimensions[0]));
int height = Math.Max(4, int.Parse(dimensions[1]));
int[,] map = new int[width, height];
System.Console.WriteLine($"Map size set to {width}x{height}.");