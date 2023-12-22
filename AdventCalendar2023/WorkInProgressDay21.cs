using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCalendar2023
{
    [TestClass]
    public class WorkInProgressDay21
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
            int remainingSteps = 26501365 % 12;


            HashSet<Day21Point> currentPositions = new HashSet<Day21Point>();
            currentPositions.Add(new Day21Point { X = startX, Y = startY });
            HashSet<Day21Point> newPositions = new HashSet<Day21Point>();
            List<int> gardenPlotList = new List<int>();
            List<int> diffList = new List<int>();
            List<int> diffDiffList = new List<int>();
            List<int> diffDiffDiffList = new List<int>();
            List<int> edgePlots = new List<int>();
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
                //gardenPlotList.Add(currentPositions.Count());
                //if (gardenPlotList.Count() > 11)
                //    diffList.Add(gardenPlotList.Last() - gardenPlotList.SkipLast(1).First());
                //if (diffList.Count() > 11)
                //    diffDiffList.Add(diffList.Last() - diffList.SkipLast(11).First());
                //if (diffDiffList.Count() > 1)
                //    diffDiffDiffList.Add(diffDiffList.Last() - diffDiffList.TakeLast(2).First());
                newPositions = new HashSet<Day21Point>();
                //Day21Print2(rocks, currentPositions);

                //int length = Day21FindPattern(diffList);
                //if ((i - 5) % (fullData.Item1 - 1) == 0)
                //{
                gardenPlotList.Add(currentPositions.Count());
                Day21Print2(rocks, currentPositions);
                Debug.WriteLine("i: " + i + " plots: " + currentPositions.Count());
                Debug.WriteLine("plotlist: " + string.Join(',', gardenPlotList));
                Debug.WriteLine("Start garden plots: " + currentPositions.Where(w => w.X >= 0 && w.X < maxX && w.Y >= 0 && w.Y < maxY).Count());

                int fullLen = i - 6;
                //int fullLen2 = i;
                if (i >= 27)
                {
                    int fullPlots = 1;
                    int plots = 0;
                    for (int j = 0; j < (fullLen / 11); j++)
                        fullPlots += 4 * j;
                    if (((fullLen / 11) % 2 == 0 && fullLen % 2 == 0) || (fullLen / 11) % 2 == 1 && fullLen % 2 == 1)
                        plots = fullData.Item3 * (fullLen / 11) * (fullLen / 11) + (fullPlots - (fullLen / 11) * (fullLen / 11)) * fullData.Item2;
                    else
                        plots = fullData.Item2 * (fullLen / 11) * (fullLen / 11) + (fullPlots - (fullLen / 11) * (fullLen / 11)) * fullData.Item3;



                    //int top = currentPositions.Where(w => w.X >= 0 && w.X < maxX && w.Y < -(((fullLen / 11) - 1) * 11)).Count();
                    //int topRight1 = ((fullLen2 / 11) - 1) * currentPositions.Where(w => w.X >= 11 && w.X < 11 + maxX && w.Y < -(((fullLen2 / 11) - 2) * 11) && w.Y >= -(((fullLen2 / 11) - 2) * 11 + maxY)).Count();
                    //int topRight2 = (fullLen2 / 11) * currentPositions.Where(w => w.X >= 2 * 11 && w.X < 2 * 11 + maxX && w.Y < -(((fullLen2 / 11) - 2) * 11) && w.Y >= -(((fullLen2 / 11) - 2) * 11 + maxY)).Count();
                    //int topLeft1 = ((fullLen2 / 11) - 1) * currentPositions.Where(w => w.X < 0 && w.X >= -maxX && w.Y < -(((fullLen2 / 11) - 2) * 11) && w.Y >= -(((fullLen2 / 11) - 2) * 11 + maxY)).Count();
                    //int topLeft2 = (fullLen2 / 11) * currentPositions.Where(w => w.X < -11 && w.X >= -(maxX + 11) && w.Y < -(((fullLen2 / 11) - 2) * 11) && w.Y >= -(((fullLen2 / 11) - 2) * 11 + maxY)).Count();
                    //int bottom = currentPositions.Where(w => w.X >= 0 && w.X < maxX && w.Y >= ((fullLen / 11) * 11)).Count();
                    //int bottomRight1 = ((fullLen2 / 11) - 1) * currentPositions.Where(w => w.X >= 11 && w.X < 11 + maxX && w.Y >= (((fullLen2 / 11) - 1) * 11) && w.Y < (((fullLen2 / 11) - 1) * 11 + maxY)).Count();
                    //int bottomRight2 = (fullLen2 / 11) * currentPositions.Where(w => w.X >= 11 * 2 && w.X < 11 * 2 + maxX && w.Y >= (((fullLen2 / 11) - 1) * 11) && w.Y < (((fullLen2 / 11) - 1) * 11 + maxY)).Count();
                    //int bottomLeft1 = ((fullLen2 / 11) - 1) * currentPositions.Where(w => w.X < 0 && w.X >= -maxX && w.Y >= (((fullLen2 / 11) - 1) * 11) && w.Y < (((fullLen2 / 11) - 1) * 11 + maxY)).Count();
                    //int bottomLeft2 = (fullLen2 / 11) * currentPositions.Where(w => w.X < -11 && w.X >= -(maxX + 11) && w.Y >= (((fullLen2 / 11) - 1) * 11) && w.Y < (((fullLen2 / 11) - 1) * 11 + maxY)).Count();
                    //int left = currentPositions.Where(w => w.X < -(((fullLen / 11) - 1) * 11) && w.Y >= 0 && w.Y < maxY).Count();
                    //int right = currentPositions.Where(w => w.X >= (fullLen / 11) * 11 && w.Y >= 0 && w.Y < maxY).Count();

                    edgePlots.Add(currentPositions.Count() - plots);
                    Debug.WriteLine("calc full gardens: " + fullPlots);
                    Debug.WriteLine("calc full gardens plots: " + plots);
                    //Debug.WriteLine("top: " + top + " topRight1: " + topRight1 + " topRight2: " + topRight2 + " topLeft1: " + topLeft1 + " topLeft2: " + topLeft2
                    //    + " bottom: " + bottom + " bottomRight1: " + bottomRight1 + " bottomRight2: " + bottomRight2 + " bottomLeft1: " + bottomLeft1 + " bottomLeft2: " + bottomLeft2
                    //    + " left: " + left + " right: " + right);
                    //Debug.WriteLine("Total calculated plots: " + (plots + top + topRight1 + topRight2 + topLeft1 + topLeft2 + bottom + bottomRight1 + bottomRight2
                    //    + bottomLeft1 + bottomLeft2 + left + right));
                    //Debug.WriteLine("Calculated edge plots: " + (top + topRight1 + topRight2 + topLeft1 + topLeft2 + bottom + bottomRight1 + bottomRight2
                    //    + bottomLeft1 + bottomLeft2 + left + right));
                    Debug.WriteLine("Edge plots: " + (currentPositions.Count() - plots));
                    Debug.WriteLine("Edge plotList: " + string.Join(',', edgePlots));

                    // Edge plotList: 
                    // continue calculating edge plotlist. This should be possible with baseValue + (i%11)*addValue
                    //437,491,533,586,639,696,737,
                    //455,534,573,627,662,737,790,864,930,1009,1060,
                    //630,733,782,851,891,986,1049,1143,1221,1322,1383,
                    //805,932,991,1075,1120,1235,1308,1422,1512,1635,1706,
                    //980,1131


                    //}
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
    }
}
