using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AdventCalendar2023
{
    [TestClass]
    public class WorkInProgress
    {
        [TestMethod]
        public void Day14_1()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day14.txt").ToList();
            List<Day14Position> grid = new List<Day14Position>();
            int y = 0, x = 0;
            foreach (string input in inputList)
            {
                x = 0;
                foreach (char c in input)
                {
                    if (c != '.')
                        grid.Add(new Day14Position { Type = c, X = x, Y = y });
                    x++;
                }
                y++;
            }
            foreach (Day14Position roundRock in grid.Where(w => w.Type == 'O').OrderBy(o => o.Y))
            {
                Day14Position closestObject = grid.Where(w => w.X == roundRock.X && w.Y < roundRock.Y).OrderByDescending(o => o.Y).FirstOrDefault();
                roundRock.Y = closestObject == null ? 0 : closestObject.Y + 1;
            }
            int maxY = grid.Max(m => m.Y) + 1;
            long load = grid.Where(w => w.Type == 'O').Sum(s => maxY - s.Y);
            Debug.WriteLine(load);
        }

        [TestMethod]
        public void Day14_2()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day14Test.txt").ToList();
            List<Day14Position> grid = new List<Day14Position>();
            int y = 0, x = 0;
            foreach (string input in inputList)
            {
                x = 0;
                foreach (char c in input)
                {
                    if (c != '.')
                        grid.Add(new Day14Position { Type = c, X = x, Y = y });
                    x++;
                }
                y++;
            }
            int maxY = grid.Max(m => m.Y);
            int maxX = grid.Max(m => m.Y);
            List<int> pattern = new List<int>();
            for (int i = 1; i <= 1000000000; i++)
            {
                Day14SpinCycle(grid, maxY, maxX);
                //Day14Print(grid);
                pattern.Add(grid.Where(w => w.Type == 'O').Sum(s => maxY - s.Y));
                int length = Day14FindPattern(pattern);
                if (length > -1)
                {
                    break;
                }
                Debug.WriteLine("Cycle: " + i + " Load: " + grid.Where(w => w.Type == 'O').Sum(s => maxY - s.Y));
            }
            long load = grid.Where(w => w.Type == 'O').Sum(s => (maxY +1) - s.Y);
            Debug.WriteLine(load);
        }

        private void Day14SpinCycle(List<Day14Position> grid, int maxY, int maxX)
        {
            foreach (Day14Position roundRock in grid.Where(w => w.Type == 'O').OrderBy(o => o.Y))
            {
                Day14Position closestObject = grid.Where(w => w.X == roundRock.X && w.Y < roundRock.Y).OrderByDescending(o => o.Y).FirstOrDefault();
                roundRock.Y = closestObject == null ? 0 : closestObject.Y + 1;
            }
            foreach (Day14Position roundRock in grid.Where(w => w.Type == 'O').OrderBy(o => o.X))
            {
                Day14Position closestObject = grid.Where(w => w.X < roundRock.X && w.Y == roundRock.Y).OrderByDescending(o => o.X).FirstOrDefault();
                roundRock.X = closestObject == null ? 0 : closestObject.X + 1;
            }
            foreach (Day14Position roundRock in grid.Where(w => w.Type == 'O').OrderByDescending(o => o.Y))
            {
                Day14Position closestObject = grid.Where(w => w.X == roundRock.X && w.Y > roundRock.Y).OrderBy(o => o.Y).FirstOrDefault();
                roundRock.Y = closestObject == null ? maxY : closestObject.Y - 1;
            }
            foreach (Day14Position roundRock in grid.Where(w => w.Type == 'O').OrderByDescending(o => o.X))
            {
                Day14Position closestObject = grid.Where(w => w.X > roundRock.X && w.Y == roundRock.Y).OrderBy(o => o.X).FirstOrDefault();
                roundRock.X = closestObject == null ? maxX : closestObject.X - 1;
            }
        }

        private int Day14FindPattern(List<int> pattern)
        {
            int length = -1;
            return length;
        }

        private void Day14Print(List<Day14Position> grid)
        {

            for (int y = 0; y <= grid.Max(m => m.Y); y++)
            {
                string line = string.Empty;
                for (int x = 0; x <= grid.Max(m => m.X); x++)
                {
                    Day14Position value = grid.FirstOrDefault(w => w.Y == y && w.X == x);
                    if (value == null)
                        line += ".";
                    else
                        line += value.Type;
                }
                Debug.WriteLine(line);
            }
        }

        private class Day14Position
        {
            public int Y { get; set; }
            public int X { get; set; }
            public char Type { get; set; }
        }

        [TestMethod]
        public void Day15()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day15.txt").ToList();
        }

        [TestMethod]
        public void Day16()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day16.txt").ToList();
        }

        [TestMethod]
        public void Day17()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day17.txt").ToList();
        }

        [TestMethod]
        public void Day18()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day18.txt").ToList();
        }

        [TestMethod]
        public void Day19()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day19.txt").ToList();
        }

        [TestMethod]
        public void Day20()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day20.txt").ToList();
        }

        [TestMethod]
        public void Day21()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day21.txt").ToList();
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