using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace AdventOfCode
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            _input = File.ReadAllText("../../../input.txt");
    
            Console.WriteLine($"Part 1: {Part1()}");
            Console.WriteLine($"Part 2: {Part2()}");
        }
    
        private static int Part1()
        {
            var lines = _input.Split('\n');
            
            var grid = new Dictionary<int, Dictionary<int, int>>();
            
            int overlaps = 0;

            foreach (var line in lines)
            {
                var parts = line.Split(' ');

                var coords = parts[2].Remove(parts[2].Length - 1, 1).Split(',');
                var xCoord = int.Parse(coords[0]);
                var yCoord = int.Parse(coords[1]);
                
                var size = parts[3].Split('x');
                var xSize = int.Parse(size[0]);
                var ySize = int.Parse(size[1]);

                for (int x = xCoord; x < xCoord + xSize; ++x)
                {
                    for (int y = yCoord; y < yCoord + ySize; ++y)
                    {
                        if (!grid.TryGetValue(x, out var gridDictY))
                        {
                            gridDictY = new Dictionary<int, int>();
                            grid[x] = gridDictY;
                        }

                        if (!gridDictY.TryGetValue(y, out var gridAtLocation))
                        {
                            gridAtLocation = 0;                            
                        }

                        ++gridAtLocation;
                        gridDictY[y] = gridAtLocation;
                    }
                }
            }
             
            for (int x = 0; x < 1000; ++x)
            {
                for (int y = 0; y < 1000; ++y)
                {
                    if (grid.TryGetValue(x, out var gridDictY))
                    {
                        if (gridDictY.TryGetValue(y, out var gridAtLocation))
                        {
                            if (gridAtLocation > 1)
                            {
                                overlaps++;
                            }
                        }
                    }
                }
            }

            return overlaps;

        }
    
        private static int Part2()
        {
            var lines = _input.Split('\n');
            
            var grid = new Dictionary<int, Dictionary<int, int>>();
            
            int overlaps = 0;

            foreach (var line in lines)
            {
                var parts = line.Split(' ');

                var coords = parts[2].Remove(parts[2].Length - 1, 1).Split(',');
                var xCoord = int.Parse(coords[0]);
                var yCoord = int.Parse(coords[1]);
                
                var size = parts[3].Split('x');
                var xSize = int.Parse(size[0]);
                var ySize = int.Parse(size[1]);

                for (int x = xCoord; x < xCoord + xSize; ++x)
                {
                    for (int y = yCoord; y < yCoord + ySize; ++y)
                    {
                        if (!grid.TryGetValue(x, out var gridDictY))
                        {
                            gridDictY = new Dictionary<int, int>();
                            grid[x] = gridDictY;
                        }

                        if (!gridDictY.TryGetValue(y, out var gridAtLocation))
                        {
                            gridAtLocation = 0;                            
                        }

                        ++gridAtLocation;
                        gridDictY[y] = gridAtLocation;
                    }
                }
            }
            
            // Pass over each claim again, and check if it was overlapped by any other claim
            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                
                var claimID = int.Parse(parts[0].Remove(0, 1));    // Remove #

                var coords = parts[2].Remove(parts[2].Length - 1, 1).Split(',');
                var xCoord = int.Parse(coords[0]);
                var yCoord = int.Parse(coords[1]);
                
                var size = parts[3].Split('x');
                var xSize = int.Parse(size[0]);
                var ySize = int.Parse(size[1]);
                
                bool isCandidate = true;
                
                for (int x = xCoord; x < xCoord + xSize; ++x)
                {
                    for (int y = yCoord; y < yCoord + ySize; ++y)
                    {
                        if (grid.TryGetValue(x, out var gridDictY))
                        {
                            if (gridDictY.TryGetValue(y, out var gridAtLocation))
                            {
                                if (gridAtLocation > 1)
                                {
                                    isCandidate = false;
                                    break;
                                }
                            }
                        }
                    }
                }
                
                if (isCandidate)
                {
                    return claimID;
                }
            }

            return -1;
        }
    
        private static string _input;
    }
    
    public static class StringUtils
    {   
        public static List<int> StringToInts(string input, params char[] separators)
        {
            var tokens = input.Split(separators);
    
            return tokens.Select(t => int.Parse(t)).ToList();
        }
    
        public static List<float> StringToFloats(string input, params char[] separators)
        {
            var tokens = input.Split(separators);
    
            return tokens.Select(t => float.Parse(t)).ToList();
        }
    
        public static List<string> StringToStrings(string input, params char[] separators)
        {
            return input.Split(separators).ToList();
        }
    }
}