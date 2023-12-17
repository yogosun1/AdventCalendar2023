using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AdventCalendar2023
{
    [TestClass]
    public class WorkInProgress
    {
        [TestMethod]
        public void Day17_1()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day17.txt").ToList();
            List<Day17Position> grid = new List<Day17Position>();
            int y = 0, x = 0;
            foreach (string input in inputList)
            {
                x = 0;
                foreach (int i in input.Select(s => int.Parse(s.ToString())))
                    grid.Add(new Day17Position { Y = y, X = x++, HeatLoss = i });
                y++;
            }
            int minHeatLoss = Day17CalculateMinHeatLoss(grid, 0, 3);
            Debug.WriteLine(minHeatLoss); // 916
        }

        [TestMethod]
        public void Day17_2()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day17.txt").ToList();
            List<Day17Position> grid = new List<Day17Position>();
            int y = 0, x = 0;
            foreach (string input in inputList)
            {
                x = 0;
                foreach (int i in input.Select(s => int.Parse(s.ToString())))
                    grid.Add(new Day17Position { Y = y, X = x++, HeatLoss = i });
                y++;
            }
            int minHeatLoss = Day17CalculateMinHeatLoss(grid, 4, 10);
            Debug.WriteLine(minHeatLoss); // 1067
        }

        private int Day17CalculateMinHeatLoss(List<Day17Position> grid, short minMoveDistance, short maxMoveDistance)
        {
            List<Day17QueueItem> queue = new List<Day17QueueItem>();
            HashSet<string> tested = new HashSet<string>();
            queue.Add(new Day17QueueItem { Pos = grid.First(w => w.X == 0 && w.Y == 0) });
            int maxX = grid.Max(m => m.X);
            int maxY = grid.Max(m => m.Y);
            int loops = 0;
            int minHeatLoss = int.MaxValue;
            while (true)
            {
                loops++;
                if (loops % 1000 == 0)
                    queue.RemoveAll(w => w.CurrentHeatLoss >= minHeatLoss);
                Day17QueueItem currentPos = queue.Where(w => w.CurrentHeatLoss < minHeatLoss).OrderBy(o => o.Priority).FirstOrDefault();
                if (currentPos == null)
                    return minHeatLoss;
                queue.Remove(currentPos);
                if (currentPos.Pos.X == maxX && currentPos.Pos.Y == maxY)
                {
                    if (minHeatLoss > currentPos.CurrentHeatLoss && (currentPos.East + currentPos.West + currentPos.North + currentPos.South) >= minMoveDistance)
                        minHeatLoss = currentPos.CurrentHeatLoss;
                    continue;
                }
                if (currentPos.East < maxMoveDistance && currentPos.West == 0 && ((currentPos.North + currentPos.South) >= minMoveDistance || currentPos.East > 0 || (currentPos.Pos.X + currentPos.Pos.Y) == 0))
                {
                    Day17Position pos = grid.FirstOrDefault(w => w.X == currentPos.Pos.X + 1 && w.Y == currentPos.Pos.Y && currentPos.CurrentHeatLoss + w.HeatLoss < minHeatLoss);
                    if (pos != null)
                    {
                        string key = pos.X + " " + pos.Y + " " + currentPos.East + 1 + " " + 0 + " " + 0 + " " + 0;
                        if (!tested.Contains(key))
                        {
                            Day17QueueItem newItem = new Day17QueueItem { CurrentHeatLoss = currentPos.CurrentHeatLoss + pos.HeatLoss, Priority = maxX + maxY - pos.X - pos.Y + currentPos.CurrentHeatLoss + pos.HeatLoss, Pos = pos, East = currentPos.East + 1, West = 0, North = 0, South = 0 };
                            queue.Add(newItem);
                            tested.Add(key);
                        }
                    }
                }
                if (currentPos.West < maxMoveDistance && currentPos.East == 0 && ((currentPos.North + currentPos.South) >= minMoveDistance || currentPos.West > 0))
                {
                    Day17Position pos = grid.FirstOrDefault(w => w.X == currentPos.Pos.X - 1 && w.Y == currentPos.Pos.Y && currentPos.CurrentHeatLoss + w.HeatLoss < minHeatLoss);
                    if (pos != null)
                    {
                        string key = pos.X + " " + pos.Y + " " + 0 + " " + currentPos.West + 1 + " " + 0 + " " + 0;
                        if (!tested.Contains(key))
                        {
                            Day17QueueItem newItem = new Day17QueueItem { CurrentHeatLoss = currentPos.CurrentHeatLoss + pos.HeatLoss, Priority = maxX + maxY - pos.X - pos.Y + currentPos.CurrentHeatLoss + pos.HeatLoss, Pos = pos, East = 0, West = currentPos.West + 1, North = 0, South = 0 };
                            queue.Add(newItem);
                            tested.Add(key);
                        }
                    }
                }
                if (currentPos.North < maxMoveDistance && currentPos.South == 0 && ((currentPos.East + currentPos.West) >= minMoveDistance || currentPos.North > 0))
                {
                    Day17Position pos = grid.FirstOrDefault(w => w.X == currentPos.Pos.X && w.Y == currentPos.Pos.Y - 1 && currentPos.CurrentHeatLoss + w.HeatLoss < minHeatLoss);
                    if (pos != null)
                    {
                        string key = pos.X + " " + pos.Y + " " + 0 + " " + 0 + " " + currentPos.North + 1 + " " + 0;
                        if (!tested.Contains(key))
                        {
                            Day17QueueItem newItem = new Day17QueueItem { CurrentHeatLoss = currentPos.CurrentHeatLoss + pos.HeatLoss, Priority = maxX + maxY - pos.X - pos.Y + currentPos.CurrentHeatLoss + pos.HeatLoss, Pos = pos, East = 0, West = 0, North = currentPos.North + 1, South = 0 };
                            queue.Add(newItem);
                            tested.Add(key);
                        }
                    }
                }
                if (currentPos.South < maxMoveDistance && currentPos.North == 0 && ((currentPos.East + currentPos.West) >= minMoveDistance || currentPos.South > 0 || (currentPos.Pos.X + currentPos.Pos.Y) == 0))
                {
                    Day17Position pos = grid.FirstOrDefault(w => w.X == currentPos.Pos.X && w.Y == currentPos.Pos.Y + 1 && currentPos.CurrentHeatLoss + w.HeatLoss < minHeatLoss);
                    if (pos != null)
                    {
                        string key = pos.X + " " + pos.Y + " " + 0 + " " + 0 + " " + 0 + " " + currentPos.South + 1;
                        if (!tested.Contains(key))
                        {
                            Day17QueueItem newItem = new Day17QueueItem { CurrentHeatLoss = currentPos.CurrentHeatLoss + pos.HeatLoss, Priority = maxX + maxY - pos.X - pos.Y + currentPos.CurrentHeatLoss + pos.HeatLoss, Pos = pos, East = 0, West = 0, North = 0, South = currentPos.South + 1 };
                            queue.Add(newItem);
                            tested.Add(key);
                        }
                    }
                }
            }
        }

        private class Day17Position
        {
            public int Y { get; set; }
            public int X { get; set; }
            public int HeatLoss { get; set; }
        }

        private class Day17QueueItem
        {
            public Day17Position Pos { get; set; }
            public int East { get; set; }
            public int West { get; set; }
            public int North { get; set; }
            public int South { get; set; }
            public int CurrentHeatLoss { get; set; }
            public int Priority { get; set; }
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