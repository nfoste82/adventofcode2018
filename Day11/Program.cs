using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
internal class Program
{
    public static void Main(string[] args)
    {
        var part1Results = Part1(9306, 3);
        
        Console.WriteLine($"Part 1: {part1Results.coord.X},{part1Results.coord.Y}");
        
        Console.WriteLine("Part 2 takes a little while, please wait...");

        int largestValue = -1;
        int optimalGridSize = -1;
        Vector2i optimalCoord = new Vector2i(-1, -1);
        
        for (int i = 3; i < 300; ++i)
        {
            var (total, coord) = Part1(9306, i);

            if (total > largestValue)
            {
                largestValue = total;
                optimalGridSize = i;
                optimalCoord = coord;
            }

            // If the total is zero then the square has passed is usable size
            if (total == 0)
            {
                break;
            }
        }
        
        Console.WriteLine($"Part 2: {optimalCoord.X},{optimalCoord.Y},{optimalGridSize}");
    }

    private static (int gridValue, Vector2i coord) Part1(int serialNumber, int gridSize)
    {
        int largestGridValue = 0;
        int yAtLargest = 0;
        int xAtLargest = 0;
        
        for (int y = 1; y < 300; y++) {
            for (int x = 1; x < 300; x++) {
                int total = 0;

                for (int innerY = 0; innerY < gridSize; ++innerY) {
                    for (int innerX = 0; innerX < gridSize; ++innerX) {
                        total += PowerAtCoordinate(new Vector2i(x + innerX, y + innerY), serialNumber);
                    }
                }

                if (total > largestGridValue)
                {
                    largestGridValue = total;
                    yAtLargest = y;
                    xAtLargest = x;
                }
            }
        }
       
        return (largestGridValue, new Vector2i(xAtLargest, yAtLargest));
    }

    private static int PowerAtCoordinate(Vector2i coord, int serialNumber)
    {
        var rackID = coord.X + 10;
        
        var powerLevel = rackID * coord.Y;
        powerLevel += serialNumber;
        powerLevel *= rackID;
        powerLevel = (powerLevel / 100) % 10;
        return powerLevel - 5;
    }
}

public struct Vector2i
{
    public int X;
    public int Y;

    public Vector2i(int x, int y)
    {
        X = x;
        Y = y;
    }
}
}