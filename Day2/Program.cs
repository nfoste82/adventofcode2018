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
    
            Console.WriteLine("Part 1");
            Part1();
            
            Console.WriteLine("\nPart 2");
            Part2();
        }
    
        private static void Part1()
        {
            var values = StringUtils.StringToStrings(_input, '\n');
    
            int wordsWithDoubles = 0;
            int wordsWithTriples = 0;
    
            foreach (var word in values)
            {
                var letterCounts = new Dictionary<char, int>();
                
                bool doubleFound = false;
                bool tripleFound = false;
    
                foreach (char c in word)
                {
                    letterCounts.TryGetValue(c, out var current); 
                    letterCounts[c] = current + 1;
                }
    
                foreach (var count in letterCounts.Values)
                {
                    if (count == 2 && !doubleFound)
                    {
                        doubleFound = true;
                        ++wordsWithDoubles;
                    }
                    else if (count == 3 && !tripleFound)
                    {
                        tripleFound = true;
                        ++wordsWithTriples;
                    }
                }
            }   
            
            Console.WriteLine($"Double words: {wordsWithDoubles}, Triple words: {wordsWithTriples}. Checksum: {wordsWithDoubles * wordsWithTriples}");
        }
    
        private static void Part2()
        {
            List<string> words = StringUtils.StringToStrings(_input, '\n');
    
            var smallestDiff = int.MaxValue;
            var firstWord = string.Empty;
            var secondWord = string.Empty;
    
            foreach (var word in words)
            {
                foreach (var otherWord in words)
                {
                    // Ignore self
                    if (word == otherWord)
                    {
                        continue;
                    }
                    
                    // For each index of the two words, find count of differences
                    var differences = word.Where((t, i) => t != otherWord[i]).Count();
                    
                    if (differences < smallestDiff)
                    {
                        firstWord = word;
                        secondWord = otherWord;
                        smallestDiff = differences;
                    }
                }
            }
            
            Console.WriteLine($"Closest words: {firstWord} | {secondWord}");
            Console.Write("Matching chars: ");
            for (var i = 0; i < firstWord.Length; ++i)
            {
                if (firstWord[i] == secondWord[i])
                {
                    Console.Write(firstWord[i]);
                }
            }
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