using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AdventOfCode
{
internal class Program
{
    public static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("../../../input.txt");
        
        Part1(lines);
        Part2(50000000000);
    }
    
    // NOTES:
    // I totally hacked this one because I was trying to go fast and couldn't think
    // of a better way in the given time.
    //
    // Part 1 outputs the correct answer, but also prints a lot of extra information to the console
    // that I used to look for a repeating pattern. Part 2 requires 50000000000 iterations (50 billion)
    // which would've taken years to complete, I couldn't think of a quick optimization to my code so
    // I figured if I could find a pattern I might be able to calculate what that pattern would be worth
    // if its values were shifted by 50 billion.
    //
    // The pattern you'll see hard coded in Part2 was the pattern my input generated from Part1, your
    // input will likely generate something different so Part2 will not generate a correct answer for you
    // unless you find a pattern and do what I did.

    public static void Part1(string[] lines)
    {
        var state = lines[1];
        state = ".........." + state + "...............................................................";    // Add a few bits to the left of the start point

        var rules = new Dictionary<string, bool>();
        for (int i = 3; i < lines.Length; ++i)
        {
            var line = lines[i];

            var rule = line.Substring(0, 5);
            var lastChar = line.Substring(line.Length - 1, 1);
            var isPlant = lastChar == "#"; 
            rules[rule] = isPlant;
        }

        Console.WriteLine(state);
        Console.WriteLine();
        
        for (int gen = 0; gen < 200; ++gen)
        {
            var stateCopy = state;
            var sb = new StringBuilder(state);
            
            for (int i = 0; i < stateCopy.Length - 2; ++i)
            {
                sb[i] = stateCopy[i];

                var fivePots = FivePots(stateCopy, i);

                rules.TryGetValue(fivePots, out var createsPlant);
                
                sb[i] = (createsPlant) ? '#' : '.';
            }
            state = sb.ToString();
            sb.Clear();
            
            Console.WriteLine("Gen: " + gen + " | " + state);
        }

        int total = 0;
        for (int i = 0; i < state.Length; ++i)
        {
            var isPlant = (state[i] == '#');

            if (isPlant)
            {
                total += (i - 10);
            }
        }

        Console.WriteLine("Part 1: " + total);
    }

    private static void Part2(long gens)
    {
        string start = "........................";
        string input =
            "........................##.#..##.#..##.#..##.#..##.#....##.#..##.#..##.#..##.#....##.#....##.#..##.#....##.#..##.#..##.#..##.#..##.#....##.#..##.#..##.#..##.#..##.#....##.#..##.#..##.#..##.#....##.#";
        
        long total = 0;
        for (int i = 0; i < input.Length; ++i)
        {
            var isPlant = (input[i] == '#');

            if (isPlant)
            {
                total += (i + gens - 100);    // 100 is because "input" is the string of my pattern at the 100th generation
            }
        }   
        
        Console.WriteLine("Part 2: " + total);
    }

    private static string FivePots(string state, int index)
    {
        StringBuilder sb = new StringBuilder();
        if (index - 2 < 0)
        {
            sb.Append(".");
        }
        else
        {
            sb.Append(state[index - 2]);
        }
        
        if (index - 1 < 0)
        {
            sb.Append(".");
        }
        else
        {
            sb.Append(state[index - 1]);
        }
        
        sb.Append(state[index]);
        
        if (index + 1 > state.Length - 1)
        {
            sb.Append(".");
        }
        else
        {
            sb.Append(state[index + 1]);
        }
        
        if (index + 2 > state.Length - 1)
        {
            sb.Append(".");
        }
        else
        {
            sb.Append(state[index + 2]);
        }

        return sb.ToString();
    }
}
}