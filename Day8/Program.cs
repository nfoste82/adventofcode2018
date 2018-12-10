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

            List<int> nodes = _input.Split(' ').Select(n => int.Parse(n)).ToList();

            var ret = ReadNode(nodes, 0, 0, 0);            
            
            Console.WriteLine($"metadataTotal (Part 1 answer): {ret.metadataTotal}: Part 2 answer: {ret.nodeValue}");
        }

        private static (int index, int metadataTotal, int nodeValue) ReadNode(List<int> nodes, int index, int metadataTotal, int depth)
        {
            int numChildNodes = nodes[index++];
            int numMetadataNodes = nodes[index++];

            int thisNodeValue = 0;

            var childNodeValues = new List<int>(numChildNodes);
            
            // Read child nodes
            for (int i = 0; i < numChildNodes; ++i)
            {
                var ret = ReadNode(nodes, index, metadataTotal, depth + 1);
                index = ret.index;
                metadataTotal = ret.metadataTotal;

                childNodeValues.Add(ret.nodeValue);
            }
            
            // Read metadata
            for (int i = 0; i < numMetadataNodes; ++i)
            {
                if (depth == 4)
                {
                    Console.Write("");
                }
                
                var metadataValue = nodes[index++];
                metadataTotal += metadataValue;
                
                // Determine this node's value (for part 2)
                if (numChildNodes > 0)
                {
                    if (metadataValue > 0 && metadataValue <= childNodeValues.Count)
                    {
                        thisNodeValue += childNodeValues[metadataValue - 1];
                    }
                }
                else
                {
                    thisNodeValue += metadataValue;
                }
            }
            
            return (index, metadataTotal, thisNodeValue);
        }

        private static string _input;
    }
}