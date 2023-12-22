using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AdventCalendar2023
{
    [TestClass]
    public class WorkInProgressDay21
    {
        [TestMethod]
        public void Day21_1()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day21.txt").ToList();
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
                //Day21Print(grid, currentPositions);
                Debug.WriteLine("i: " + i + " plots: " + currentPositions.Count());
            }
            Debug.WriteLine(currentPositions.Count());
        }

        [TestMethod]
        public void Day21_2()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day21.txt").ToList();
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
            List<Tuple<int, int>> borderReachList = Day21CalculateBorderReach(rocks, startY, startX, maxX, maxY, 3);
            long border1Diff = borderReachList[1].Item2 - borderReachList[0].Item2;
            long border2Diff = borderReachList[2].Item2 - borderReachList[1].Item2;
            long borderDiffDiff = border2Diff - border1Diff;
            long stepDiff = borderReachList[2].Item1 - borderReachList[1].Item1;
            long startValue = borderReachList[0].Item2;
            long startDiff = border1Diff - borderDiffDiff;

            long add = startDiff;
            long plots = startValue;
            for (long steps = (borderReachList[1].Item1+1); steps <= 26501365; steps += stepDiff)
            {
                add += borderDiffDiff;
                plots += add;
                if (steps > 26500000 || steps < 1000)
                    Debug.WriteLine("Steps: " + steps + " plots: " + plots);
            }
            Debug.WriteLine(plots);
            //This solution only works for real data and not the test data.
            //617729401414635 correct
        }

        private List<Tuple<int, int>> Day21CalculateBorderReach(HashSet<Day21Point> rocks, int startY, int startX, int maxX, int maxY, int maxBorders)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            HashSet<Day21Point> currentPositions = new HashSet<Day21Point>();
            currentPositions.Add(new Day21Point { X = startX, Y = startY });
            HashSet<Day21Point> newPositions = new HashSet<Day21Point>();
            int nextBorderNr = 1;
            List<int> gardenPlotList = new List<int>();
            for (int i = 0; i < 700; i++)
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
                newPositions = new HashSet<Day21Point>();
                //Debug.WriteLine("Steps: " + i + " Plots: " + currentPositions.Count());
                if (currentPositions.Contains(new Day21Point { X = (maxX * nextBorderNr) - 1, Y = maxY / 2 }))
                {
                    nextBorderNr++;
                    result.Add(new Tuple<int, int>(i, currentPositions.Count()));
                    if (maxBorders == (nextBorderNr - 1))
                        return result;
                }
            }
            return null;
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

        private void Day21Print2(HashSet<Day21Point> rocks, List<Day21Point> currentPositions)
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

        private struct Day21Point : IEquatable<Day21Point>
        {
            public int X { get; set; }
            public int Y { get; set; }

            public override int GetHashCode()
            {
                return Tuple.Create(X, Y).GetHashCode();
            }

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
    }
}

