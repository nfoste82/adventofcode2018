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
            const int numPlayers = 439;
            const int numMarbles = 71307;
            
            Part1(numPlayers, numMarbles);
            Part2(numPlayers, numMarbles * 100);
        }

        private static void Part1(int numPlayers, int numMarbles)
        {
            var marblesLeft = numMarbles;

            var scores = new long[numPlayers];
            var marbles = new List<int>{0, 1};

            var marbleNumber = 1;
            int currentIndex = 1;

            while (marblesLeft > 0)
            {
                --marblesLeft;
                ++marbleNumber;

                if ((marbleNumber % 23) == 0)
                {
                    int playerIndex = marbleNumber % numPlayers;
                    scores[playerIndex] += marbleNumber;
                    
                    currentIndex = MoveIndex(currentIndex, -7, marbles.Count);

                    scores[playerIndex] += marbles[currentIndex];
                    
                    marbles.RemoveAt(currentIndex);
                }
                else
                {
                    currentIndex = MoveIndex(currentIndex, 2, marbles.Count);
                    marbles.Insert(currentIndex, marbleNumber);
                }
            }

            var max = scores.Max();
            Console.WriteLine("Answer: " + max);
        }
        
        private static void Part2(int numPlayers, int numMarbles)
        {
            var scores = new long[numPlayers]; 
            var linkedList = new LinkedList<long>(); 
            var currentNode = linkedList.AddFirst(0);

            for (int marbleNumber = 1; marbleNumber < numMarbles; ++marbleNumber)
            {
                if (marbleNumber % 23 == 0)
                {
                    int playerIndex = marbleNumber % numPlayers;
                    scores[playerIndex] += marbleNumber;

                    // Move back 7 nodes
                    for (var i = 0; i < 7; ++i)
                    {
                        currentNode = currentNode.Previous ?? linkedList.Last;
                    } 
                    
                    scores[playerIndex] += currentNode.Value; 
                    
                    var remove = currentNode; 
                    currentNode = remove.Next; 
                    linkedList.Remove(remove);
                }
                else
                {
                    currentNode = linkedList.AddAfter(currentNode.Next ?? linkedList.First, marbleNumber);
                }
            }

            var answer = scores.Max();
            Console.WriteLine("Answer: " + answer);
        }

        private static int MoveIndex(int index, int moveAmount, int size)
        {
            index += moveAmount;
            if (index > size)
            {
                index -= size;
            }
            else if (index < 0)
            {
                index += size;
            }

            return index;
        }
    }
}