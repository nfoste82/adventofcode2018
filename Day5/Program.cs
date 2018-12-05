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
            // Read in your input file (your file location may vary)
            _input = File.ReadAllText("../../../input.txt");
        
            Console.WriteLine($"Part 1: {Part1()}");    
            Console.WriteLine($"Part 2: {Part2()}");
        }
        
        public static string React(string input)
        {
            int i = 0;
            while (i < input.Length - 1)
            {
                // Read two characters, if they react, remove them and go back one index
                var first = input[i].ToString();
                var second = input[i + 1].ToString();
        
                if (first != second && first.Equals(second, StringComparison.InvariantCultureIgnoreCase))
                {
                    input = input.Remove(i, 2);
                    --i;
        
                    i = Math.Max(i, 0);
                }
                else
                {
                    ++i;
                }
            }
        
            return input;
        }
        
        private static int Part1()
        {
            return React(_input).Length;
        }
        
        private static int Part2()
        {
            List<string> charsUsed = _input.Select(c => c.ToString().ToUpperInvariant()).Distinct().ToList();
        
            string shortest = null;
            
            foreach (var charUsed in charsUsed)
            {
                var textWithoutChar = _input.Replace(charUsed, "");
                textWithoutChar = textWithoutChar.Replace(charUsed.ToLowerInvariant(), "");
        
                textWithoutChar = React(textWithoutChar);
        
                if (shortest == null || textWithoutChar.Length < shortest.Length)
                {
                    shortest = textWithoutChar;
                }
            }
            
            return shortest.Length;
        }
        
        private static string _input;
    }
}