using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;

namespace AdventCalendar2023
{
    [TestClass]
    public class WorkInProgress
    {
        [TestMethod]
        public void Day21_1()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day21Test.txt").ToList();
            List<Day21Position> grid = new List<Day21Position>();
            int y = 0, x = 0;
            foreach (string input in inputList)
            {
                x = 0;
                foreach (char c in input)
                    grid.Add(new Day21Position { Type = c, X = x++, Y = y });
                y++;
            }
            Day21Position startPos = grid.First(w => w.Type == 'S');
            List<Day21Position> currentPositions = new List<Day21Position>();
            currentPositions.Add(startPos);
            List<Day21Position> newPositions = new List<Day21Position>();
            for (int i = 0; i < (64); i++)
            {
                foreach (Day21Position position in currentPositions)
                {
                    List<Day21Position> neighbours = grid.Where(w => w.Type != '#' && Math.Abs(w.X - position.X) + Math.Abs(w.Y - position.Y) == 1).ToList();
                    foreach (Day21Position neigbhour in neighbours.Where(w => !newPositions.Contains(w)))
                        newPositions.Add(neigbhour);
                }
                currentPositions = newPositions;
                newPositions = new List<Day21Position>();
                Day21Print(grid, currentPositions);
                Debug.WriteLine("i: " + i + " plots: " + currentPositions.Count());
            }
            Debug.WriteLine(currentPositions.Count());
        }

        [TestMethod]
        public void Day21_2()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day21Test.txt").ToList();
            HashSet<Day21Point> rocks = new HashSet<Day21Point>();
            int y = 0, x = 0;
            int startY = 0, startX = 0;
            foreach (string input in inputList)
            {
                x = 0;
                foreach (char c in input)
                {
                    if (c == '#')
                        rocks.Add(new Day21Point { X = x, Y = y });
                    else if (c == 'S')
                    {
                        startY = y;
                        startX = x;
                    }
                    x++;
                }
                y++;
            }
            int maxY = y;
            int maxX = x;
            //u stepsToFull = Day21StepsToFull(rocks, startX, startY, maxX, maxY);

            HashSet<Day21Point> currentPositions = new HashSet<Day21Point>();
            currentPositions.Add(new Day21Point { X = startX, Y = startY });
            HashSet<Day21Point> newPositions = new HashSet<Day21Point>();
            List<int> gardenPlotList = new List<int>();
            List<int> diffList = new List<int>();
            List<int> diffDiffList = new List<int>();
            List<int> diffDiffDiffList = new List<int>();
            for (int i = 0; i < 300; i++)
            {
                foreach (Day21Point position in currentPositions)
                {
                    Day21Point keyNorth = new Day21Point { Y = position.Y - 1, X = position.X };
                    Day21Point keySouth = new Day21Point { Y = position.Y + 1, X = position.X };
                    Day21Point keyWest = new Day21Point { Y = position.Y, X = position.X - 1 };
                    Day21Point keyEast = new Day21Point { Y = position.Y, X = position.X + 1 };
                    Day21Point keyNorthRockCheck = new Day21Point { Y = (position.Y - 1) < 0 ? maxY + (position.Y - 1) % maxY : (position.Y - 1) % maxY, X = position.X < 0 ? maxY + position.X % maxY : position.X % maxY };
                    Day21Point keySouthRockCheck = new Day21Point { Y = (position.Y + 1) < 0 ? maxY + (position.Y + 1) % maxY : (position.Y + 1) % maxY, X = position.X < 0 ? maxY + position.X % maxY : position.X % maxY };
                    Day21Point keyWestRockCheck = new Day21Point { Y = position.Y < 0 ? maxY + position.Y % maxY : position.Y % maxY, X = (position.X - 1) < 0 ? maxY + (position.X - 1) % maxY : (position.X - 1) % maxY };
                    Day21Point keyEastRockCheck = new Day21Point { Y = position.Y < 0 ? maxY + position.Y % maxY : position.Y % maxY, X = (position.X + 1) < 0 ? maxY + (position.X + 1) % maxY : (position.X + 1) % maxY };
                    if (!rocks.Contains(keyNorthRockCheck) && !newPositions.Contains(keyNorth))
                        newPositions.Add(keyNorth);
                    if (!rocks.Contains(keySouthRockCheck) && !newPositions.Contains(keySouth))
                        newPositions.Add(keySouth);
                    if (!rocks.Contains(keyWestRockCheck) && !newPositions.Contains(keyWest))
                        newPositions.Add(keyWest);
                    if (!rocks.Contains(keyEastRockCheck) && !newPositions.Contains(keyEast))
                        newPositions.Add(keyEast);
                }
                currentPositions = newPositions;
                gardenPlotList.Add(currentPositions.Count());
                if (gardenPlotList.Count() > 11)
                    diffList.Add(gardenPlotList.Last() - gardenPlotList.SkipLast(1).First());
                if (diffList.Count() > 11)
                    diffDiffList.Add(diffList.Last() - diffList.SkipLast(11).First());
                //if (diffDiffList.Count() > 1)
                //    diffDiffDiffList.Add(diffDiffList.Last() - diffDiffList.TakeLast(2).First());
                newPositions = new HashSet<Day21Point>();
                //Day21Print2(rocks, currentPositions);

                //int length = Day21FindPattern(diffList);
                //if ((i + 1) % stepsToFull == 0)
                //{
                //    Day21Print2(rocks, currentPositions);
                //    Debug.WriteLine("i: " + i + " plots: " + currentPositions.Count());
                //}
            }
            Debug.WriteLine(currentPositions.Count());
        }

        [TestMethod]
        public void Day21_2_2()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day21Test.txt").ToList();
            HashSet<Day21Point> rocks = new HashSet<Day21Point>();
            int y = 0, x = 0;
            int startY = 0, startX = 0;
            foreach (string input in inputList)
            {
                x = 0;
                foreach (char c in input)
                {
                    if (c == '#')
                        rocks.Add(new Day21Point { X = x, Y = y });
                    else if (c == 'S')
                    {
                        startY = y;
                        startX = x;
                    }
                    x++;
                }
                y++;
            }
            int maxY = y;
            int maxX = x;
            Tuple<int, int, int> fullData = Day21StepsToFull(rocks, startX, startY, maxX, maxY);

            HashSet<Day21Point> currentPositions = new HashSet<Day21Point>();
            currentPositions.Add(new Day21Point { X = startX, Y = startY });
            HashSet<Day21Point> newPositions = new HashSet<Day21Point>();
            List<int> gardenPlotList = new List<int>();
            List<int> diffList = new List<int>();
            List<int> diffDiffList = new List<int>();
            List<int> diffDiffDiffList = new List<int>();
            for (int i = 0; i < 300; i++)
            {
                foreach (Day21Point position in currentPositions)
                {
                    Day21Point keyNorth = new Day21Point { Y = position.Y - 1, X = position.X };
                    Day21Point keySouth = new Day21Point { Y = position.Y + 1, X = position.X };
                    Day21Point keyWest = new Day21Point { Y = position.Y, X = position.X - 1 };
                    Day21Point keyEast = new Day21Point { Y = position.Y, X = position.X + 1 };
                    Day21Point keyNorthRockCheck = new Day21Point { Y = (position.Y - 1) < 0 ? maxY + (position.Y - 1) % maxY : (position.Y - 1) % maxY, X = position.X < 0 ? maxY + position.X % maxY : position.X % maxY };
                    Day21Point keySouthRockCheck = new Day21Point { Y = (position.Y + 1) < 0 ? maxY + (position.Y + 1) % maxY : (position.Y + 1) % maxY, X = position.X < 0 ? maxY + position.X % maxY : position.X % maxY };
                    Day21Point keyWestRockCheck = new Day21Point { Y = position.Y < 0 ? maxY + position.Y % maxY : position.Y % maxY, X = (position.X - 1) < 0 ? maxY + (position.X - 1) % maxY : (position.X - 1) % maxY };
                    Day21Point keyEastRockCheck = new Day21Point { Y = position.Y < 0 ? maxY + position.Y % maxY : position.Y % maxY, X = (position.X + 1) < 0 ? maxY + (position.X + 1) % maxY : (position.X + 1) % maxY };
                    if (!rocks.Contains(keyNorthRockCheck) && !newPositions.Contains(keyNorth))
                        newPositions.Add(keyNorth);
                    if (!rocks.Contains(keySouthRockCheck) && !newPositions.Contains(keySouth))
                        newPositions.Add(keySouth);
                    if (!rocks.Contains(keyWestRockCheck) && !newPositions.Contains(keyWest))
                        newPositions.Add(keyWest);
                    if (!rocks.Contains(keyEastRockCheck) && !newPositions.Contains(keyEast))
                        newPositions.Add(keyEast);
                }
                currentPositions = newPositions;
                gardenPlotList.Add(currentPositions.Count());
                if (gardenPlotList.Count() > 11)
                    diffList.Add(gardenPlotList.Last() - gardenPlotList.SkipLast(1).First());
                if (diffList.Count() > 11)
                    diffDiffList.Add(diffList.Last() - diffList.SkipLast(11).First());
                //if (diffDiffList.Count() > 1)
                //    diffDiffDiffList.Add(diffDiffList.Last() - diffDiffList.TakeLast(2).First());
                newPositions = new HashSet<Day21Point>();
                //Day21Print2(rocks, currentPositions);

                //int length = Day21FindPattern(diffList);
                if ((i + 1) % fullData.Item1 == 0)
                {
                    Day21Print2(rocks, currentPositions);
                    Debug.WriteLine("i: " + i + " plots: " + currentPositions.Count());
                    Debug.WriteLine("Calculated full plots: " +  (((i + 1) / fullData.Item1) * fullData.Item2 + (((i + 1) / fullData.Item1) - 1) * 4 * fullData.Item2));
                }
            }
            Debug.WriteLine(currentPositions.Count());
        }

        private Tuple<int, int, int> Day21StepsToFull(HashSet<Day21Point> rocks, int startX, int startY, int maxX, int maxY)
        {
            HashSet<Day21Point> currentPositions = new HashSet<Day21Point>();
            currentPositions.Add(new Day21Point { X = startX, Y = startY });
            HashSet<Day21Point> newPositions = new HashSet<Day21Point>();
            List<int> stepPlotList = new List<int>();
            for (int i = 0; i < 300; i++)
            {
                foreach (Day21Point position in currentPositions)
                {
                    Day21Point keyNorth = new Day21Point { Y = position.Y - 1, X = position.X };
                    Day21Point keySouth = new Day21Point { Y = position.Y + 1, X = position.X };
                    Day21Point keyWest = new Day21Point { Y = position.Y, X = position.X - 1 };
                    Day21Point keyEast = new Day21Point { Y = position.Y, X = position.X + 1 };
                    if (!rocks.Contains(keyNorth) && !newPositions.Contains(keyNorth) && keyNorth.Y >= 0)
                        newPositions.Add(keyNorth);
                    if (!rocks.Contains(keySouth) && !newPositions.Contains(keySouth) && keySouth.Y < maxY)
                        newPositions.Add(keySouth);
                    if (!rocks.Contains(keyWest) && !newPositions.Contains(keyWest) && keyWest.X >= 0)
                        newPositions.Add(keyWest);
                    if (!rocks.Contains(keyEast) && !newPositions.Contains(keyEast) && keyEast.X < maxX)
                        newPositions.Add(keyEast);
                }
                currentPositions = newPositions;
                newPositions = new HashSet<Day21Point>();
                stepPlotList.Add(currentPositions.Count());
                Debug.WriteLine(currentPositions.Count());
                if (stepPlotList.Count() >= 3 && stepPlotList.Last() == stepPlotList.SkipLast(2).Last())
                    return new Tuple<int, int, int>(i - 2, stepPlotList.Last(), stepPlotList.SkipLast(1).Last());
            }
            throw new Exception("couldnt find steps");
        }

        private void Day21Print(List<Day21Position> grid, List<Day21Position> currentPositions)
        {
            for (int y = grid.Min(m => m.Y); y <= grid.Max(m => m.Y); y++)
            {
                string line = string.Empty;
                for (int x = grid.Min(m => m.X); x <= grid.Max(m => m.X); x++)
                {
                    Day21Position pos = grid.First(w => w.X == x && w.Y == y);
                    line += currentPositions.Contains(pos) ? "O" : pos.Type;
                }
                Debug.WriteLine(line);
            }
        }

        private void Day21Print2(HashSet<Day21Point> rocks, HashSet<Day21Point> currentPositions)
        {
            int maxY = rocks.Max(m => m.Y) + 2;
            int maxX = rocks.Max(m => m.X) + 2;
            for (int y = currentPositions.Min(m => m.Y); y <= currentPositions.Max(m => m.Y); y++)
            {
                string line = string.Empty;
                for (int x = currentPositions.Min(m => m.X); x <= currentPositions.Max(m => m.X); x++)
                {
                    if (currentPositions?.Contains<Day21Point>(new Day21Point { Y = y, X = x }) ?? false)
                        line += "O";
                    else if (rocks.Contains<Day21Point>(new Day21Point { Y = y < 0 ? maxY + y % maxY : y % maxY, X = x < 0 ? maxY + x % maxY : x % maxY }))
                        line += "#";
                    else
                        line += ".";
                    if ((x + 1) % maxX == 0)
                        line += "|";
                }
                Debug.WriteLine(line);
                if ((y + 1) % maxY == 0)
                    Debug.WriteLine("".PadRight(line.Length, '-'));
            }
        }

        private int Day21FindPattern(List<int> pattern)
        {
            int repeatCount = -1;
            int patternLength = 0;
            for (int i = 0; i < 100; i++)
            {
                repeatCount = -1;
                int length = 10 + i;
                if (length > pattern.Count() / 4)
                    return 0;
                List<int> test = pattern.Skip(pattern.Count() - length).Reverse().ToList();
                List<int> remaining = pattern.Take(pattern.Count() - length).Reverse().ToList();
                for (int p = 0; p < remaining.Count(); p++)
                {
                    if (remaining[p] != test[p % length])
                        break;
                    else if (p % length == 0)
                        repeatCount++;
                }
                if (repeatCount > 2)
                {
                    patternLength = length;
                    break;
                }
            }
            return patternLength;
        }

        public struct Day21Point : IEquatable<Day21Point>
        {
            public int X { get; set; }
            public int Y { get; set; }

            public override int GetHashCode()
            {
                return Tuple.Create(X, Y).GetHashCode();
            }

            //public override bool Equals(object obj)
            //{
            //    return obj is Day21Point && Equals((Day21Point)obj);
            //}

            public bool Equals(Day21Point p)
            {
                return X == p.X && Y == p.Y;
            }

        }

        private class Day21Position
        {
            public int X { get; set; }
            public int Y { get; set; }
            public char Type { get; set; }
        }

        [TestMethod]
        public void Day22()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day22.txt").ToList();
        }

        [TestMethod]
        public void Day23()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day23.txt").ToList();
        }

        [TestMethod]
        public void Day24()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day24.txt").ToList();
        }

        [TestMethod]
        public void Day25()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day25.txt").ToList();
        }
    }
}