using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("../../../input.txt");

            Part1(lines);
            Part2(lines);
        }

        private static void Part1(string[] lines)
        {
            var gridOrig = new List<string>();
            foreach (var line in lines)
            {
                gridOrig.Add(line);
            }

            for (int minutes = 1; minutes <= 10; ++minutes)
            {
                var grid = new List<string>(gridOrig);

                int width = grid[0].Length;
                for (int y = 0; y < grid.Count; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        char charAtPos = grid[y][x];
                        if (charAtPos == '.') // Open acre
                        {
                            if (NumCharSurrounding('|', x, y, grid) >= 3)
                            {
                                gridOrig[y] = gridOrig[y].ReplaceAt(x, '|');
                            }
                        }
                        else if (charAtPos == '|') // Trees
                        {
                            if (NumCharSurrounding('#', x, y, grid) >= 3)
                            {
                                gridOrig[y] = gridOrig[y].ReplaceAt(x, '#');
                            }
                        }
                        else if (charAtPos == '#') // Lumberyard
                        {
                            if (NumCharSurrounding('#', x, y, grid) == 0)
                            {
                                gridOrig[y] = gridOrig[y].ReplaceAt(x, '.');
                                continue;
                            }

                            if (NumCharSurrounding('|', x, y, grid) == 0)
                            {
                                gridOrig[y] = gridOrig[y].ReplaceAt(x, '.');
                            }
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException(nameof(charAtPos));
                        }
                    }
                }
            }
            
            int numLumberyards = 0;
            int numTrees = 0;
            for (int y = 0; y < gridOrig.Count; ++y)
            {
                int width = gridOrig[0].Length;
                for (int x = 0; x < width; ++x)
                {
                    char charAtPos = gridOrig[y][x];
                        
                    if (charAtPos == '|') ++numTrees;
                    if (charAtPos == '#') ++numLumberyards;
                }
            }
                
            Console.WriteLine("Part 1 answer: " + numTrees * numLumberyards);
        }
        
        private static void Part2(string[] lines)
        {
            var gridOrig = new List<string>();
            foreach (var line in lines)
            {
                gridOrig.Add(line);
            }
            
            for (int minutes = 1; minutes < 10200; ++minutes)
            {
                var grid = new List<string>(gridOrig);

                int width = grid[0].Length;
                for (int y = 0; y < grid.Count; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        char charAtPos = grid[y][x];
                        if (charAtPos == '.') // Open acre
                        {
                            if (NumCharSurrounding('|', x, y, grid) >= 3)
                            {
                                gridOrig[y] = gridOrig[y].ReplaceAt(x, '|');
                            }
                        }
                        else if (charAtPos == '|') // Trees
                        {
                            if (NumCharSurrounding('#', x, y, grid) >= 3)
                            {
                                gridOrig[y] = gridOrig[y].ReplaceAt(x, '#');
                            }
                        }
                        else if (charAtPos == '#') // Lumberyard
                        {
                            if (NumCharSurrounding('#', x, y, grid) == 0)
                            {
                                gridOrig[y] = gridOrig[y].ReplaceAt(x, '.');
                                continue;
                            }

                            if (NumCharSurrounding('|', x, y, grid) == 0)
                            {
                                gridOrig[y] = gridOrig[y].ReplaceAt(x, '.');
                            }
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException(nameof(charAtPos));
                        }
                    }
                }

                // Wait until the forest has become stable and then output its data.
                // Inspect the data, look for the period length for repetitions.
                // Then find 1000000000 % period.
                // Then find a minute number printed out by the Console.WriteLine below with the same module as (1000000000 % period length).
                // The resource value for that minute is your answer.
                if (minutes > 10000)
                {   
                    int numLumberyards = 0;
                    int numTrees = 0;
                    for (int y = 0; y < gridOrig.Count; ++y)
                    {
                        for (int x = 0; x < width; ++x)
                        {
                            char charAtPos = gridOrig[y][x];
                        
                            if (charAtPos == '|') ++numTrees;
                            else if (charAtPos == '#') ++numLumberyards;
                        }
                    }
                
                    Console.WriteLine("Minutes: " + minutes + ", Resource value: " + numTrees * numLumberyards);
                }
            }
        }

        private static int NumCharSurrounding(char c, int x, int y, List<string> grid)
        {
            int total = 0;
            
            if (x > 0 && y > 0 && grid[y-1][x-1] == c) ++total;
            if (y > 0 && grid[y-1][x] == c) ++total;
            if ((x < (grid[0].Length - 1)) && y > 0 && grid[y-1][x+1] == c) ++total;

            if (x > 0 && grid[y][x-1] == c) ++total;
            if ((x < (grid[0].Length - 1)) && grid[y][x+1] == c) ++total;
            
            if (x > 0 && (y < grid.Count - 1) && grid[y+1][x-1] == c) ++total;
            if ((y < grid.Count - 1) && grid[y+1][x] == c) ++total;
            if ((x < (grid[0].Length - 1)) && (y < grid.Count - 1) && grid[y+1][x+1] == c) ++total;

            return total;
        }
    }
    
    public static class StringExtensions
    {
        public static string ReplaceAt(this string input, int index, char newChar)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            char[] chars = input.ToCharArray();
            chars[index] = newChar;
            return new string(chars);
        }
    }
}