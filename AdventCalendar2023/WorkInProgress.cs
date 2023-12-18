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
        public void Day18()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day18Test.txt").ToList();
            List<Day18Instruction> instructionList = new List<Day18Instruction>();
            foreach (string input in inputList)
            {
                List<string> inputSplit = input.Split(' ').ToList();
                instructionList.Add(new Day18Instruction { Direction = inputSplit.First(), Length = int.Parse(inputSplit[1]), Color = inputSplit[2] });
            }

            List<Day18Position> grid = new List<Day18Position>();
            int x = 0, y = 0, index = 0;
            foreach (Day18Instruction instruction in instructionList)
            {
                if (instruction.Direction == "R")
                    for (int i = 1; i <= instruction.Length; i++)
                        grid.Add(new Day18Position { X = ++x, Y = y, IsEdge = true, Index = index++ });
                else if (instruction.Direction == "L")
                    for (int i = 1; i <= instruction.Length; i++)
                        grid.Add(new Day18Position { X = --x, Y = y, IsEdge = true, Index = index++ });
                else if (instruction.Direction == "U")
                    for (int i = 1; i <= instruction.Length; i++)
                        grid.Add(new Day18Position { X = x, Y = --y, IsEdge = true, Index = index++ });
                else if (instruction.Direction == "D")
                    for (int i = 1; i <= instruction.Length; i++)
                        grid.Add(new Day18Position { X = x, Y = ++y, IsEdge = true, Index = index++ });
            }
            Day18FillPool(grid);
            Debug.WriteLine(grid.Count());
        }

        private void Day18FillPool(List<Day18Position> grid)
        {
            Day18Position edge1 = grid.First(w => w.X == 0 && w.Y == 0);
            Day18Position edge2 = null;
            Day18Position fill = null;
            string direction = "east";
            int oldX, oldY;
            while (edge1 != null)
            {
                if (direction == "east")
                {
                    edge2 = grid.Where(w => w.X > edge1.X && w.Y == edge1.Y && w.IsEdge).OrderBy(o => o.X).FirstOrDefault();
                    for (int i = edge1.X; i <= edge2.X; i++)
                        grid.Add(new Day18Position { X = i, Y = edge1.Y, IsEdge = false });
                }
                else if (direction == "west")
                {
                    edge2 = grid.Where(w => w.X < edge1.X && w.Y == edge1.Y && w.IsEdge).OrderByDescending(o => o.X).FirstOrDefault();
                    for (int i = edge1.X; i <= edge2.X; i++)
                        grid.Add(new Day18Position { X = i, Y = edge1.Y, IsEdge = false });
                }
                else if (direction == "north")
                {
                    edge2 = grid.Where(w => w.X == edge1.X && w.Y < edge1.Y && w.IsEdge).OrderByDescending(o => o.Y).FirstOrDefault();
                    for (int i = edge1.X; i <= edge2.X; i++)
                        grid.Add(new Day18Position { X = i, Y = edge1.Y, IsEdge = false });
                }
                else if (direction == "south")
                {
                    edge2 = grid.Where(w => w.X == edge1.X && w.Y > edge1.Y && w.IsEdge).OrderBy(o => o.Y).FirstOrDefault();
                    for (int i = edge1.X; i <= edge2.X; i++)
                        grid.Add(new Day18Position { X = i, Y = edge1.Y, IsEdge = false });
                }
                oldX = edge1.X;
                oldY = edge1.Y;
                edge1 = grid.FirstOrDefault(w => w.Index == edge1.Index + 1);
                if (edge1 != null)
                {
                    //string newDirection = edge1.X > oldX ? "east" : edge1.X < oldX ? "west" : edge1.Y > oldY ? "north" : "south";
                    //else if (direction == "east")
                    //{
                    //    direction = newDirection == "south" ? "east" : newDirection == "north" ? "east" : newDirection == "west" ? 
                    //}
                    //else if (direction == "east")
                    //{
                    //    direction = "east";
                    //}
                }
            }
        }

        private void Day18PrintGrid(List<Day18Position> grid)
        {
            for (int y = 0; y <= grid.Max(m => m.Y); y++)
            {
                string line = string.Empty;
                for (int x = 0; x <= grid.Max(m => m.X); x++)
                {
                    Day18Position pos = grid.FirstOrDefault(w => w.X == x && w.Y == y);
                    line += pos != null ? "#" : ".";
                }
                Debug.WriteLine(line);
            }
        }

        private class Day18Instruction
        {
            public string Direction { get; set; }
            public int Length { get; set; }
            public string Color { get; set; }
        }

        private class Day18Position
        {
            public int Y { get; set; }
            public int X { get; set; }
            public int Index { get; set; }
            public bool IsEdge { get; set; }
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