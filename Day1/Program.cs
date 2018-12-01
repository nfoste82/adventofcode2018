using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCodeDay1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($"Part 1 answer: {Part1()}");
            Console.WriteLine($"Part 2 answer: {Part2()}");
        }

        private static int Part1()
        {
            var lines = _input.Split('\n');
            return lines.Sum(int.Parse);            
        }

        private static int Part2()
        {
            var lines = _input.Split('\n');
            var total = 0;

            var frequenciesFound = new HashSet<int>();

            while (true)
            {
                foreach (var line in lines)
                {
                    var number = int.Parse(line);
                    total += number;

                    if (!frequenciesFound.Add(total))
                    {
                        return total;
                    }
                }
            }
        }

        private static string _input = @"";    // Paste your puzzle input here
    }
}