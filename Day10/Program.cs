using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            _input = File.ReadAllLines("../../../input.txt");
            
            List<(Vector2i pos, Vector2i vel)> dots = new List<(Vector2i pos, Vector2i vel)>();
            
            foreach (var line in _input)
            {
                var first = line.IndexOf('<') + 1;
                var last = line.IndexOf('>');
                var positionParts = line.Substring(first, last - first).Replace(" ", "").Split(',');
                var position = new Vector2i(int.Parse(positionParts[0]), int.Parse(positionParts[1]));

                first = line.IndexOf('<', last + 1) + 1;
                last = line.IndexOf('>', last + 1);
                var velocityParts = line.Substring(first, last - first).Replace(" ", "").Split(',');
                var velocity = new Vector2i(int.Parse(velocityParts[0]), int.Parse(velocityParts[1]));

                dots.Add(new ValueTuple<Vector2i, Vector2i>(position, velocity));
            }

            int minX = 0;
            int maxX = 0;
            int minY = 0;
            int maxY = 0;
            FindCorners(dots, out minX, out maxX, out minY, out maxY);
            int smallestDimensions = Math.Abs((maxX - minX) * (maxY - minY));
            
            for (int i = 0; i < 20000; ++i)
            {
                for (int j = 0; j < dots.Count; ++j)
                {
                    var dot = dots[j];
                    var newPos = ApplyVelocity(dot.pos, dot.vel, 1);
                    dots[j] = (new ValueTuple<Vector2i, Vector2i>(newPos, dot.vel));
                }
                
                minX = 0;
                maxX = 0;
                minY = 0;
                maxY = 0;
                FindCorners(dots, out minX, out maxX, out minY, out maxY);

                int dimensions = Math.Abs((maxX - minX) * (maxY - minY));
                if (dimensions < smallestDimensions)
                {
                    smallestDimensions = dimensions;
                    Console.WriteLine("Dimensions total: " + smallestDimensions + " at i: " + i);

                    if (i == 10576)    // This i + 1 is the answer to part 2
                    {
                        PrintPoints(dots, minX - 2, maxX + 2, minY - 2, maxY + 2);
                    }
                }
            }
        }

        private static void FindCorners(List<(Vector2i pos, Vector2i vel)> dots, out int minX, out int maxX, out int minY,
            out int maxY)
        {
            minX = Int32.MaxValue;
            maxX = Int32.MinValue;
            minY = Int32.MaxValue;
            maxY = Int32.MinValue;

            foreach (var dot in dots)
            {
                var pos = dot.pos;
                if (pos.X < minX)
                {
                    minX = pos.X;
                }

                if (pos.X > maxX)
                {
                    maxX = pos.X;
                }
                
                if (pos.Y < minY)
                {
                    minY = pos.Y;
                }

                if (pos.Y > maxY)
                {
                    maxY = pos.Y;
                }
            }
        }

        private static void PrintPoints(List<(Vector2i pos, Vector2i vel)> dots, int minX, int maxX, int minY, int maxY)
        {
            for (int y = minY; y < maxY; ++y)
            {
                for (int x = minX; x < maxX; ++x)
                {
                    bool found = false;
                    foreach (var dot in dots)
                    {
                        if (dot.pos.X == x && dot.pos.Y == y)
                        {
                            Console.Write("X");
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
        }

        private static Vector2i ApplyVelocity(Vector2i position, Vector2i velocity, int seconds)
        {
            return new Vector2i(position.X + velocity.X * seconds, position.Y + velocity.Y * seconds);
        }

        private static string[] _input;
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