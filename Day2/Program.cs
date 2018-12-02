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
                var letters = new Dictionary<char, int>();
                
                bool doubleFound = false;
                bool tripleFound = false;

                foreach (char letter in word)
                {
                    if (letters.ContainsKey(letter))
                    {
                        var current = letters[letter];
                        letters[letter] = ++current;

                        if (current > 3)
                        {
                            break;
                        }
                    }
                    else
                    {
                        letters[letter] = 1;
                    }
                }

                foreach (var kvp in letters)
                {
                    if (doubleFound && tripleFound)
                    {
                        break;
                    }
                    
                    if (kvp.Value == 2)
                    {
                        if (doubleFound)
                        {
                            continue;
                        }

                        doubleFound = true;

                        ++wordsWithDoubles;
                    }
                    else if (kvp.Value == 3)
                    {
                        if (tripleFound)
                        {
                            continue;
                        }

                        tripleFound = true;

                        ++wordsWithTriples;
                    }
                }
            }   
            
            Console.WriteLine($"Double words: {wordsWithDoubles}, Triple words: {wordsWithTriples}. Checksum: {wordsWithDoubles * wordsWithTriples}");
        }

        private static void Part2()
        {
            var words = StringUtils.StringToStrings(_input, '\n');

            var smallestDiff = Int32.MaxValue;
            string firstWord = words[0];
            string secondWord = words[0];

            foreach (var word in words)
            {
                foreach (var otherWord in words)
                {
                    if (word == otherWord)
                    {
                        continue;
                    }
                    
                    int differences = 0;
                    
                    for (int i = 0; i < word.Length; ++i)
                    {
                        if (word[i] != otherWord[i])
                        {
                            differences++;
                        }
                    }

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