using System.Diagnostics;

namespace AdventCalendar2023
{
    [TestClass]
    public class AdventCalendar2023
    {
        [TestMethod]
        public void Day1_1()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day1.txt").ToList();
            int sum = 0;
            foreach (string line in inputList)
            {
                string first = line.Where(w => char.IsDigit(w)).First().ToString();
                string last = line.Where(w => char.IsDigit(w)).Last().ToString();
                int number = int.Parse(first + last);
                sum += number;
            }
        }

        [TestMethod]
        public void Day1_2()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day1.txt").ToList();
            Dictionary<string, string> numberTranslator = new Dictionary<string, string>
            {
                { "one", "1" },
                { "two", "2" },
                { "three", "3" },
                { "four", "4" },
                { "five", "5" },
                { "six", "6" },
                { "seven", "7" },
                { "eight", "8" },
                { "nine", "9" },
            };
            int sum = 0;
            foreach (string line in inputList)
            {
                string lineParsed = string.Empty;
                for (int i = 0; i < line.Length; i++)
                {
                    if (char.IsDigit(line[i]))
                        lineParsed += line[i].ToString();
                    else
                        foreach (KeyValuePair<string, string> keyValuePair in numberTranslator)
                            if (line.Substring(i).StartsWith(keyValuePair.Key))
                                lineParsed += keyValuePair.Value;
                }
                string first = lineParsed.First().ToString();
                string last = lineParsed.Last().ToString();
                int number = int.Parse(first + last);
                sum += number;
            }
        }

        [TestMethod]
        public void Day2_1()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day2.txt").ToList();
            int maxRed = 12, maxGreen = 13, maxBlue = 14;
            int sumGames = 0;
            foreach (string input in inputList)
            {
                List<string> inputSplitGame = input.Split(':').ToList();
                int game = int.Parse(new String(inputSplitGame[0].Where(w => char.IsDigit(w)).ToArray()));
                bool gameIsPossible = true;
                foreach (string set in inputSplitGame[1].Split(';'))
                {
                    List<string> cubeList = set.Split(',').ToList();
                    string redCountString = new String(cubeList.FirstOrDefault(w => w.Contains("red"))?.Where(w => char.IsDigit(w))?.ToArray());
                    string greenCountString = new String(cubeList.FirstOrDefault(w => w.Contains("green"))?.Where(w => char.IsDigit(w))?.ToArray());
                    string blueCountString = new String(cubeList.FirstOrDefault(w => w.Contains("blue"))?.Where(w => char.IsDigit(w))?.ToArray());
                    int redCount = int.Parse(string.IsNullOrEmpty(redCountString) ? "0" : redCountString);
                    int greenCount = int.Parse(string.IsNullOrEmpty(greenCountString) ? "0" : greenCountString);
                    int blueCount = int.Parse(string.IsNullOrEmpty(blueCountString) ? "0" : blueCountString);
                    if (redCount > maxRed || greenCount > maxGreen || blueCount > maxBlue)
                        gameIsPossible = false;
                }
                if (gameIsPossible)
                    sumGames += game;
            }
        }

        [TestMethod]
        public void Day2_2()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day2.txt").ToList();
            int sumGames = 0;
            foreach (string input in inputList)
            {
                List<string> inputSplitGame = input.Split(':').ToList();
                int game = int.Parse(new String(inputSplitGame[0].Where(w => char.IsDigit(w)).ToArray()));
                int minRed = 0, minGreen = 0, minBlue = 0;
                foreach (string set in inputSplitGame[1].Split(';'))
                {
                    List<string> cubeList = set.Split(',').ToList();
                    string redCountString = new String(cubeList.FirstOrDefault(w => w.Contains("red"))?.Where(w => char.IsDigit(w))?.ToArray());
                    string greenCountString = new String(cubeList.FirstOrDefault(w => w.Contains("green"))?.Where(w => char.IsDigit(w))?.ToArray());
                    string blueCountString = new String(cubeList.FirstOrDefault(w => w.Contains("blue"))?.Where(w => char.IsDigit(w))?.ToArray());
                    minRed = Math.Max(minRed, int.Parse(string.IsNullOrEmpty(redCountString) ? "0" : redCountString));
                    minGreen = Math.Max(minGreen, int.Parse(string.IsNullOrEmpty(greenCountString) ? "0" : greenCountString));
                    minBlue = Math.Max(minBlue, int.Parse(string.IsNullOrEmpty(blueCountString) ? "0" : blueCountString));
                }
                sumGames += (minRed * minGreen * minBlue);
            }
        }

        [TestMethod]
        public void Day3_1()
        {
            List<Day3SchematicValue> schematic = Day3LoadSchematic(File.ReadAllLines(@"Input\Day3.txt").ToList());
            int partSum = 0;
            foreach (Day3SchematicValue part in schematic.Where(w => w.IsNumbers))
                if (schematic.Any(w => w.Y >= (part.Y - 1) && w.Y <= (part.Y + 1) && w.StartX >= (part.StartX - 1) && w.EndX <= (part.EndX + 1) && !char.IsDigit(w.Value[0])))
                    partSum += int.Parse(part.Value);
        }

        [TestMethod]
        public void Day3_2()
        {
            List<Day3SchematicValue> schematic = Day3LoadSchematic(File.ReadAllLines(@"Input\Day3.txt").ToList());
            int gearSum = 0;
            foreach (Day3SchematicValue gear in schematic.Where(w => w.Value == "*"))
            {
                List<Day3SchematicValue> partList = schematic.Where(w => w.Y >= (gear.Y - 1) && w.Y <= (gear.Y + 1)
                    && gear.StartX >= (w.StartX - 1) && gear.EndX <= (w.EndX + 1) && w.IsNumbers).ToList();
                if (partList.Count() == 2)
                    gearSum += partList.Select(s => int.Parse(s.Value)).Aggregate((x, y) => x * y);
            }
        }

        private List<Day3SchematicValue> Day3LoadSchematic(List<string> inputList)
        {
            List<Day3SchematicValue> schematic = new List<Day3SchematicValue>();
            int x, y = 0;
            foreach (string input in inputList)
            {
                x = 0;
                List<char> valueList = new List<char>();
                int? startX = null, endX = null;
                foreach (char c in input)
                {
                    if (!char.IsDigit(c))
                    {
                        if (c != '.')
                            schematic.Add(new Day3SchematicValue { StartX = x, EndX = x, Y = y, Value = c.ToString(), IsNumbers = false });
                        if (valueList.Count() > 0)
                        {
                            schematic.Add(new Day3SchematicValue { StartX = (int)startX, EndX = (int)endX, Y = y, Value = new string(valueList.ToArray()), IsNumbers = true });
                            valueList.Clear();
                            startX = null;
                            endX = null;
                        }
                    }
                    else if (char.IsDigit(c))
                    {
                        if (startX == null)
                            startX = x;
                        endX = x;
                        valueList.Add(c);
                    }
                    x++;
                }
                if (valueList.Count() > 0)
                    schematic.Add(new Day3SchematicValue { StartX = (int)startX, EndX = (int)endX, Y = y, Value = new string(valueList.ToArray()), IsNumbers = true });
                y++;
            }
            return schematic;
        }

        private class Day3SchematicValue
        {
            public int StartX { get; set; }
            public int EndX { get; set; }
            public int Y { get; set; }
            public string Value { get; set; }
            public bool IsNumbers { get; set; }
        }

        private void Day3Print(List<Day3SchematicValue> schematic)
        {
            for (int y = 0; y <= schematic.Max(m => m.Y); y++)
            {
                string line = string.Empty;
                for (int x = 0; x <= schematic.Max(m => m.EndX); x++)
                {
                    Day3SchematicValue value = schematic.FirstOrDefault(w => w.Y == y && x >= w.StartX && x <= w.EndX);
                    if (value == null)
                        line += ".";
                    else
                    {
                        line += value.Value;
                        x = value.EndX;
                    }
                }
                Debug.WriteLine(line);
            }
        }
    }
}