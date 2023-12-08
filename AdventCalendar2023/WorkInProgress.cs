using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventCalendar2023
{
    [TestClass]
    public class WorkInProgress
    {
        [TestMethod]
        public void Day8_1()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day8.txt").ToList();
            List<char> instructionList = inputList.First().ToCharArray().ToList();
            List<Day8Node> nodeList = new List<Day8Node>();
            foreach (string input in inputList.Skip(2))
            {
                Day8Node node = new Day8Node();
                List<string> inputSplit = input.Split(' ').ToList();
                node.Name = inputSplit[0];
                node.LeftNode = inputSplit[2].Substring(1, 3);
                node.RightNode = inputSplit[3].Substring(0, 3);
                nodeList.Add(node);
            }
            Day8Node currentNode = nodeList.FirstOrDefault(w => w.Name == "AAA");
            int steps = 0, i = 0;
            while (currentNode.Name != "ZZZ")
            {
                i = steps % instructionList.Count();
                char instruction = instructionList[i];
                if (instruction == 'R')
                    currentNode = nodeList.FirstOrDefault(w => w.Name == currentNode.RightNode);
                else
                    currentNode = nodeList.FirstOrDefault(w => w.Name == currentNode.LeftNode);
                steps++;
            }
            Debug.WriteLine(steps);
        }

        [TestMethod]
        public void Day8_2()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day8.txt").ToList();
            List<char> instructionList = inputList.First().ToCharArray().ToList();
            List<Day8Node> nodeList = new List<Day8Node>();
            foreach (string input in inputList.Skip(2))
            {
                Day8Node node = new Day8Node();
                List<string> inputSplit = input.Split(' ').ToList();
                node.Name = inputSplit[0];
                node.LeftNode = inputSplit[2].Substring(1, 3);
                node.RightNode = inputSplit[3].Substring(0, 3);
                nodeList.Add(node);
            }
            List<Day8Node> startNodeList = nodeList.Where(w => w.Name.EndsWith("A")).ToList();
            foreach (Day8Node startNode in startNodeList)
            {
                Day8Node currentNode = startNode;
                int steps = 0, i = 0;
                while (!currentNode.Name.EndsWith("Z"))
                {
                    i = steps % instructionList.Count();
                    char instruction = instructionList[i];
                    if (instruction == 'R')
                        currentNode = nodeList.FirstOrDefault(w => w.Name == currentNode.RightNode);
                    else
                        currentNode = nodeList.FirstOrDefault(w => w.Name == currentNode.LeftNode);
                    steps++;
                }
                startNode.Steps = steps;
            }
            long totalSteps = startNodeList.Select(s => (long)s.Steps).LeastCommonMultiple();
            Debug.WriteLine(totalSteps);
        }

        private class Day8Node
        {
            public string Name { get; set; }
            public string LeftNode { get; set; }
            public string RightNode { get; set; }
            public int Steps { get; set; }
        }

        [TestMethod]
        public void Day9()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day9.txt").ToList();
        }

        [TestMethod]
        public void Day10()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day10.txt").ToList();
        }

        [TestMethod]
        public void Day11()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day11.txt").ToList();
        }

        [TestMethod]
        public void Day12()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day12.txt").ToList();
        }

        [TestMethod]
        public void Day13()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day13.txt").ToList();
        }

        [TestMethod]
        public void Day14()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day14.txt").ToList();
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