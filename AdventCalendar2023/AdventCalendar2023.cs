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

        [TestMethod]
        public void Day6_1()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day6.txt").ToList();
            List<int> timeList = inputList.First().Split(' ').Skip(1).Where(w => !string.IsNullOrWhiteSpace(w)).Select(int.Parse).ToList();
            List<int> distanceList = inputList.Last().Split(' ').Skip(1).Where(w => !string.IsNullOrWhiteSpace(w)).Select(int.Parse).ToList();
            int sumWins = 1;
            for (int i = 0; i < timeList.Count; i++)
            {
                int raceWinCount = 0;
                int time = timeList[i];
                int distance = distanceList[i];
                for (int j = 1; j < time; j++)
                    if (j * (time - j) > distance)
                        raceWinCount++;
                sumWins *= raceWinCount;
            }
            Debug.WriteLine(sumWins);
        }

        [TestMethod]
        public void Day6_2()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day6.txt").ToList();
            Int64 time = Int64.Parse(inputList.First().Replace(" ", "").Split(':').Last());
            Int64 distance = Int64.Parse(inputList.Last().Replace(" ", "").Split(':').Last());
            Int64 raceWinCount = 0;
            for (Int64 j = 1; j < time; j++)
                if (j * (time - j) > distance)
                    raceWinCount++;
            Debug.WriteLine(raceWinCount);
        }

        [TestMethod]
        public void Day7_1()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day7.txt").ToList();
            List<Day7Card> cardList = new List<Day7Card>();
            foreach (string input in inputList)
            {
                List<string> splitInput = input.Split(' ').ToList();
                List<int> intCardList = splitInput[0].ToCharArray().Select(s => s == 'A' ? 14 : s == 'K' ? 13 : s == 'Q' ? 12 : s == 'J' ? 11 : s == 'T' ? 10 : int.Parse(s.ToString())).ToList();
                Day7Card card = new Day7Card { cardList = intCardList, bid = int.Parse(splitInput[1]) };
                var groupByCard = from p in card.cardList group p by p into g select new { Count = g.Count() };
                if (groupByCard.Count() == 1)
                    card.type = 7; // Five of a kind
                else if (groupByCard.Count() == 2)
                {
                    if (groupByCard.Any(a => a.Count == 4))
                        card.type = 6; // Four of a kind
                    else
                        card.type = 5; // Full house
                }
                else if (groupByCard.Count() == 3)
                {
                    if (groupByCard.Any(a => a.Count == 3))
                        card.type = 4; // Three of a kind
                    else
                        card.type = 3; // Two pair
                }
                else if (groupByCard.Count() == 4)
                    card.type = 2; // One pair
                else
                    card.type = 1; // High card
                cardList.Add(card);
            }
            cardList = cardList.OrderBy(o => o.type).ThenBy(t => t.cardList[0]).ThenBy(t => t.cardList[1])
                            .ThenBy(t => t.cardList[2]).ThenBy(t => t.cardList[3]).ThenBy(t => t.cardList[4]).ToList();
            int rank = 1;
            cardList.ForEach(e => e.rank = rank++);
            int sum = cardList.Sum(s => (s.rank * s.bid));
            Debug.WriteLine(sum);
        }

        [TestMethod]
        public void Day7_2()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day7.txt").ToList();
            List<Day7Card> cardList = new List<Day7Card>();
            foreach (string input in inputList)
            {
                List<string> splitInput = input.Split(' ').ToList();
                List<int> intCardList = splitInput[0].ToCharArray().Select(s => s == 'A' ? 14 : s == 'K' ? 13 : s == 'Q' ? 12 : s == 'J' ? 1 : s == 'T' ? 10 : int.Parse(s.ToString())).ToList();
                Day7Card card = new Day7Card { cardList = intCardList, bid = int.Parse(splitInput[1]) };
                var groupByCard = from p in intCardList group p by p into g select new { Count = g.Count() };
                int jokerCount = intCardList.Where(w => w == 1).Count();
                if (groupByCard.Count() == 1)
                    card.type = 7; // Five of a kind
                else if (groupByCard.Count() == 2)
                {
                    if (jokerCount > 0)
                        card.type = 7; // Five of a kind
                    else if (groupByCard.Any(a => a.Count == 4))
                        card.type = 6; // Four of a kind
                    else
                        card.type = 5; // Full house
                }
                else if (groupByCard.Count() == 3)
                {
                    if (jokerCount > 1)
                        card.type = 6; // Four of a kind
                    else if (jokerCount == 1)
                    {
                        if (groupByCard.Any(a => a.Count == 3))
                            card.type = 6; // Four of a kind
                        else
                            card.type = 5; // Full house
                    }
                    else if (groupByCard.Any(a => a.Count == 3))
                        card.type = 4; // Three of a kind
                    else
                        card.type = 3; // Two pair
                }
                else if (groupByCard.Count() == 4)
                {
                    if (jokerCount > 0)
                        card.type = 4; // Three of a kind
                    else
                        card.type = 2; // One pair
                }
                else
                {
                    if (jokerCount > 0)
                        card.type = 2; // One pair
                    else
                        card.type = 1; // High card
                }
                cardList.Add(card);
            }
            cardList = cardList.OrderBy(o => o.type).ThenBy(t => t.cardList[0]).ThenBy(t => t.cardList[1])
                .ThenBy(t => t.cardList[2]).ThenBy(t => t.cardList[3]).ThenBy(t => t.cardList[4]).ToList();
            int rank = 1;
            cardList.ForEach(e => e.rank = rank++);
            int sum = cardList.Sum(s => (s.rank * s.bid));
            Debug.WriteLine(sum);
        }

        private class Day7Card
        {
            public List<int> cardList { get; set; }
            public int bid { get; set; }
            public int rank { get; set; }
            public int type { get; set; }
        }

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
        public void Day9_1()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day9.txt").ToList();
            long sum = 0;
            inputList.ForEach(e => sum += Day9GetNextValue(e.Split(' ').Select(long.Parse).ToList(), true));
            Debug.WriteLine(sum);
        }

        [TestMethod]
        public void Day9_2()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day9.txt").ToList();
            long sum = 0;
            inputList.ForEach(e => sum += Day9GetNextValue(e.Split(' ').Select(long.Parse).ToList(), false));
            Debug.WriteLine(sum);
        }

        private long Day9GetNextValue(List<long> valueList, bool isForward)
        {
            List<long> nextValueList = new List<long>();
            for (int i = 0; i < (valueList.Count - 1); i++)
                nextValueList.Add(valueList[i + 1] - valueList[i]);
            if (nextValueList.Any(a => a != 0))
                return isForward ? (valueList.Last() + Day9GetNextValue(nextValueList, isForward))
                    : valueList.First() - Day9GetNextValue(nextValueList, isForward);
            else
                return valueList.FirstOrDefault();
        }

        [TestMethod]
        public void Day10_1()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day10.txt").ToList();
            List<Day10Position> grid = new List<Day10Position>();
            int y = 0;
            foreach (string input in inputList)
            {
                int x = 0;
                foreach (char c in input)
                    grid.Add(new Day10Position { X = x++, Y = y, Type = c, LoopPart = false, Visited = false });
                y++;
            }
            Dictionary<string, List<char>> directionList = new Dictionary<string, List<char>>
            {
                { "east", new List<char> { 'J', '7', '-', 'S' } },
                { "west", new List<char> { 'L', 'F', '-', 'S' } },
                { "north", new List<char> { 'F', '7', '|', 'S' } },
                { "south", new List<char> { 'L', 'J', '|', 'S' } }
            };
            Day10Position currentPos = grid.FirstOrDefault(w => w.Type == 'S');
            currentPos.Distance = 0;
            Day10Position east = grid.FirstOrDefault(w => w.X == currentPos.X + 1 && w.Y == currentPos.Y && directionList.FirstOrDefault(w => w.Key == "east").Value.Contains(w.Type));
            Day10Position west = grid.FirstOrDefault(w => w.X == currentPos.X - 1 && w.Y == currentPos.Y && directionList.FirstOrDefault(w => w.Key == "west").Value.Contains(w.Type));
            Day10Position north = grid.FirstOrDefault(w => w.X == currentPos.X && w.Y == currentPos.Y - 1 && directionList.FirstOrDefault(w => w.Key == "north").Value.Contains(w.Type));
            Day10Position south = grid.FirstOrDefault(w => w.X == currentPos.X && w.Y == currentPos.Y + 1 && directionList.FirstOrDefault(w => w.Key == "south").Value.Contains(w.Type));
            if (!Day10IsPartOfLoop(west, grid, "west", directionList))
                if (!Day10IsPartOfLoop(east, grid, "east", directionList))
                    if (!Day10IsPartOfLoop(north, grid, "north", directionList))
                        Day10IsPartOfLoop(south, grid, "south", directionList);
            Debug.WriteLine(Math.Round(((double)grid.Where(w => w.LoopPart).Max(m => m.Distance)) / 2, MidpointRounding.ToPositiveInfinity));
        }

        [TestMethod]
        public void Day10_2()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day10.txt").ToList();
            List<Day10Position> grid = new List<Day10Position>();
            int y = 0;
            foreach (string input in inputList)
            {
                int x = 0;
                foreach (char c in input)
                    grid.Add(new Day10Position { X = x++, Y = y, Type = c, LoopPart = false, Visited = false, Marked = false });
                y++;
            }
            Dictionary<string, List<char>> directionList = new Dictionary<string, List<char>>
            {
                { "east", new List<char> { 'J', '7', '-', 'S' } },
                { "west", new List<char> { 'L', 'F', '-', 'S' } },
                { "north", new List<char> { 'F', '7', '|', 'S' } },
                { "south", new List<char> { 'L', 'J', '|', 'S' } }
            };
            Day10Position currentPos = grid.FirstOrDefault(w => w.Type == 'S');
            currentPos.Distance = 0;
            Day10Position east = grid.FirstOrDefault(w => w.X == currentPos.X + 1 && w.Y == currentPos.Y && directionList.FirstOrDefault(w => w.Key == "east").Value.Contains(w.Type));
            Day10Position west = grid.FirstOrDefault(w => w.X == currentPos.X - 1 && w.Y == currentPos.Y && directionList.FirstOrDefault(w => w.Key == "west").Value.Contains(w.Type));
            Day10Position north = grid.FirstOrDefault(w => w.X == currentPos.X && w.Y == currentPos.Y - 1 && directionList.FirstOrDefault(w => w.Key == "north").Value.Contains(w.Type));
            Day10Position south = grid.FirstOrDefault(w => w.X == currentPos.X && w.Y == currentPos.Y + 1 && directionList.FirstOrDefault(w => w.Key == "south").Value.Contains(w.Type));
            if (!Day10IsPartOfLoop(west, grid, "west", directionList))
                if (!Day10IsPartOfLoop(east, grid, "east", directionList))
                    if (!Day10IsPartOfLoop(north, grid, "north", directionList))
                        Day10IsPartOfLoop(south, grid, "south", directionList);
            if (!Day10MarkInsideLoop(grid, "west"))
                if (!Day10MarkInsideLoop(grid, "east"))
                    if (!Day10MarkInsideLoop(grid, "north"))
                        Day10MarkInsideLoop(grid, "south");
            Debug.WriteLine(grid.Where(w => w.Marked).Count());
        }

        private bool Day10IsPartOfLoop(Day10Position position, List<Day10Position> grid, string newDirection, Dictionary<string, List<char>> directionList)
        {
            if (position == null)
                return false;
            Day10Position nextPosition = position;
            List<Day10Position> loopList = new List<Day10Position>();
            int distance = 0;
            while (nextPosition != null && nextPosition?.Type != 'S')
            {
                loopList.Add(nextPosition);
                nextPosition.Visited = true;
                nextPosition.Distance = ++distance;
                List<char> allowedTypes;
                if (nextPosition.Type == '|')
                {
                    allowedTypes = directionList.FirstOrDefault(w => w.Key == newDirection).Value;
                    var query = grid.Where(w => !w.Visited && allowedTypes.Contains(w.Type));
                    query = newDirection == "south" ? query.Where(w => w.X == nextPosition.X && w.Y == nextPosition.Y + 1) : query.Where(w => w.X == nextPosition.X && w.Y == nextPosition.Y - 1);
                    nextPosition = query.FirstOrDefault();
                }
                else if (nextPosition.Type == '-')
                {
                    allowedTypes = directionList.FirstOrDefault(w => w.Key == newDirection).Value;
                    var query = grid.Where(w => !w.Visited && allowedTypes.Contains(w.Type));
                    query = newDirection == "east" ? query.Where(w => w.X == nextPosition.X + 1 && w.Y == nextPosition.Y) : query.Where(w => w.X == nextPosition.X - 1 && w.Y == nextPosition.Y);
                    nextPosition = query.FirstOrDefault();
                }
                else if (nextPosition.Type == 'L')
                {
                    newDirection = newDirection == "south" ? "east" : "north";
                    allowedTypes = directionList.FirstOrDefault(w => w.Key == newDirection).Value;
                    var query = grid.Where(w => !w.Visited && allowedTypes.Contains(w.Type));
                    query = newDirection == "east" ? query.Where(w => w.X == nextPosition.X + 1 && w.Y == nextPosition.Y) : query.Where(w => w.X == nextPosition.X && w.Y == nextPosition.Y - 1);
                    nextPosition = query.FirstOrDefault();
                }
                else if (nextPosition.Type == 'J')
                {
                    newDirection = newDirection == "south" ? "west" : "north";
                    allowedTypes = directionList.FirstOrDefault(w => w.Key == newDirection).Value;
                    var query = grid.Where(w => !w.Visited && allowedTypes.Contains(w.Type));
                    query = newDirection == "west" ? query.Where(w => w.X == nextPosition.X - 1 && w.Y == nextPosition.Y) : query.Where(w => w.X == nextPosition.X && w.Y == nextPosition.Y - 1);
                    nextPosition = query.FirstOrDefault();
                }
                else if (nextPosition.Type == '7')
                {
                    newDirection = newDirection == "north" ? "west" : "south";
                    allowedTypes = directionList.FirstOrDefault(w => w.Key == newDirection).Value;
                    var query = grid.Where(w => !w.Visited && allowedTypes.Contains(w.Type));
                    query = newDirection == "west" ? query.Where(w => w.X == nextPosition.X - 1 && w.Y == nextPosition.Y) : query.Where(w => w.X == nextPosition.X && w.Y == nextPosition.Y + 1);
                    nextPosition = query.FirstOrDefault();
                }
                else if (nextPosition.Type == 'F')
                {
                    newDirection = newDirection == "north" ? "east" : "south";
                    allowedTypes = directionList.FirstOrDefault(w => w.Key == newDirection).Value;
                    var query = grid.Where(w => !w.Visited && allowedTypes.Contains(w.Type));
                    query = newDirection == "east" ? query.Where(w => w.X == nextPosition.X + 1 && w.Y == nextPosition.Y) : query.Where(w => w.X == nextPosition.X && w.Y == nextPosition.Y + 1);
                    nextPosition = query.FirstOrDefault();
                }
                else
                    nextPosition = null;
            }
            if (nextPosition?.Type == 'S')
            {
                loopList.Add(nextPosition);
                loopList.ForEach(e => e.LoopPart = true);
            }
            return nextPosition != null;
        }

        private bool Day10MarkInsideLoop(List<Day10Position> grid, string startSide)
        {
            grid.ForEach(e => e.Marked = false);
            Day10Position nextPosition = null;
            string currentMarkDirection = startSide;
            int maxDistance = grid.Where(w => w.LoopPart).Max(m => (int)m.Distance);
            int currentDistance = -1;
            while (true)
            {
                nextPosition = grid.First(w => w.LoopPart && w.Distance == currentDistance + 1);
                if (nextPosition.Type == '|' || nextPosition.Type == '-' || nextPosition.Type == 'S')
                {
                    if (!Day10MarkDirection(currentMarkDirection, nextPosition, grid))
                        return false;
                }
                else if (nextPosition.Type == 'L')
                {
                    if (!Day10MarkDirection(currentMarkDirection, nextPosition, grid))
                        return false;
                    currentMarkDirection = currentMarkDirection == "east" ? "north"
                        : currentMarkDirection == "west" ? "south"
                        : currentMarkDirection == "north" ? "east"
                        : "west";
                    if (!Day10MarkDirection(currentMarkDirection, nextPosition, grid))
                        return false;
                }
                else if (nextPosition.Type == 'J')
                {
                    if (!Day10MarkDirection(currentMarkDirection, nextPosition, grid))
                        return false;
                    currentMarkDirection = currentMarkDirection == "east" ? "south"
                        : currentMarkDirection == "west" ? "north"
                        : currentMarkDirection == "north" ? "west"
                        : "east";
                    if (!Day10MarkDirection(currentMarkDirection, nextPosition, grid))
                        return false;
                }
                else if (nextPosition.Type == '7')
                {
                    if (!Day10MarkDirection(currentMarkDirection, nextPosition, grid))
                        return false;
                    currentMarkDirection = currentMarkDirection == "east" ? "north"
                        : currentMarkDirection == "west" ? "south"
                        : currentMarkDirection == "north" ? "east"
                        : "west";
                    if (!Day10MarkDirection(currentMarkDirection, nextPosition, grid))
                        return false;
                }
                else if (nextPosition.Type == 'F')
                {
                    if (!Day10MarkDirection(currentMarkDirection, nextPosition, grid))
                        return false;
                    currentMarkDirection = currentMarkDirection == "east" ? "south"
                        : currentMarkDirection == "west" ? "north"
                        : currentMarkDirection == "north" ? "west"
                        : "east";
                    if (!Day10MarkDirection(currentMarkDirection, nextPosition, grid))
                        return false;
                }
                else
                    nextPosition = null;
                if (nextPosition.Distance == maxDistance)
                    break;
                currentDistance++;
            }
            return true;
        }

        private bool Day10MarkDirection(string currentMarkDirection, Day10Position nextPosition, List<Day10Position> grid)
        {
            Day10Position edge = null;
            List<Day10Position> markList = new List<Day10Position>();
            if (currentMarkDirection == "east")
            {
                edge = grid.Where(w => w.X > nextPosition.X && w.Y == nextPosition.Y && w.LoopPart).OrderBy(o => o.X).FirstOrDefault();
                if (edge == null)
                    return false;
                markList = grid.Where(w => w.X > nextPosition.X && w.Y == nextPosition.Y && !w.LoopPart && w.X < edge.X && w.Y == edge.Y).OrderBy(o => o.X).ToList();
            }
            else if (currentMarkDirection == "west")
            {
                edge = grid.Where(w => w.X < nextPosition.X && w.Y == nextPosition.Y && w.LoopPart).OrderByDescending(o => o.X).FirstOrDefault();
                if (edge == null)
                    return false;
                markList = grid.Where(w => w.X < nextPosition.X && w.Y == nextPosition.Y && !w.LoopPart && w.X > edge.X && w.Y == edge.Y).OrderBy(o => o.X).ToList();
            }
            else if (currentMarkDirection == "north")
            {
                edge = grid.Where(w => w.X == nextPosition.X && w.Y < nextPosition.Y && w.LoopPart).OrderByDescending(o => o.Y).FirstOrDefault();
                if (edge == null)
                    return false;
                markList = grid.Where(w => w.X == nextPosition.X && w.Y < nextPosition.Y && !w.LoopPart && w.X == edge.X && w.Y > edge.Y).OrderBy(o => o.X).ToList();
            }
            else if (currentMarkDirection == "south")
            {
                edge = grid.Where(w => w.X == nextPosition.X && w.Y > nextPosition.Y && w.LoopPart).OrderBy(o => o.Y).FirstOrDefault();
                if (edge == null)
                    return false;
                markList = grid.Where(w => w.X == nextPosition.X && w.Y > nextPosition.Y && !w.LoopPart && w.X == edge.X && w.Y < edge.Y).OrderBy(o => o.X).ToList();
            }
            markList.ForEach(e => e.Marked = true);
            return true;
        }

        private void Day10PrintGrid(List<Day10Position> grid, int type)
        {
            for (int y = 0; y <= grid.Max(m => m.Y); y++)
            {
                string line = string.Empty;
                for (int x = 0; x <= grid.Max(m => m.X); x++)
                {
                    Day10Position pos = grid.First(w => w.X == x && w.Y == y);
                    if (type == 1)
                        line += (pos.Distance?.ToString() ?? pos.Type.ToString()).PadRight(2);
                    else if (type == 2)
                        line += pos.LoopPart ? "T" : pos.Marked ? "M" : pos.Type;
                    else if (type == 3)
                        line += pos.Marked ? "I" : pos.Type;
                }
                Debug.WriteLine(line);
            }
        }

        private class Day10Position
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int? Distance { get; set; }
            public char Type { get; set; }
            public bool LoopPart { get; set; }
            public bool Visited { get; set; }
            public bool Marked { get; set; }
            //| is a vertical pipe connecting north and south.
            //- is a horizontal pipe connecting east and west.
            //L is a 90-degree bend connecting north and east.
            //J is a 90-degree bend connecting north and west.
            //7 is a 90-degree bend connecting south and west.
            //F is a 90-degree bend connecting south and east.
            //. is ground; there is no pipe in this tile.
            //S is the starting position of the animal; there is a pipe on this tile, but your sketch doesn't show what shape the pipe has.
        }

        [TestMethod]
        public void Day11_1()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day11.txt").ToList();
            List<Day11Galaxy> universe = new List<Day11Galaxy>();
            int x = 0, y = 0;
            foreach (string row in inputList)
            {
                x = 0;
                foreach (char c in row)
                {
                    if (c == '#')
                        universe.Add(new Day11Galaxy { Y = y, X = x, IsChecked = false });
                    x++;
                }
                y++;
            }
            Day11ExpandUniverse(universe, 1);
            long distance = Day11CalculateDistance(universe);
            Debug.WriteLine(distance);
        }

        [TestMethod]
        public void Day11_2()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day11.txt").ToList();
            List<Day11Galaxy> universe = new List<Day11Galaxy>();
            int x = 0, y = 0;
            foreach (string row in inputList)
            {
                x = 0;
                foreach (char c in row)
                {
                    if (c == '#')
                        universe.Add(new Day11Galaxy { Y = y, X = x, IsChecked = false });
                    x++;
                }
                y++;
            }
            long distance1 = Day11CalculateDistance(universe);
            Day11ExpandUniverse(universe, 1);
            long distance2 = Day11CalculateDistance(universe);
            Debug.WriteLine(distance1 + (distance2 - distance1) * 999999);
        }

        private long Day11CalculateDistance(List<Day11Galaxy> universe)
        {
            long sumLength = 0;
            universe.ForEach(e => e.IsChecked = false);
            foreach (Day11Galaxy galaxy in universe)
            {
                galaxy.IsChecked = true;
                foreach (Day11Galaxy galaxy2 in universe.Where(w => !w.IsChecked))
                    sumLength += Math.Abs(galaxy.X - galaxy2.X) + Math.Abs(galaxy.Y - galaxy2.Y);
            }
            return sumLength;
        }

        private void Day11ExpandUniverse(List<Day11Galaxy> universe, int expansionRate)
        {
            for (int y = 0; y < universe.Max(m => m.Y); y++)
            {
                if (!universe.Any(a => a.Y == y))
                {
                    universe.Where(w => w.Y > y).ToList().ForEach(e => e.Y += expansionRate);
                    y += expansionRate;
                }
            }
            for (int x = 0; x < universe.Max(m => m.X); x++)
            {
                if (!universe.Any(a => a.X == x))
                {
                    universe.Where(w => w.X > x).ToList().ForEach(e => e.X += expansionRate);
                    x += expansionRate;
                }
            }
        }

        private class Day11Galaxy
        {
            public int X { get; set; }
            public int Y { get; set; }
            public bool IsChecked { get; set; }
        }

        private void Day11PrintUniverse(List<Day11Galaxy> universe)
        {
            for (int y = 0; y <= universe.Max(m => m.Y); y++)
            {
                string line = string.Empty;
                for (int x = 0; x <= universe.Max(m => m.X); x++)
                {
                    Day11Galaxy galaxy = universe.FirstOrDefault(w => w.Y == y && w.X == x);
                    line += galaxy != null ? "#" : ".";
                }
                Debug.WriteLine(line);
            }
        }

        [TestMethod]
        public void Day13_1()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day13.txt").ToList();
            List<Day13Pattern> patternList = Day13ParsePattern(inputList);
            int summarize = 0;
            foreach (Day13Pattern pattern in patternList)
            {
                Tuple<int, string> reflection = Day13FindReflectionLines(pattern).First();
                pattern.ReflectionLine = reflection.Item1;
                pattern.ReflectionType = reflection.Item2;
                Debug.WriteLine("PatternNr: " + pattern.PatternNr + " Summarize: " + summarize + " ReflectionLine: " + pattern.ReflectionLine + " ReflectionType: " + pattern.ReflectionType);
                summarize += pattern.ReflectionType == "row" ? pattern.ReflectionLine * 100 : pattern.ReflectionLine;
            }
            Debug.WriteLine(summarize);
        }

        [TestMethod]
        public void Day13_2()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day13.txt").ToList();
            List<Day13Pattern> patternList = Day13ParsePattern(inputList);
            int summarize = 0;
            foreach (Day13Pattern pattern in patternList)
            {
                Tuple<int, string> reflection = Day13FindReflectionLines(pattern).First();
                pattern.ReflectionLine = reflection.Item1;
                pattern.ReflectionType = reflection.Item2;
                Day13FixSmudge(pattern);
                summarize += pattern.ReflectionType == "row" ? pattern.ReflectionLine * 100 : pattern.ReflectionLine;
                Debug.WriteLine("PatternNr: " + pattern.PatternNr + " Summarize: " + summarize + " ReflectionLine: " + pattern.ReflectionLine + " ReflectionType: " + pattern.ReflectionType);
            }
            Debug.WriteLine(summarize);
        }

        private void Day13FixSmudge(Day13Pattern pattern)
        {
            bool isFixed = false;
            int maxY = pattern.Grid.Max(m => m.Y);
            int maxX = pattern.Grid.Max(m => m.X);
            for (int y = 1; y <= maxY; y++)
            {
                for (int u = y + 1; u <= maxY; u++)
                {
                    string currentRow1 = string.Join("", pattern.Grid.Where(w => w.Y == y).Select(s => s.Value));
                    string currentRow2 = string.Join("", pattern.Grid.Where(w => w.Y == u).Select(s => s.Value));
                    Tuple<int, int> match = Day13StringMatch(currentRow1, currentRow2);
                    if (match.Item1 == 1)
                    {
                        Day13Position pos = pattern.Grid.First(w => w.Y == y && w.X == match.Item2);
                        pos.Value = pos.Value == '.' ? '#' : '.';
                        List<Tuple<int, string>> reflectionList = Day13FindReflectionLines(pattern);
                        Tuple<int, string> reflection = reflectionList.FirstOrDefault(w => w.Item1 > -1 && (w.Item1 != pattern.ReflectionLine || w.Item2 != pattern.ReflectionType));
                        if (reflection != null)
                        {
                            isFixed = true;
                            pattern.ReflectionLine = reflection.Item1;
                            pattern.ReflectionType = reflection.Item2;
                            break;
                        }
                        else
                            pos.Value = pos.Value == '.' ? '#' : '.';
                    }
                }
                if (isFixed)
                    break;
            }
            if (!isFixed)
            {
                for (int x = 1; x <= maxX; x++)
                {
                    for (int u = x + 1; u <= maxX; u++)
                    {
                        string currentRow1 = string.Join("", pattern.Grid.Where(w => w.X == x).Select(s => s.Value));
                        string currentRow2 = string.Join("", pattern.Grid.Where(w => w.X == u).Select(s => s.Value));
                        Tuple<int, int> match = Day13StringMatch(currentRow1, currentRow2);
                        if (match.Item1 == 1)
                        {
                            Day13Position pos = pattern.Grid.First(w => w.Y == match.Item2 && w.X == x);
                            pos.Value = pos.Value == '.' ? '#' : '.';
                            List<Tuple<int, string>> reflectionList = Day13FindReflectionLines(pattern);
                            Tuple<int, string> reflection = reflectionList.FirstOrDefault(w => w.Item1 > -1 && (w.Item1 != pattern.ReflectionLine || w.Item2 != pattern.ReflectionType));
                            if (reflection != null)
                            {
                                isFixed = true;
                                pattern.ReflectionLine = reflection.Item1;
                                pattern.ReflectionType = reflection.Item2;
                                break;
                            }
                            else
                                pos.Value = pos.Value == '.' ? '#' : '.';
                        }
                    }
                    if (isFixed)
                        break;
                }
            }
        }

        private Tuple<int, int> Day13StringMatch(string string1, string string2)
        {
            int mismatchCount = 0;
            int mismatchIndex = -1;
            for (int i = 0; i < string1.Length; i++)
            {
                if (string1[i] != string2[i])
                {
                    mismatchCount++;
                    mismatchIndex = i;
                }
            }
            return new Tuple<int, int>(mismatchCount, mismatchIndex + 1);
        }

        private List<Day13Pattern> Day13ParsePattern(List<string> inputList)
        {
            List<Day13Pattern> patternList = new List<Day13Pattern>();
            int patternNr = 1;
            Day13Pattern patternInput = new Day13Pattern { Grid = new List<Day13Position>(), PatternNr = patternNr };
            patternList.Add(patternInput);
            int y = 1, x = 1;
            foreach (string input in inputList)
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    patternNr++;
                    patternInput = new Day13Pattern { Grid = new List<Day13Position>(), PatternNr = patternNr };
                    patternList.Add(patternInput);
                    y = 0;
                }
                else
                {
                    x = 1;
                    foreach (char c in input)
                    {
                        patternInput.Grid.Add(new Day13Position { Y = y, X = x, Value = c });
                        x++;
                    }
                }
                y++;
            }
            return patternList;
        }

        private List<Tuple<int, string>> Day13FindReflectionLines(Day13Pattern pattern)
        {
            int reflectionLine = -1;
            int maxY = pattern.Grid.Max(m => m.Y);
            int maxX = pattern.Grid.Max(m => m.X);
            int minY = 1;
            int minX = 1;
            string firstRow = string.Join("", pattern.Grid.Where(w => w.Y == minY).Select(s => s.Value));
            string lastRow = string.Join("", pattern.Grid.Where(w => w.Y == maxY).Select(s => s.Value));
            string firstColumn = string.Join("", pattern.Grid.Where(w => w.X == minX).Select(s => s.Value));
            string lastColumn = string.Join("", pattern.Grid.Where(w => w.X == maxX).Select(s => s.Value));
            List<Tuple<int, string>> returnList = new List<Tuple<int, string>>();
            for (int y = 1; y <= maxY; y++)
            {
                string currentRow = string.Join("", pattern.Grid.Where(w => w.Y == y).Select(s => s.Value));
                if (y != maxY && currentRow == lastRow)
                {
                    if ((y + maxY) % 2 != 1)
                        continue;
                    reflectionLine = Day13ValidateMirror(pattern, y, maxY, true);
                    if (reflectionLine > -1)
                        returnList.Add(new Tuple<int, string>(reflectionLine, "row"));
                }
                if (y != minY && currentRow == firstRow)
                {
                    if ((y + minY) % 2 != 1)
                        continue;
                    reflectionLine = Day13ValidateMirror(pattern, minY, y, true);
                    if (reflectionLine > -1)
                        returnList.Add(new Tuple<int, string>(reflectionLine, "row"));
                }
            }
            for (int x = 1; x <= maxX; x++)
            {
                string currentRow = string.Join("", pattern.Grid.Where(w => w.X == x).Select(s => s.Value));
                if (x != maxX && currentRow == lastColumn)
                {
                    if ((x + maxX) % 2 != 1)
                        continue;
                    reflectionLine = Day13ValidateMirror(pattern, x, maxX, false);
                    if (reflectionLine > -1)
                        returnList.Add(new Tuple<int, string>(reflectionLine, "column"));
                }
                if (x != minX && currentRow == firstColumn)
                {
                    if ((x + minX) % 2 != 1)
                        continue;
                    reflectionLine = Day13ValidateMirror(pattern, minX, x, false);
                    if (reflectionLine > -1)
                        returnList.Add(new Tuple<int, string>(reflectionLine, "column"));
                }
            }
            return returnList;
        }

        private int Day13ValidateMirror(Day13Pattern pattern, int pos1, int pos2, bool checkRows)
        {
            bool isMirrored = true;
            string side1 = string.Empty, side2 = string.Empty;
            int smaller = pos1 < pos2 ? pos1 : pos2;
            int bigger = pos1 > pos2 ? pos1 : pos2;
            while (true)
            {
                if (bigger - smaller == 2)
                {
                    isMirrored = false;
                    break;
                }
                else if ((bigger - smaller) > 2)
                {
                    bigger--;
                    smaller++;
                    if (checkRows)
                    {
                        side1 = string.Join("", pattern.Grid.Where(w => w.Y == bigger).Select(s => s.Value));
                        side2 = string.Join("", pattern.Grid.Where(w => w.Y == smaller).Select(s => s.Value));
                    }
                    else
                    {
                        side1 = string.Join("", pattern.Grid.Where(w => w.X == bigger).Select(s => s.Value));
                        side2 = string.Join("", pattern.Grid.Where(w => w.X == smaller).Select(s => s.Value));
                    }
                    if (side1 != side2)
                    {
                        isMirrored = false;
                        break;
                    }
                }
                else
                    break;
            }
            if (isMirrored)
                return smaller;
            else
                return -1;
        }

        private void Day13Print(Day13Pattern pattern)
        {
            for (int y = 1; y <= pattern.Grid.Max(m => m.Y); y++)
            {
                string row = string.Empty;
                for (int x = 1; x <= pattern.Grid.Max(m => m.X); x++)
                {
                    row += pattern.Grid.First(w => w.Y == y && w.X == x).Value;
                    if (pattern.ReflectionType == "column" && x == pattern.ReflectionLine)
                        row += "|";
                }
                Debug.WriteLine(row);
                if (pattern.ReflectionType == "row" && y == pattern.ReflectionLine)
                    Debug.WriteLine("".PadRight(pattern.Grid.Max(m => m.X), '-'));

            }
        }

        private class Day13Pattern
        {
            public List<Day13Position> Grid { get; set; }
            public int ReflectionLine { get; set; }
            public string ReflectionType { get; set; }
            public int PatternNr { get; set; }
        }

        private class Day13Position
        {
            public int X { get; set; }
            public int Y { get; set; }
            public char Value { get; set; }
        }
    }
}