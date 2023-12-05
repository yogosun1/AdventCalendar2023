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

        [TestMethod]
        public void Day4_1()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day4.txt").ToList();
            int score = 0;
            foreach (string input in inputList)
            {
                List<string> gameSplit = input.Split(':').ToList();
                int game = int.Parse(gameSplit[0].Split(' ').Last());
                List<string> numberList = gameSplit[1].Split('|').ToList();
                List<int> winningNumberList = numberList[0].Split(' ').Where(w => !string.IsNullOrWhiteSpace(w)).Select(s => int.Parse(s)).ToList();
                List<int> userNumberList = numberList[1].Split(' ').Where(w => !string.IsNullOrWhiteSpace(w)).Select(s => int.Parse(s)).ToList();
                int gameScore = 0;
                foreach (int win in winningNumberList)
                    if (userNumberList.Contains(win))
                        gameScore = gameScore == 0 ? 1 : gameScore * 2;
                score += gameScore;
            }
        }

        [TestMethod]
        public void Day4_2()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day4.txt").ToList();
            List<Day4Scratchcard> scratchcardList = new List<Day4Scratchcard>();
            foreach (string input in inputList)
            {
                Day4Scratchcard scratchcard = new Day4Scratchcard();
                List<string> gameSplit = input.Split(':').ToList();
                scratchcard.game = int.Parse(gameSplit[0].Split(' ').Last());
                List<string> numberList = gameSplit[1].Split('|').ToList();
                scratchcard.winningNumbers = numberList[0].Split(' ').Where(w => !string.IsNullOrWhiteSpace(w)).Select(s => int.Parse(s)).ToList();
                scratchcard.userNumbers = numberList[1].Split(' ').Where(w => !string.IsNullOrWhiteSpace(w)).Select(s => int.Parse(s)).ToList();
                scratchcard.cardCount = 1;
                scratchcardList.Add(scratchcard);
            }
            foreach (Day4Scratchcard scratchcard in scratchcardList)
            {
                int wins = scratchcard.winningNumbers.Where(w => scratchcard.userNumbers.Contains(w)).Count();
                for (int i = 1; i <= wins; i++)
                    scratchcardList.Where(w => w.game == (scratchcard.game + i)).ToList().ForEach(e => e.cardCount += scratchcard.cardCount);
            }
            int totalCardCount = scratchcardList.Sum(s => s.cardCount);
        }

        private class Day4Scratchcard
        {
            public List<int> winningNumbers { get; set; }
            public List<int> userNumbers { get; set; }
            public int cardCount { get; set; }
            public int game { get; set; }
        }

        [TestMethod]
        public void Day5_1()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day5.txt").ToList();
            List<uint> seedList = inputList.First().Split(' ').Skip(1).Select(uint.Parse).ToList();
            List<Day5Converter> converterList = Day5ParseConverterInput(inputList);
            uint minValue = uint.MaxValue, currentValue;
            foreach (uint seed in seedList)
            {
                currentValue = Day5_1Convert(converterList.Where(w => w.name == "seed-to-soil").ToList(), seed);
                currentValue = Day5_1Convert(converterList.Where(w => w.name == "soil-to-fertilizer").ToList(), currentValue);
                currentValue = Day5_1Convert(converterList.Where(w => w.name == "fertilizer-to-water").ToList(), currentValue);
                currentValue = Day5_1Convert(converterList.Where(w => w.name == "water-to-light").ToList(), currentValue);
                currentValue = Day5_1Convert(converterList.Where(w => w.name == "light-to-temperature").ToList(), currentValue);
                currentValue = Day5_1Convert(converterList.Where(w => w.name == "temperature-to-humidity").ToList(), currentValue);
                currentValue = Day5_1Convert(converterList.Where(w => w.name == "humidity-to-location").ToList(), currentValue);
                if (currentValue < minValue)
                    minValue = currentValue;
            }
            Debug.WriteLine(minValue);
        }

        private uint Day5_1Convert(List<Day5Converter> converterList, uint source)
        {
            Day5Converter convert = converterList.FirstOrDefault(w => w.sourceStart <= source && (w.sourceStart + w.range) > source);
            if (convert == null)
                return source;
            return convert.destinationStart + (source - convert.sourceStart);
        }

        [TestMethod]
        public void Day5_2()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day5.txt").ToList();
            List<uint> inputSeedList = inputList.First().Split(' ').Skip(1).Select(s => uint.Parse(s)).ToList();
            List<Day5Seed> seedRangeList = new List<Day5Seed>();
            for (int i = 0; i < inputSeedList.Count; i += 2)
                seedRangeList.Add(new Day5Seed { seedStart = inputSeedList[i], range = inputSeedList[i + 1] });
            List<Day5Converter> converterList = Day5ParseConverterInput(inputList);
            uint minValue = uint.MaxValue;
            foreach (Day5Seed seed in seedRangeList)
            {
                for (uint i = seed.seedStart; i < (seed.seedStart + seed.range); i++)
                {
                    Tuple<uint, uint> currentValue = new Tuple<uint, uint>(i, Math.Min(seed.range, seed.seedStart + seed.range - i));
                    currentValue = Day5_2Convert(converterList.Where(w => w.name == "seed-to-soil").ToList(), currentValue);
                    currentValue = Day5_2Convert(converterList.Where(w => w.name == "soil-to-fertilizer").ToList(), currentValue);
                    currentValue = Day5_2Convert(converterList.Where(w => w.name == "fertilizer-to-water").ToList(), currentValue);
                    currentValue = Day5_2Convert(converterList.Where(w => w.name == "water-to-light").ToList(), currentValue);
                    currentValue = Day5_2Convert(converterList.Where(w => w.name == "light-to-temperature").ToList(), currentValue);
                    currentValue = Day5_2Convert(converterList.Where(w => w.name == "temperature-to-humidity").ToList(), currentValue);
                    currentValue = Day5_2Convert(converterList.Where(w => w.name == "humidity-to-location").ToList(), currentValue);
                    if (currentValue.Item1 < minValue)
                        minValue = currentValue.Item1;
                    i += (currentValue.Item2 - 1);
                }
            }
            Debug.WriteLine(minValue);
        }

        private List<Day5Converter> Day5ParseConverterInput(List<string> inputList)
        {
            List<Day5Converter> converterList = new List<Day5Converter>();
            string currentConverter = string.Empty;
            foreach (string input in inputList.Skip(2))
            {
                Day5Converter converter = new Day5Converter();
                if (input.Contains("map"))
                {
                    currentConverter = input.Split(' ')[0];
                    continue;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(input))
                        continue;
                    List<uint> inputSplit = input.Split(' ').Select(s => uint.Parse(s)).ToList();
                    converter.destinationStart = inputSplit[0];
                    converter.sourceStart = inputSplit[1];
                    converter.range = inputSplit[2];
                    converter.name = currentConverter;
                }
                converterList.Add(converter);
            }
            return converterList;
        }

        private Tuple<uint, uint> Day5_2Convert(List<Day5Converter> converterList, Tuple<uint, uint> source)
        {
            Day5Converter convert = converterList.FirstOrDefault(w => w.sourceStart <= source.Item1 && (w.sourceStart + w.range) > source.Item1);
            if (convert != null)
            {
                uint start = convert.destinationStart + (source.Item1 - convert.sourceStart);
                uint range = Math.Min(source.Item2, convert.destinationStart + convert.range - start);
                return new Tuple<uint, uint>(start, range);
            }
            else
            {
                uint? nextStart = converterList.Where(w => w.sourceStart >= source.Item1 + source.Item2).OrderBy(o => o.sourceStart).FirstOrDefault()?.sourceStart;
                return new Tuple<uint, uint>(source.Item1, nextStart != null ? Math.Min(source.Item2, (uint)nextStart - source.Item1) : source.Item2);
            }
        }

        private class Day5Seed
        {
            public uint seedStart { get; set; }
            public uint range { get; set; }
        }

        private class Day5Converter
        {
            public uint destinationStart { get; set; }
            public uint sourceStart { get; set; }
            public uint range { get; set; }
            public string name { get; set; }
        }
    }
}