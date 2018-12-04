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
            
            // Sample
            //[1518-05-23 00:03] Guard #1877 begins shift
            //[1518-10-06 00:36] wakes up
            //[1518-09-27 00:37] wakes up
            //[1518-03-22 00:43] falls asleep

            var events = new List<KeyValuePair<DateTime, string>>();

            foreach (var line in lines)
            {
                string dtStr = line.Split('[', ']')[1];
                
                var parts = dtStr.Split(' ');

                var date = parts[0];
                var dateParts = date.Split('-');

                var time = parts[1];
                var timeParts = time.Split(':');
                
                DateTime dateTime = new DateTime(int.Parse(dateParts[0]), 
                    int.Parse(dateParts[1]), 
                    int.Parse(dateParts[2]),
                    int.Parse(timeParts[0]),
                    int.Parse(timeParts[1]),
                    0);
                
                int lastBracket = line.IndexOf(']');
                string details = line.Remove(0, lastBracket + 1);
                
                events.Add(new KeyValuePair<DateTime, string>(dateTime, details));
            }

            events.Sort((a, b) => DateTime.Compare(a.Key, b.Key));

            Dictionary<int, double> minutesSleptPerGuard = new Dictionary<int, double>();
            
            int guardNumber = -1;
            DateTime start = default(DateTime);
            DateTime end = default(DateTime);

            foreach (var guardEvent in events)
            {
                int poundIndex = guardEvent.Value.IndexOf("#");
                
                // New guard
                if (poundIndex != -1)
                {
                    // Find guard #
                    int endGuardNumber = guardEvent.Value.IndexOf(' ', poundIndex);

                    guardNumber = int.Parse(guardEvent.Value.Substring(poundIndex + 1, endGuardNumber - poundIndex));
                }                
                else if (guardEvent.Value.Contains("falls"))
                {
                    start = guardEvent.Key;
                }
                else if (guardEvent.Value.Contains("wakes"))
                {
                    end = guardEvent.Key;

                    double numMinutesSlept = end.Subtract(start).TotalMinutes;

                    if (minutesSleptPerGuard.TryGetValue(guardNumber, out var prevMinutesSlept))
                    {
                        numMinutesSlept += prevMinutesSlept;
                    }

                    minutesSleptPerGuard[guardNumber] = numMinutesSlept;
                }
            }

            int sleepiestGuard = minutesSleptPerGuard.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

            var timesSleptPerMinute = new Dictionary<int, int>();    // Key is minute, value is number of times
            bool correctGuard = false;
            
            // Find each minute this guard was asleep during the midnight hour
            foreach (var guardEvent in events)
            {
                int poundIndex = guardEvent.Value.IndexOf("#");
                
                // New guard
                if (poundIndex != -1)
                {
                    correctGuard = false;
                    
                    // Find guard #
                    int endGuardNumber = guardEvent.Value.IndexOf(' ', poundIndex);

                    var guardNum =
                        int.Parse(guardEvent.Value.Substring(poundIndex + 1, endGuardNumber - poundIndex));
                    
                    // If it's the guard we care about
                    if (guardNum == sleepiestGuard)
                    {
                        correctGuard = true;
                    }

                    continue;
                }

                if (!correctGuard)
                {
                    continue;
                }
                
                if (guardEvent.Value.Contains("falls"))
                {
                    start = guardEvent.Key;
                }
                else if (guardEvent.Value.Contains("wakes"))
                {
                    end = guardEvent.Key;

                    for (int i = start.Minute; i < end.Minute; ++i)
                    {
                        timesSleptPerMinute.TryGetValue(i, out var currentCount); 
                        timesSleptPerMinute[i] = currentCount + 1;
                    }
                }
            }
            
            int sleepiestMinute = timesSleptPerMinute.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            //Console.WriteLine($"Guard: {sleepiestGuard}, minute slept most during: {sleepiestMinute}");

            return sleepiestGuard * sleepiestMinute;
        }
    
        private static int Part2()
        {
            var lines = _input.Split('\n');
            
            var events = new List<KeyValuePair<DateTime, string>>();

            foreach (var line in lines)
            {
                string dtStr = line.Split('[', ']')[1];
                
                var parts = dtStr.Split(' ');

                var date = parts[0];
                var dateParts = date.Split('-');

                var time = parts[1];
                var timeParts = time.Split(':');
                
                DateTime dateTime = new DateTime(int.Parse(dateParts[0]), 
                    int.Parse(dateParts[1]), 
                    int.Parse(dateParts[2]),
                    int.Parse(timeParts[0]),
                    int.Parse(timeParts[1]),
                    0);
                
                int lastBracket = line.IndexOf(']');
                string details = line.Remove(0, lastBracket + 1);
                
                events.Add(new KeyValuePair<DateTime, string>(dateTime, details));
            }

            events.Sort((a, b) => DateTime.Compare(a.Key, b.Key));

            var guardToMinuteToTimes = new Dictionary<int, Dictionary<int, int>>();
            
            int currentGuard = -1;
            DateTime start = default(DateTime);
            DateTime end = default(DateTime);
            
            foreach (var guardEvent in events)
            {
                int poundIndex = guardEvent.Value.IndexOf("#");
                
                // New guard
                if (poundIndex != -1)
                {   
                    // Find guard #
                    int endGuardNumber = guardEvent.Value.IndexOf(' ', poundIndex);

                    currentGuard =
                        int.Parse(guardEvent.Value.Substring(poundIndex + 1, endGuardNumber - poundIndex));

                    if (!guardToMinuteToTimes.ContainsKey(currentGuard))
                    {
                        guardToMinuteToTimes[currentGuard] = new Dictionary<int, int>();
                    }
                }
                else if (guardEvent.Value.Contains("falls"))
                {
                    start = guardEvent.Key;
                }
                else if (guardEvent.Value.Contains("wakes"))
                {
                    end = guardEvent.Key;

                    for (int i = start.Minute; i < end.Minute; ++i)
                    {
                        guardToMinuteToTimes[currentGuard].TryGetValue(i, out var currentCount); 
                        guardToMinuteToTimes[currentGuard][i] = currentCount + 1;
                    }
                }
            }

            int numMinutes = 0;
            int guardID = 0;
            int minuteID = -1;
            foreach (var guardDetails in guardToMinuteToTimes)
            {
                if (!guardDetails.Value.Any())
                {
                    continue;
                }
                
                int minsSlept = guardDetails.Value.Values.Max();
                if (minsSlept > numMinutes)
                {
                    numMinutes = minsSlept;
                    guardID = guardDetails.Key;
                    minuteID = guardDetails.Value.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
                }
            }
            
            //Console.WriteLine($"Guard: {guardID}, Minute slept most during: {minuteID}");
            return guardID * minuteID;
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