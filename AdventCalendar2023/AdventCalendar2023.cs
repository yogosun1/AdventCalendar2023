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
        public void Day12_1()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<string> inputList = File.ReadAllLines(@"Input\Day12.txt").ToList();
            List<Day12Group> groups = new List<Day12Group>();
            foreach (string groupInput in inputList)
            {
                List<string> splitInput = groupInput.Split(' ').ToList();
                Day12Group group = new Day12Group { DamageList = new List<int>(), Springs = splitInput[0] };
                group.DamageList = splitInput[1].Split(',').Select(int.Parse).ToList();
                groups.Add(group);
            }
            int arrangements = 0;
            int tests = 0;
            int groupCount = groups.Count();
            int currentGroup = 0;
            foreach (Day12Group group in groups)
            {
                int maxLength = group.Springs.Length;
                string test = string.Empty;
                int[] operationalList = new int[group.DamageList.Count];
                for (int i = 0; i < operationalList.Length; i++)
                    operationalList[i] = 1;
                operationalList[0] = group.Springs.IndexOfAny(new char[] { '?', '#' });
                int maxOperational = group.Springs.Length - group.DamageList.Sum();
                int minEndingOperationalSpring = group.Springs.Length - (group.Springs.LastIndexOfAny(new char[] { '?', '#' }) + 1);
                maxOperational -= minEndingOperationalSpring;
                int oIndex = operationalList.Length - 1;
                while (true)
                {
                    tests++;
                    test = string.Empty;
                    for (int i = 0; i < group.DamageList.Count; i++)
                        test += "".PadRight(operationalList[i], '.') + "".PadRight(group.DamageList[i], '#');
                    test = test.PadRight(minEndingOperationalSpring + test.Length, '.');
                    if (test.Length <= maxLength)
                    {
                        test = test.PadRight(maxLength, '.');
                        if (Day12IsValid(test, group.Springs))
                            arrangements++;
                    }
                    operationalList[oIndex]++;
                    while (operationalList.Sum() > maxOperational && oIndex > 0)
                    {
                        for (int k = oIndex; k < operationalList.Length; k++)
                            operationalList[k] = 1;
                        oIndex--;
                        operationalList[oIndex]++;
                    }
                    if (operationalList.Sum() > maxOperational)
                        break;
                    oIndex = operationalList.Length - 1;
                }
                currentGroup++;
                Debug.WriteLine("Group: " + currentGroup + " Arrangements: " + arrangements + " Tests: " + tests);
            }
            Debug.WriteLine(arrangements + " " + tests);
            Debug.WriteLine("Arrangements: " + arrangements + " Tests: " + +tests + " Elapsed: " + stopwatch.Elapsed);
        }

        [TestMethod]
        public void Day12_2_DoesNotWork()
        {
            // This code does not work (its not fast enough)
            // This attempt is without recursive calls
            List<string> inputList = File.ReadAllLines(@"Input\Day12.txt").ToList();
            List<Day12Group> groups = new List<Day12Group>();
            int i = 0, p = 0, k = 0;
            foreach (string groupInput in inputList)
            {
                List<string> splitInput = groupInput.Split(' ').ToList();
                string springInput = splitInput[0];
                string damageInput = splitInput[1];
                for (i = 0; i < 4; i++)
                {
                    springInput += "?" + splitInput[0];
                    damageInput += "," + splitInput[1];
                }
                Day12Group group = new Day12Group { DamageList = new List<int>(), Springs = springInput, OriginalSprings = springInput };
                group.DamageList = damageInput.Split(',').Select(int.Parse).ToList();
                group.UnknownList = springInput.Split('.').Where(w => !string.IsNullOrWhiteSpace(w)).ToList();
                groups.Add(group);
            }
            Day12SolvePartOfSprings(groups);
            Day12SolvePartOfSprings2(groups);
            Day12SolvePartOfSprings(groups);
            long arrangements = 0;
            long tests = 0;
            int groupCount = groups.Count();
            int currentGroup = 0;
            Stopwatch stopwatchInitGroup = new Stopwatch();
            Stopwatch stopwatchValidation = new Stopwatch();
            Stopwatch stopwatchSwitchIndex = new Stopwatch();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<long> arrangementList = new List<long>();
            foreach (Day12Group group in groups)
            {
                int testCount = 0;
                arrangements = 0;
                stopwatchInitGroup.Start();
                int maxLength = group.Springs.Length;
                string test = string.Empty;
                int[] operationalList = new int[group.DamageList.Count];
                for (i = 0; i < operationalList.Length; i++)
                    operationalList[i] = 1;
                operationalList[0] = group.Springs.IndexOfAny(new char[] { '?', '#' });
                int maxOperational = group.Springs.Length - group.DamageList.Sum();
                int minEndingOperationalSpring = group.Springs.Length - (group.Springs.LastIndexOfAny(new char[] { '?', '#' }) + 1);
                maxOperational -= minEndingOperationalSpring;
                int oIndex = operationalList.Length - 1;
                Day12PrefillOperationalList(group, operationalList);
                int[] operationalPrefillList = new int[operationalList.Count()];
                for (i = 0; i < operationalList.Count(); i++)
                    operationalPrefillList[i] = operationalList[i];
                stopwatchInitGroup.Stop();
                while (true)
                {
                    tests++;
                    stopwatchValidation.Start();
                    if (Day12IsValid2(group, operationalList))
                    {
                        arrangements++;
                    }
                    stopwatchValidation.Stop();
                    stopwatchSwitchIndex.Start();
                    operationalList[oIndex]++;
                    while (operationalList.Sum() > maxOperational && oIndex > 0)
                    {
                        for (k = oIndex; k < operationalList.Length; k++)
                            operationalList[k] = operationalPrefillList[k];
                        oIndex--;
                        operationalList[oIndex]++;
                    }
                    if (operationalList.Sum() > maxOperational)
                        break;
                    oIndex = operationalList.Length - 1;
                    stopwatchSwitchIndex.Stop();
                }
                currentGroup++;
                arrangementList.Add(arrangements);
                Debug.WriteLine("Group: " + currentGroup + " Arrangements: " + arrangements + " Tests: " + testCount + " Springs: " + group.Springs + " " + string.Join(',', group.DamageList));
            }
            stopwatch.Stop();
            Debug.WriteLine("Arrangements: " + arrangementList.Sum() + " Tests: " + tests
                + " Elapsed: " + stopwatch.Elapsed
                + " stopwatchInitGroup: " + stopwatchInitGroup.Elapsed
                + " stopwatchValidation: " + stopwatchValidation.Elapsed
                + " stopwatchSwitchIndex: " + stopwatchSwitchIndex.Elapsed
                + " Total minus init: " + (stopwatch.Elapsed - stopwatchInitGroup.Elapsed)
                + " Important time: " + (stopwatchValidation.Elapsed + stopwatchSwitchIndex.Elapsed)
                );
        }

        [TestMethod]
        public void Day12_2_Works()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day12.txt").ToList();
            List<Day12Group> groups = new List<Day12Group>();
            int groupNr = 1;
            foreach (string groupInput in inputList)
            {
                List<string> splitInput = groupInput.Split(' ').ToList();
                string springInput = splitInput[0];
                string damageInput = splitInput[1];
                for (int i = 0; i < 4; i++)
                {
                    springInput += "?" + splitInput[0];
                    damageInput += "," + splitInput[1];
                }
                Day12Group group = new Day12Group { DamageList = new List<int>(), Springs = springInput, OriginalSprings = springInput };
                group.DamageList = damageInput.Split(',').Select(int.Parse).ToList();
                group.UnknownList = springInput.Split('.').Where(w => !string.IsNullOrWhiteSpace(w)).ToList();
                group.GroupNr = groupNr;
                groups.Add(group);
                groupNr++;
            }
            // These presolvers are not necessary. I used them in a previous attempt that was slow
            // But im keeping them anyway for fun
            Day12SolvePartOfSprings(groups);
            Day12SolvePartOfSprings2(groups);
            Day12SolvePartOfSprings(groups);
            long arrangements = 0;
            List<long> arrangementList = new List<long>();
            foreach (Day12Group group in groups)
            {
                arrangements = Day12TestArrangements(group.Springs, group.DamageList);
                arrangementList.Add(arrangements);
                Debug.WriteLine("Group: " + group.GroupNr + " Arrangements: " + arrangements + " Springs: " + group.Springs + " " + string.Join(',', group.DamageList));
            }
            Debug.WriteLine("Arrangements: " + arrangementList.Sum());
        }

        private Dictionary<string, long> Day12KnownArrangements = new Dictionary<string, long>();

        private long Day12TestArrangements(string springs, List<int> damageList)
        {
            if (springs.Count(c => c == '#' || c == '?') < damageList.Sum())
                return 0;
            else if (damageList.Count() == 0 && !springs.Any(a => a == '#'))
                return 1;
            else if (damageList.Count() == 0 && springs.Any(a => a == '#'))
                return 0;
            else if (springs[0] == '.')
                return Day12CheckKnownArrangements(springs.Substring(1), damageList);
            else if (springs.Length < (damageList.Sum() + damageList.Count() - 1))
                return 0;
            else if (springs[0] == '#')
            {
                if ((springs.Length > damageList.First() && springs[damageList.First()] == '#')
                    || springs.Take(damageList.First()).Any(a => a == '.'))
                    return 0;
                return Day12CheckKnownArrangements(
                    springs.Length == damageList.First() ? springs.Substring(damageList.First()) : '.' + springs.Substring(damageList.First() + 1)
                    , damageList.Count() > 0 ? damageList.Skip(1).ToList() : null);
            }
            else if (springs[0] == '?')
                return Day12CheckKnownArrangements('.' + springs.Substring(1), damageList)
                    + Day12CheckKnownArrangements('#' + springs.Substring(1), damageList);
            return 0;
        }

        private long Day12CheckKnownArrangements(string springs, List<int> damageList)
        {
            string key = springs + " " + string.Join(',', damageList);
            long value;
            if (!Day12KnownArrangements.TryGetValue(key, out value))
            {
                value = Day12TestArrangements(springs, damageList);
                Day12KnownArrangements.Add(key, value);
            }
            return value;
        }

        private void Day12PrefillOperationalList(Day12Group group, int[] operationalList)
        {
            int unknownIndex = 0;
            int damageIndex = 0;
            int currentOperationalCount = 0;
            int operationalIndex = 0;
            bool equalUnknown = group.UnknownList.Count() == group.DamageList.Count();
            if (equalUnknown)
            {
                for (int i = 0; i < group.UnknownList.Count(); i++)
                    if (group.UnknownList[i].Count() > (group.DamageList[i] + 1))
                        equalUnknown = false;
            }
            List<string> matchList = new List<string> { "#", "?#?" };
            for (int i = 0; i < group.Springs.Count(); i++)
            {
                char c = group.Springs[i];
                if (c == '.')
                    currentOperationalCount++;
                else if (c == '?' || c == '#')
                {
                    operationalList[operationalIndex] = currentOperationalCount;
                    currentOperationalCount = 0;
                    string unknown = group.UnknownList[unknownIndex];
                    int damaged = group.DamageList[unknownIndex];
                    int nextDamaged = unknownIndex <= (group.DamageList.Count() - 2) ? group.DamageList[unknownIndex + 1] : 1;

                    if (matchList.Contains(unknown))
                        i += (unknown.Length - 1);
                    else if (unknown.Any(a => a == '#') && (unknown.Count() - nextDamaged) <= damaged)
                        i += (unknown.Length - 1);
                    else if (unknown.Count() > 1 && unknown.Count(c => c == '?') <= 1)
                        i += (unknown.Length - 1);
                    else if (unknown.Count() > 2 && unknown.Count(c => c == '?') == 1 && unknown.First() == '?' && unknown.Last() == '?')
                        i += (unknown.Length - 1);
                    else if (equalUnknown && unknown.Count() == damaged) // does nothing atm
                        i += (unknown.Length - 1);
                    else
                        break;
                    unknownIndex++;
                    operationalIndex++;
                    if (operationalIndex >= operationalList.Count())
                        break;
                }
            }
            unknownIndex = group.UnknownList.Count() - 1;
            damageIndex = group.DamageList.Count() - 1;
            currentOperationalCount = 0;
            operationalIndex = operationalList.Count() - 1;
            bool first = true;
            for (int i = group.Springs.Count() - 1; i >= 0; i--)
            {
                char c = group.Springs[i];
                if (c == '.')
                    currentOperationalCount++;
                else if (c == '?' || c == '#')
                {
                    if (!first)
                        operationalList[operationalIndex] = currentOperationalCount;
                    else
                    {
                        operationalIndex++;
                        first = false;
                    }
                    currentOperationalCount = 0;
                    string unknown = group.UnknownList[unknownIndex];
                    int damaged = damageIndex >= 0 ? group.DamageList[damageIndex] : 0;
                    int nextDamaged = damageIndex - 1 >= 0 ? group.DamageList[damageIndex - 1] : 1;
                    if (matchList.Contains(unknown))
                        i -= (unknown.Length - 1);
                    else if (unknown.Any(a => a == '#') && (unknown.Count() - nextDamaged) <= damaged)
                        i -= (unknown.Length - 1);
                    else if (unknown.Count() > 1 && unknown.Count(c => c == '?') <= 1)
                        i -= (unknown.Length - 1);
                    else if (unknown.Count() > 2 && unknown.Count(c => c == '?') == 1 && unknown.First() == '?' && unknown.Last() == '?')
                        i -= (unknown.Length - 1);
                    else if (equalUnknown && unknown.Count() == damaged) // does nothing atm
                        i -= (unknown.Length - 1);
                    else
                        break;
                    unknownIndex--;
                    operationalIndex--;
                    damageIndex--;
                    if (operationalIndex < 0)
                        break;
                }
            }
        }

        private void Day12SolvePartOfSprings(List<Day12Group> groups)
        {
            foreach (Day12Group group in groups)
            {
                int unknownIndex = 0;
                int damageIndex = 0;
                bool equalUnknown = group.UnknownList.Count() == group.DamageList.Count();
                if (equalUnknown)
                {
                    for (int i = 0; i < group.UnknownList.Count(); i++)
                        if (group.UnknownList[i].Count() > (group.DamageList[i] + 1))
                            equalUnknown = false;
                }
                List<string> matchList = new List<string> { "#", "?#?" };
                for (int i = 0; i < group.Springs.Count(); i++)
                {
                    char c = group.Springs[i];
                    if (c == '.')
                        continue;
                    else if (c == '?' || c == '#')
                    {
                        string unknown = group.UnknownList[unknownIndex];
                        if (damageIndex > (group.DamageList.Count() - 1) && unknownIndex < group.UnknownList.Count())
                        {
                            for (int s = i; s < group.Springs.Length; s++)
                                if (group.Springs[s] == '?')
                                    group.Springs = group.Springs.ReplaceAt(s, '.');
                            break;
                        }
                        int damaged = group.DamageList[damageIndex];
                        int nextDamaged = damageIndex <= (group.DamageList.Count() - 2) ? group.DamageList[damageIndex + 1] : 1;

                        if (unknown.Count(t => t == '#') == damaged && unknown.Length == (damaged + 1))
                        {
                            for (int s = i; s < i + unknown.Length; s++)
                                if (group.Springs[s] == '?')
                                    group.Springs = group.Springs.ReplaceAt(s, '.');
                            i += (unknown.Length - 1);
                        }
                        else if (unknown.Count() == damaged && unknown.Contains('#') && unknown.Contains('?'))
                        {
                            for (int s = i; s < i + unknown.Length; s++)
                                if (group.Springs[s] == '?')
                                    group.Springs = group.Springs.ReplaceAt(s, '#');
                            i += (unknown.Length - 1);
                        }
                        else if (unknown.Any(a => a == '#') && (unknown.Count() - nextDamaged) <= damaged)
                        {
                            int edge = unknown.Length - damaged;
                            for (int s = i + edge; s < i + unknown.Length - edge; s++)
                                if (group.Springs[s] == '?')
                                    group.Springs = group.Springs.ReplaceAt(s, '#');
                            int firstIndex = unknown.IndexOf('#');
                            int lastIndex = unknown.LastIndexOf('#');
                            if (firstIndex + damaged < unknown.Length)
                            {
                                for (int s = i + firstIndex + damaged; s < i + unknown.Length; s++)
                                    if (group.Springs[s] == '?')
                                        group.Springs = group.Springs.ReplaceAt(s, '.');
                            }
                            if (lastIndex - damaged > 0)
                            {
                                int index = lastIndex - damaged;
                                for (int s = i; s < i + index; s++)
                                    if (group.Springs[s] == '?')
                                        group.Springs = group.Springs.ReplaceAt(s, '.');
                            }
                            i += (unknown.Length - 1);
                        }
                        else if (equalUnknown && unknown.Count() == damaged)
                        {
                            if (unknown.Any(a => a == '?'))
                                for (int s = i; s < i + unknown.Length; s++)
                                    if (group.Springs[s] == '?')
                                        group.Springs = group.Springs.ReplaceAt(s, '#');
                            i += (unknown.Length - 1);
                        }
                        else if (equalUnknown && unknown.Count() <= 2)
                        {
                            if (damaged == unknown.Count())
                                for (int s = i; s < i + unknown.Length; s++)
                                    if (group.Springs[s] == '?')
                                        group.Springs = group.Springs.ReplaceAt(s, '#');
                            i += (unknown.Length - 1);
                        }
                        else if (unknown == "?#?")
                        {
                            if (damaged == 1)
                                for (int s = i; s < i + unknown.Length; s++)
                                    if (group.Springs[s] == '?')
                                        group.Springs = group.Springs.ReplaceAt(s, '.');
                            i += (unknown.Length - 1);
                        }
                        else if (unknown.Count() < damaged && unknown.All(a => a == '?'))
                        {
                            for (int s = i; s < i + unknown.Length; s++)
                                if (group.Springs[s] == '?')
                                    group.Springs = group.Springs.ReplaceAt(s, '.');
                            i += (unknown.Length - 1);
                            damageIndex--;
                        }
                        else if (unknown.Substring(0, damaged - 1).Contains('#') && unknown.Substring(0, damaged).Contains('?'))
                        {
                            int edge = unknown.Substring(0, damaged - 1).IndexOf('#');
                            for (int s = i + edge; s < i + damaged; s++)
                                if (group.Springs[s] == '?')
                                    group.Springs = group.Springs.ReplaceAt(s, '#');
                            break;
                        }
                        else if (unknown.Any(a => a == '#') && unknown.Length - damaged < damaged)
                        {
                            int edge = unknown.Length - damaged;
                            for (int s = i + edge; s < i + unknown.Length - edge; s++)
                                if (group.Springs[s] == '?')
                                    group.Springs = group.Springs.ReplaceAt(s, '#');
                            break;
                        }
                        else
                            break;
                        unknownIndex++;
                        damageIndex++;
                    }
                }
                group.UnknownList = group.Springs.Split('.').Where(w => !string.IsNullOrWhiteSpace(w)).ToList();
                unknownIndex = group.UnknownList.Count() - 1;
                damageIndex = group.DamageList.Count() - 1;
                for (int i = group.Springs.Count() - 1; i >= 0; i--)
                {
                    char c = group.Springs[i];
                    if (c == '.')
                        continue;
                    else if (c == '?' || c == '#')
                    {
                        string unknown = group.UnknownList[unknownIndex];
                        if (damageIndex < 0 && unknownIndex >= 0)
                        {
                            for (int s = i; s >= 0; s--)
                                if (group.Springs[s] == '?')
                                    group.Springs = group.Springs.ReplaceAt(s, '.');
                            break;
                        }
                        int damaged = group.DamageList[damageIndex];
                        int nextDamaged = damageIndex - 1 >= 0 ? group.DamageList[damageIndex - 1] : 1;

                        if (unknown.Count(t => t == '#') == damaged && unknown.Length == (damaged + 1))
                        {
                            for (int s = i; s > i - unknown.Length; s--)
                                if (group.Springs[s] == '?')
                                    group.Springs = group.Springs.ReplaceAt(s, '.');
                            i -= (unknown.Length - 1);
                        }
                        else if (unknown.Count() == damaged && unknown.Contains('#') && unknown.Contains('?'))
                        {
                            for (int s = i; s > i - unknown.Length; s--)
                                if (group.Springs[s] == '?')
                                    group.Springs = group.Springs.ReplaceAt(s, '#');
                            i -= (unknown.Length - 1);
                        }
                        else if (unknown.Any(a => a == '#') && (unknown.Count() - nextDamaged) <= damaged)
                        {
                            int edge = unknown.Length - damaged;
                            for (int s = i - edge; s > i - unknown.Length + edge; s--)
                                if (group.Springs[s] == '?')
                                    group.Springs = group.Springs.ReplaceAt(s, '#');
                            i -= (unknown.Length - 1);
                        }
                        else if (equalUnknown && unknown.Count() == damaged)
                        {
                            if (unknown.Any(a => a == '?'))
                                for (int s = i; s > i - unknown.Length; s--)
                                    if (group.Springs[s] == '?')
                                        group.Springs = group.Springs.ReplaceAt(s, '#');
                            i -= (unknown.Length - 1);
                        }
                        else if (equalUnknown && unknown.Count() <= 2)
                        {
                            if (damaged == unknown.Count())
                                for (int s = i; s > i - unknown.Length; s--)
                                    if (group.Springs[s] == '?')
                                        group.Springs = group.Springs.ReplaceAt(s, '#');
                            i -= (unknown.Length - 1);
                        }
                        else if (unknown == "?#?")
                        {
                            if (damaged == 1)
                                for (int s = i; s > i - unknown.Length; s--)
                                    if (group.Springs[s] == '?')
                                        group.Springs = group.Springs.ReplaceAt(s, '.');
                            i -= (unknown.Length - 1);
                        }
                        else if (unknown.Count() < damaged && unknown.All(a => a == '?'))
                        {
                            for (int s = i; s > i - unknown.Length; s--)
                                if (group.Springs[s] == '?')
                                    group.Springs = group.Springs.ReplaceAt(s, '.');
                            i -= (unknown.Length - 1);
                            damageIndex++;
                        }
                        else if (unknown.Substring(unknown.Length - damaged, damaged - 1).Contains('#') && unknown.Substring(unknown.Length - damaged - 1, damaged - 1).Contains('?'))
                        {
                            int edge = unknown.Length - unknown.LastIndexOf('#');
                            for (int s = i - edge; s > i - damaged; s--)
                                if (group.Springs[s] == '?')
                                    group.Springs = group.Springs.ReplaceAt(s, '#');
                            break;
                        }
                        else if (unknown.Any(a => a == '#') && unknown.Length - damaged < damaged)
                        {
                            int edge = unknown.Length - damaged;
                            for (int s = i - edge; s > i - unknown.Length + edge; s--)
                                if (group.Springs[s] == '?')
                                    group.Springs = group.Springs.ReplaceAt(s, '#');
                            break;
                        }
                        else
                            break;
                        unknownIndex--;
                        damageIndex--;
                    }
                }
                group.UnknownList = group.Springs.Split('.').Where(w => !string.IsNullOrWhiteSpace(w)).ToList();
            }
        }

        private void Day12SolvePartOfSprings2(List<Day12Group> groups)
        {
            foreach (Day12Group group in groups)
            {
                int largestDamage = group.DamageList.Max();
                List<string> bigUnknown = group.UnknownList.Where(w => w.Count() >= largestDamage).ToList();
                if (bigUnknown.Count() == 1)
                {
                    string unknown = bigUnknown.FirstOrDefault();
                    int index = group.Springs.IndexOf(unknown);
                    int edge = unknown.Length - largestDamage;
                    for (int s = index + edge; s < index + unknown.Length - edge; s++)
                        if (group.Springs[s] == '?')
                            group.Springs = group.Springs.ReplaceAt(s, '#');
                }
                group.UnknownList = group.Springs.Split('.').Where(w => !string.IsNullOrWhiteSpace(w)).ToList();
            }
        }

        private bool Day12IsValid(string test, string group)
        {
            for (int i = 0; i < group.Length; i++)
            {
                if (test[i] == group[i] || ((test[i] == '#' || test[i] == '.') && group[i] == '?'))
                    continue;
                else
                    return false;
            }
            return true;
        }

        private bool Day12IsValid2(Day12Group group, int[] operationalList)
        {
            int currentIndex = 0;
            for (int i = 0; i < operationalList.Count(); i++)
            {
                for (int p = 0; p < operationalList[i]; p++)
                    if (group.Springs[currentIndex + p] == '#')
                        return false;
                currentIndex += operationalList[i];
                for (int p = 0; p < group.DamageList[i]; p++)
                    if (group.Springs[currentIndex + p] == '.')
                        return false;
                currentIndex += group.DamageList[i];
            }
            for (int p = currentIndex; p < group.Springs.Count(); p++)
                if (group.Springs[p] == '#')
                    return false;
            return true;
        }

        private void Day12PrintCurrentString(int[] operationalList, List<int> damagedList, string springs, bool isValid)
        {
            string row = string.Empty;
            for (int i = 0; i < operationalList.Count(); i++)
                row += "".PadRight(operationalList[i], '.') + "".PadRight(damagedList[i], '#');
            row += "".PadRight(springs.Length - row.Length, '.') + " " + springs + " " + string.Join(',', damagedList) + (isValid ? " Valid" : "");
            Debug.WriteLine(row);
        }

        private class Day12Group
        {
            public int GroupNr { get; set; }
            public string Springs { get; set; }
            public string OriginalSprings { get; set; }
            public List<int> DamageList { get; set; }
            public List<string> UnknownList { get; set; }
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
            int maxY = grid.Max(m => m.Y);
            int maxX = grid.Max(m => m.Y);
            List<int> pattern = new List<int>();
            List<int> patternFound = new List<int>();
            int patternLength = 0;
            for (int i = 1; i <= 1000000000; i++)
            {
                Day14SpinCycle(grid, maxY, maxX);
                pattern.Add(grid.Where(w => w.Type == 'O').Sum(s => (maxY + 1) - s.Y));
                Debug.WriteLine("Cycle: " + i + " Load: " + pattern.Last());
                if (pattern.Count() > 10)
                {
                    int length = Day14FindPattern(pattern);
                    if (length > 0)
                    {
                        patternFound = pattern.Skip(pattern.Count() - length).ToList();
                        patternLength = length;
                        break;
                    }
                }
            }
            int remainingCycles = 1000000000 - pattern.Count();
            int patternCycle = remainingCycles % patternLength;
            int load = patternFound[patternCycle - 1];
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
            int repeatCount = -1;
            int patternLength = 0;
            for (int i = 0; i < 20; i++)
            {
                repeatCount = -1;
                int length = 3 + i;
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
        public void Day15_1()
        {
            List<string> initSequence = File.ReadAllLines(@"Input\Day15.txt").ToList().First().Split(',').ToList();
            long sum = 0;
            foreach (string s in initSequence)
                sum += Day15HASH(s);
            Debug.WriteLine(sum);
        }

        [TestMethod]
        public void Day15_2()
        {
            List<string> initSequence = File.ReadAllLines(@"Input\Day15.txt").ToList().First().Split(',').ToList();
            List<Day15Instruction> instructions = new List<Day15Instruction>();
            foreach (string instructionInput in initSequence)
            {
                Day15Instruction instruction = new Day15Instruction();
                instruction.Label = instructionInput.Substring(0, instructionInput.IndexOfAny(new char[] { '-', '=' }));
                instruction.BoxNr = Day15HASH(instruction.Label);
                instruction.Operation = instructionInput[instructionInput.IndexOfAny(new char[] { '-', '=' })];
                instruction.FocalLength = instruction.Operation == '=' ? int.Parse(instructionInput.Last().ToString()) : null;
                instructions.Add(instruction);
            }
            List<Day15Box> boxList = new List<Day15Box>();
            foreach (Day15Instruction instruction in instructions)
            {
                Day15Box box = boxList.FirstOrDefault(w => w.BoxNr == instruction.BoxNr);
                if (box == null)
                {
                    box = new Day15Box { BoxNr = instruction.BoxNr, LensList = new List<Day15Lens>() };
                    boxList.Add(box);
                }
                Day15Lens lens = box.LensList.FirstOrDefault(w => w.Label == instruction.Label);
                if (instruction.Operation == '=')
                {
                    if (lens == null)
                        box.LensList.Add(new Day15Lens { FocalLength = (int)instruction.FocalLength, Label = instruction.Label });
                    else
                        lens.FocalLength = (int)instruction.FocalLength;
                }
                else if (instruction.Operation == '-' && lens != null)
                    box.LensList.Remove(lens);
            }
            int totalFocusingPower = 0;
            foreach (Day15Box box in boxList)
                foreach (Day15Lens lens in box.LensList)
                    totalFocusingPower += (1 + box.BoxNr) * (box.LensList.IndexOf(lens) + 1) * lens.FocalLength;
            Debug.WriteLine(totalFocusingPower);
        }

        private int Day15HASH(string input)
        {
            int hash = 0;
            foreach (char c in input)
            {
                hash += (int)c;
                hash *= 17;
                hash %= 256;
            }
            return hash;
        }

        private class Day15Box
        {
            public int BoxNr { get; set; }
            public List<Day15Lens> LensList { get; set; }
        }

        private class Day15Lens
        {
            public string Label { get; set; }
            public int FocalLength { get; set; }
        }

        private class Day15Instruction
        {
            public string Label { get; set; }
            public int BoxNr { get; set; }
            public char Operation { get; set; }
            public int? FocalLength { get; set; }
        }

        [TestMethod]
        public void Day16_1()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day16.txt").ToList();
            List<Day16Position> grid = new List<Day16Position>();
            int y = 0, x = 0;
            foreach (string input in inputList)
            {
                x = 0;
                foreach (char c in input)
                    grid.Add(new Day16Position { X = x++, Y = y, Type = c, IsEnergized = false });
                y++;
            }
            Day16MoveLight(grid, grid.First(w => w.X == 0 && w.Y == 0), "east");
            Debug.WriteLine(grid.Count(c => c.IsEnergized)); // 6978
        }

        [TestMethod]
        public void Day16_2()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day16.txt").ToList();
            List<Day16Position> grid = new List<Day16Position>();
            int y = 0, x = 0;
            foreach (string input in inputList)
            {
                x = 0;
                foreach (char c in input)
                    grid.Add(new Day16Position { X = x++, Y = y, Type = c, IsEnergized = false });
                y++;
            }
            int maxEnergy = 0;
            int maxX = grid.Max(m => m.X);
            int maxY = grid.Max(m => m.Y);
            for (y = 0; y <= maxY; y++)
            {
                grid.Where(w => w.IsEnergized).ToList().ForEach(e => { e.IsEnergized = false; e.EnergizedByList = new List<string>(); });
                Day16MoveLight(grid, grid.First(w => w.X == 0 && w.Y == y), "east");
                int energy = grid.Count(c => c.IsEnergized);
                if (energy > maxEnergy)
                    maxEnergy = energy;
                grid.Where(w => w.IsEnergized).ToList().ForEach(e => { e.IsEnergized = false; e.EnergizedByList = new List<string>(); });
                Day16MoveLight(grid, grid.First(w => w.X == maxX && w.Y == y), "west");
                energy = grid.Count(c => c.IsEnergized);
                if (energy > maxEnergy)
                    maxEnergy = energy;
            }
            for (x = 0; x <= maxX; x++)
            {
                grid.Where(w => w.IsEnergized).ToList().ForEach(e => { e.IsEnergized = false; e.EnergizedByList = new List<string>(); });
                Day16MoveLight(grid, grid.First(w => w.X == x && w.Y == 0), "south");
                int energy = grid.Count(c => c.IsEnergized);
                if (energy > maxEnergy)
                    maxEnergy = energy;
                grid.Where(w => w.IsEnergized).ToList().ForEach(e => { e.IsEnergized = false; e.EnergizedByList = new List<string>(); });
                Day16MoveLight(grid, grid.First(w => w.X == x && w.Y == maxY), "north");
                energy = grid.Count(c => c.IsEnergized);
                if (energy > maxEnergy)
                    maxEnergy = energy;
            }
            Debug.WriteLine(maxEnergy); // 7315
        }

        private void Day16MoveLight(List<Day16Position> grid, Day16Position currentPos, string direction)
        {
            while (true)
            {
                if (currentPos == null)
                    return;
                if (currentPos.IsEnergized && currentPos.EnergizedByList.Contains(direction))
                    return;
                currentPos.IsEnergized = true;
                currentPos.EnergizedByList.Add(direction);
                if (currentPos.Type == '.')
                {
                    if (direction == "east")
                        currentPos = grid.FirstOrDefault(w => w.X == currentPos.X + 1 && w.Y == currentPos.Y);
                    else if (direction == "west")
                        currentPos = grid.FirstOrDefault(w => w.X == currentPos.X - 1 && w.Y == currentPos.Y);
                    else if (direction == "south")
                        currentPos = grid.FirstOrDefault(w => w.X == currentPos.X && w.Y == currentPos.Y + 1);
                    else
                        currentPos = grid.FirstOrDefault(w => w.X == currentPos.X && w.Y == currentPos.Y - 1);
                }
                else if (currentPos.Type == '\\')
                {
                    if (direction == "east")
                    {
                        currentPos = grid.FirstOrDefault(w => w.X == currentPos.X && w.Y == currentPos.Y + 1);
                        direction = "south";
                    }
                    else if (direction == "west")
                    {
                        currentPos = grid.FirstOrDefault(w => w.X == currentPos.X && w.Y == currentPos.Y - 1);
                        direction = "north";
                    }
                    else if (direction == "south")
                    {
                        currentPos = grid.FirstOrDefault(w => w.X == currentPos.X + 1 && w.Y == currentPos.Y);
                        direction = "east";
                    }
                    else
                    {
                        currentPos = grid.FirstOrDefault(w => w.X == currentPos.X - 1 && w.Y == currentPos.Y);
                        direction = "west";
                    }
                }
                else if (currentPos.Type == '/')
                {
                    if (direction == "east")
                    {
                        currentPos = grid.FirstOrDefault(w => w.X == currentPos.X && w.Y == currentPos.Y - 1);
                        direction = "north";
                    }
                    else if (direction == "west")
                    {
                        currentPos = grid.FirstOrDefault(w => w.X == currentPos.X && w.Y == currentPos.Y + 1);
                        direction = "south";
                    }
                    else if (direction == "south")
                    {
                        currentPos = grid.FirstOrDefault(w => w.X == currentPos.X - 1 && w.Y == currentPos.Y);
                        direction = "west";
                    }
                    else
                    {
                        currentPos = grid.FirstOrDefault(w => w.X == currentPos.X + 1 && w.Y == currentPos.Y);
                        direction = "east";
                    }
                }
                else if (currentPos.Type == '-')
                {
                    if (direction == "east")
                        currentPos = grid.FirstOrDefault(w => w.X == currentPos.X + 1 && w.Y == currentPos.Y);
                    else if (direction == "west")
                        currentPos = grid.FirstOrDefault(w => w.X == currentPos.X - 1 && w.Y == currentPos.Y);
                    else
                    {
                        Day16MoveLight(grid, grid.FirstOrDefault(w => w.X == currentPos.X + 1 && w.Y == currentPos.Y), "east");
                        Day16MoveLight(grid, grid.FirstOrDefault(w => w.X == currentPos.X - 1 && w.Y == currentPos.Y), "west");
                    }
                }
                else if (currentPos.Type == '|')
                {
                    if (direction == "south")
                        currentPos = grid.FirstOrDefault(w => w.X == currentPos.X && w.Y == currentPos.Y + 1);
                    else if (direction == "north")
                        currentPos = grid.FirstOrDefault(w => w.X == currentPos.X && w.Y == currentPos.Y - 1);
                    else
                    {
                        Day16MoveLight(grid, grid.FirstOrDefault(w => w.X == currentPos.X && w.Y == currentPos.Y - 1), "north");
                        Day16MoveLight(grid, grid.FirstOrDefault(w => w.X == currentPos.X && w.Y == currentPos.Y + 1), "south");
                    }
                }
            }
        }

        private void Day16Print(List<Day16Position> grid)
        {
            for (int y = 0; y <= grid.Max(m => m.Y); y++)
            {
                string line = string.Empty;
                for (int x = 0; x <= grid.Max(m => m.X); x++)
                {
                    Day16Position pos = grid.First(w => w.X == x && w.Y == y);
                    line += pos.IsEnergized ? "#" : pos.Type;
                }
                Debug.WriteLine(line);
            }
        }

        private class Day16Position
        {
            public int X { get; set; }
            public int Y { get; set; }
            public char Type { get; set; }
            public bool IsEnergized { get; set; }
            public List<string> EnergizedByList = new List<string>();
        }

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

        private int Day17CalculateMinHeatLoss(List<Day17Position> gridList, short minMoveDistance, short maxMoveDistance)
        {
            PriorityQueue<Day17QueueItem, int> queue = new();
            HashSet<string> tested = new HashSet<string>();
            Dictionary<string, Day17Position> grid = new Dictionary<string, Day17Position>();
            gridList.ForEach(e => grid.Add(e.X + "-" + e.Y, e));
            queue.Enqueue(new Day17QueueItem { Pos = grid.First(w => w.Key == "0-0").Value }, 0);
            int maxX = gridList.Max(m => m.X);
            int maxY = gridList.Max(m => m.Y);
            int loops = 0, dummy;
            int minHeatLoss = int.MaxValue;
            Day17QueueItem currentPos;
            while (queue.TryDequeue(out currentPos, out dummy))
            {
                loops++;
                if (currentPos == null)
                    return minHeatLoss;
                if (currentPos.Pos.X == maxX && currentPos.Pos.Y == maxY)
                {
                    if (minHeatLoss > currentPos.CurrentHeatLoss && (currentPos.East + currentPos.West + currentPos.North + currentPos.South) >= minMoveDistance)
                        minHeatLoss = currentPos.CurrentHeatLoss;
                    continue;
                }
                if (currentPos.East < maxMoveDistance && currentPos.West == 0 && ((currentPos.North + currentPos.South) >= minMoveDistance || currentPos.East > 0 || (currentPos.Pos.X + currentPos.Pos.Y) == 0))
                {
                    Day17Position pos;
                    if (grid.TryGetValue(currentPos.Pos.X + 1 + "-" + currentPos.Pos.Y, out pos) && currentPos.CurrentHeatLoss + pos.HeatLoss < minHeatLoss)
                    {
                        string key = pos.X + " " + pos.Y + " " + currentPos.East + 1 + " " + 0 + " " + 0 + " " + 0;
                        if (!tested.Contains(key))
                        {
                            Day17QueueItem newItem = new Day17QueueItem { CurrentHeatLoss = currentPos.CurrentHeatLoss + pos.HeatLoss, Priority = maxX + maxY - pos.X - pos.Y + currentPos.CurrentHeatLoss + pos.HeatLoss, Pos = pos, East = currentPos.East + 1, West = 0, North = 0, South = 0 };
                            queue.Enqueue(newItem, newItem.Priority);
                            tested.Add(key);
                        }
                    }
                }
                if (currentPos.West < maxMoveDistance && currentPos.East == 0 && ((currentPos.North + currentPos.South) >= minMoveDistance || currentPos.West > 0))
                {
                    Day17Position pos;
                    if (grid.TryGetValue(currentPos.Pos.X - 1 + "-" + currentPos.Pos.Y, out pos) && currentPos.CurrentHeatLoss + pos.HeatLoss < minHeatLoss)
                    {
                        string key = pos.X + " " + pos.Y + " " + 0 + " " + currentPos.West + 1 + " " + 0 + " " + 0;
                        if (!tested.Contains(key))
                        {
                            Day17QueueItem newItem = new Day17QueueItem { CurrentHeatLoss = currentPos.CurrentHeatLoss + pos.HeatLoss, Priority = maxX + maxY - pos.X - pos.Y + currentPos.CurrentHeatLoss + pos.HeatLoss, Pos = pos, East = 0, West = currentPos.West + 1, North = 0, South = 0 };
                            queue.Enqueue(newItem, newItem.Priority);
                            tested.Add(key);
                        }
                    }
                }
                if (currentPos.North < maxMoveDistance && currentPos.South == 0 && ((currentPos.East + currentPos.West) >= minMoveDistance || currentPos.North > 0))
                {
                    Day17Position pos;
                    if (grid.TryGetValue(currentPos.Pos.X + "-" + (currentPos.Pos.Y - 1), out pos) && currentPos.CurrentHeatLoss + pos.HeatLoss < minHeatLoss)
                    {
                        string key = pos.X + " " + pos.Y + " " + 0 + " " + 0 + " " + currentPos.North + 1 + " " + 0;
                        if (!tested.Contains(key))
                        {
                            Day17QueueItem newItem = new Day17QueueItem { CurrentHeatLoss = currentPos.CurrentHeatLoss + pos.HeatLoss, Priority = maxX + maxY - pos.X - pos.Y + currentPos.CurrentHeatLoss + pos.HeatLoss, Pos = pos, East = 0, West = 0, North = currentPos.North + 1, South = 0 };
                            queue.Enqueue(newItem, newItem.Priority);
                            tested.Add(key);
                        }
                    }
                }
                if (currentPos.South < maxMoveDistance && currentPos.North == 0 && ((currentPos.East + currentPos.West) >= minMoveDistance || currentPos.South > 0 || (currentPos.Pos.X + currentPos.Pos.Y) == 0))
                {
                    Day17Position pos;
                    if (grid.TryGetValue(currentPos.Pos.X + "-" + (currentPos.Pos.Y + 1), out pos) && currentPos.CurrentHeatLoss + pos.HeatLoss < minHeatLoss)
                    {
                        string key = pos.X + " " + pos.Y + " " + 0 + " " + 0 + " " + 0 + " " + currentPos.South + 1;
                        if (!tested.Contains(key))
                        {
                            Day17QueueItem newItem = new Day17QueueItem { CurrentHeatLoss = currentPos.CurrentHeatLoss + pos.HeatLoss, Priority = maxX + maxY - pos.X - pos.Y + currentPos.CurrentHeatLoss + pos.HeatLoss, Pos = pos, East = 0, West = 0, North = 0, South = currentPos.South + 1 };
                            queue.Enqueue(newItem, newItem.Priority);
                            tested.Add(key);
                        }
                    }
                }
            }
            return minHeatLoss;
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
        public void Day18_1()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day18.txt").ToList();
            List<Day18Instruction> instructionList = new List<Day18Instruction>();
            foreach (string input in inputList)
            {
                List<string> inputSplit = input.Split(' ').ToList();
                instructionList.Add(new Day18Instruction { Direction = inputSplit.First(), Length = int.Parse(inputSplit[1]) });
            }
            List<Day18Position> grid = new List<Day18Position>();
            int x = 0, y = 0, index = 0;
            foreach (Day18Instruction instruction in instructionList)
            {
                if (instruction.Direction == "R")
                    for (int i = 1; i <= instruction.Length; i++)
                        grid.Add(new Day18Position { X = x++, Y = y, IsEdge = true, Index = index++ });
                else if (instruction.Direction == "L")
                    for (int i = 1; i <= instruction.Length; i++)
                        grid.Add(new Day18Position { X = x--, Y = y, IsEdge = true, Index = index++ });
                else if (instruction.Direction == "U")
                    for (int i = 1; i <= instruction.Length; i++)
                        grid.Add(new Day18Position { X = x, Y = y--, IsEdge = true, Index = index++ });
                else if (instruction.Direction == "D")
                    for (int i = 1; i <= instruction.Length; i++)
                        grid.Add(new Day18Position { X = x, Y = y++, IsEdge = true, Index = index++ });
            }
            Day18FillPool(grid, "west", "north"); // actual data
            //Day18FillPool(grid, "east", "south"); // sample data
            Debug.WriteLine(grid.Count());
        }

        [TestMethod]
        public void Day18_2()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day18.txt").ToList();
            List<Day18Instruction> instructionList = new List<Day18Instruction>();
            foreach (string input in inputList)
            {
                List<string> inputSplit = input.Split(' ').ToList();
                instructionList.Add(new Day18Instruction
                {
                    Direction = inputSplit[2][7] == '0' ? "R" : inputSplit[2][7] == '1' ? "D" : inputSplit[2][7] == '2' ? "L" : "U",
                    Length = Convert.ToInt32(inputSplit[2].Substring(2, 5), 16)
                });
            }
            List<Day18PositionLine> grid = new List<Day18PositionLine>();
            int x = 0, y = 0, currentDirection = 2, currentFillDirection = 3; // adjust these depending on input data starting position
            foreach (Day18Instruction instruction in instructionList)
            {
                if (instruction.Direction == "R")
                {
                    currentFillDirection = Day18CalcFillDirection(currentDirection, 2, currentFillDirection);
                    currentDirection = 2;
                    grid.Add(new Day18PositionLine { StartX = x, StartY = y, EndX = x + instruction.Length, EndY = y, Length = instruction.Length, IsHorizontal = true, FillDirection = currentFillDirection });
                    x += instruction.Length;
                }
                else if (instruction.Direction == "L")
                {
                    currentFillDirection = Day18CalcFillDirection(currentDirection, 0, currentFillDirection);
                    currentDirection = 0;
                    grid.Add(new Day18PositionLine { StartX = x - instruction.Length, StartY = y, EndX = x, EndY = y, Length = instruction.Length, IsHorizontal = true, FillDirection = currentFillDirection });
                    x -= instruction.Length;
                }
                else if (instruction.Direction == "U")
                {
                    currentFillDirection = Day18CalcFillDirection(currentDirection, 1, currentFillDirection);
                    currentDirection = 1;
                    grid.Add(new Day18PositionLine { StartX = x, StartY = y - instruction.Length, EndX = x, EndY = y, Length = instruction.Length, IsHorizontal = false, FillDirection = currentFillDirection });
                    y -= instruction.Length;
                }
                else if (instruction.Direction == "D")
                {
                    currentFillDirection = Day18CalcFillDirection(currentDirection, 3, currentFillDirection);
                    currentDirection = 3;
                    grid.Add(new Day18PositionLine { StartX = x, StartY = y, EndX = x, EndY = y + instruction.Length, Length = instruction.Length, IsHorizontal = false, FillDirection = currentFillDirection });
                    y += instruction.Length;
                }
            }
            long fillSum = Day18CalcFill(grid);
            Debug.WriteLine(fillSum);
        }

        private void Day18FillPool(List<Day18Position> grid, string startMoveDirection, string startFillDirection)
        {
            List<string> directionList = new List<string> { "west", "north", "east", "south" };
            HashSet<string> fillList = new HashSet<string>();
            Day18Position edge1 = grid.First(w => w.Index == 0);
            int fillDirection = directionList.IndexOf(startFillDirection);
            int moveDirection = directionList.IndexOf(startMoveDirection);
            int oldX, oldY;
            while (edge1 != null)
            {
                Day18FillLine(grid, edge1, fillDirection, fillList);
                oldX = edge1.X;
                oldY = edge1.Y;
                edge1 = grid.FirstOrDefault(w => w.Index == edge1.Index + 1);
                if (edge1 != null)
                {
                    int newMoveDirection = directionList.IndexOf(edge1.X > oldX ? "east" : edge1.X < oldX ? "west" : edge1.Y > oldY ? "south" : "north");
                    fillDirection = Day18CalcFillDirection(moveDirection, newMoveDirection, fillDirection);
                    if (moveDirection != newMoveDirection)
                        Day18FillLine(grid, grid.First(w => w.Index == edge1.Index - 1), fillDirection, fillList);
                    moveDirection = newMoveDirection;
                }
            }
        }

        private void Day18FillLine(List<Day18Position> grid, Day18Position edge1, int fillDirection, HashSet<string> fillList)
        {
            Day18Position edge2 = null;
            if (fillDirection == 2) // east
            {
                edge2 = grid.Where(w => w.X > edge1.X && w.Y == edge1.Y && w.IsEdge).OrderBy(o => o.X).FirstOrDefault();
                for (int i = edge1.X + 1; i < edge2.X; i++)
                    if (!fillList.Contains(i + "-" + edge1.Y))
                    {
                        grid.Add(new Day18Position { X = i, Y = edge1.Y, IsEdge = false });
                        fillList.Add(i + "-" + edge1.Y);
                    }
            }
            else if (fillDirection == 0) // west
            {
                edge2 = grid.Where(w => w.X < edge1.X && w.Y == edge1.Y && w.IsEdge).OrderByDescending(o => o.X).FirstOrDefault();
                for (int i = edge1.X - 1; i > edge2.X; i--)
                    if (!fillList.Contains(i + "-" + edge1.Y))
                    {
                        grid.Add(new Day18Position { X = i, Y = edge1.Y, IsEdge = false });
                        fillList.Add(i + "-" + edge1.Y);
                    }
            }
            else if (fillDirection == 1) // north
            {
                edge2 = grid.Where(w => w.X == edge1.X && w.Y < edge1.Y && w.IsEdge).OrderByDescending(o => o.Y).FirstOrDefault();
                for (int i = edge1.Y - 1; i > edge2.Y; i--)
                    if (!fillList.Contains(edge1.X + "-" + i))
                    {
                        grid.Add(new Day18Position { X = edge1.X, Y = i, IsEdge = false });
                        fillList.Add(edge1.X + "-" + i);
                    }
            }
            else if (fillDirection == 3) // south
            {
                edge2 = grid.Where(w => w.X == edge1.X && w.Y > edge1.Y && w.IsEdge).OrderBy(o => o.Y).FirstOrDefault();
                for (int i = edge1.Y + 1; i < edge2.Y; i++)
                    if (!fillList.Contains(edge1.X + "-" + i))
                    {
                        grid.Add(new Day18Position { X = edge1.X, Y = i, IsEdge = false });
                        fillList.Add(edge1.X + "-" + i);
                    }
            }
        }

        private long Day18CalcFill(List<Day18PositionLine> grid)
        {
            int minY = grid.Min(m => m.StartY);
            int maxY = grid.Max(m => m.StartY);
            Day18PositionLine edge1 = null;
            Day18PositionLine edge2 = null;
            long fillSum = 0;
            long tempFill = 0;
            for (int y = minY; y <= maxY; y++)
            {
                if (y % 100000 == 0)
                    Debug.WriteLine("Row: " + y + " fillSum: " + fillSum + " minY: " + minY + " maxY: " + maxY);
                edge1 = grid.Where(w => w.StartY <= y && w.EndY >= y).OrderBy(o => o.StartX).ThenByDescending(t => t.EndX).First();
                while (edge1 != null)
                {
                    if (edge1.IsHorizontal)
                    {
                        tempFill = edge1.EndX - edge1.StartX + 1;
                        if (grid.Any(w => w.EndX == edge1.EndX && w.EndY == edge1.EndY && !w.IsHorizontal))
                        {
                            if (edge1.FillDirection == 3)
                            {
                                edge1 = grid.Where(w => w.EndX == edge1.EndX && w.EndY == edge1.EndY && !w.IsHorizontal).First();
                                tempFill--;
                            }
                            else
                                edge1 = grid.Where(w => w.StartY <= y && w.EndY >= y && w.StartX > edge1.EndX).OrderBy(o => o.StartX).ThenByDescending(t => t.EndX).FirstOrDefault();
                        }
                        else
                        {
                            if (edge1.FillDirection == 1)
                            {
                                edge1 = grid.Where(w => w.StartX == edge1.EndX && w.StartY == edge1.EndY && !w.IsHorizontal).First();
                                tempFill--;
                            }
                            else
                                edge1 = grid.Where(w => w.StartY <= y && w.EndY >= y && w.StartX > edge1.EndX).OrderBy(o => o.StartX).ThenByDescending(t => t.EndX).FirstOrDefault();
                        }
                        fillSum += tempFill;
                    }
                    else
                    {
                        edge2 = grid.Where(w => w.StartY <= y && w.EndY >= y && w.StartX > edge1.StartX).OrderBy(o => o.StartX).ThenByDescending(t => t.EndX).First();
                        if (edge2.IsHorizontal)
                        {
                            fillSum += edge2.StartX - edge1.StartX;
                            edge1 = edge2;
                        }
                        else
                        {
                            fillSum += edge2.StartX - edge1.StartX + 1;
                            edge1 = grid.Where(w => w.StartY <= y && w.EndY >= y && w.StartX > edge2.StartX).OrderBy(o => o.StartX).ThenByDescending(t => t.EndX).FirstOrDefault();
                        }
                    }
                }
            }
            return fillSum;
        }

        private int Day18CalcFillDirection(int currentDirection, int newDirection, int currentFillDirection)
        {
            int fillDirection = currentFillDirection;
            if (currentDirection == 0 && newDirection == 3 || currentDirection == 1 && newDirection == 0
                || currentDirection == 2 && newDirection == 1 || currentDirection == 3 && newDirection == 2)
                fillDirection--;
            else if (currentDirection == 0 && newDirection == 1 || currentDirection == 1 && newDirection == 2
                || currentDirection == 2 && newDirection == 3 || currentDirection == 3 && newDirection == 0)
                fillDirection++;
            fillDirection = (fillDirection < 0 ? 4 + fillDirection : fillDirection) % 4;
            return fillDirection;
        }

        private void Day18PrintGrid(List<Day18Position> grid)
        {
            for (int y = grid.Min(m => m.Y); y <= grid.Max(m => m.Y); y++)
            {
                string line = string.Empty;
                for (int x = grid.Min(m => m.X); x <= grid.Max(m => m.X); x++)
                {
                    Day18Position pos = grid.FirstOrDefault(w => w.X == x && w.Y == y);
                    //line += pos != null ? "#" : ".";
                    line += pos != null ? pos.Index == 0 ? "F" : "#" : ".";
                    //line += pos != null ? pos.Index.ToString().PadRight(2) : ".".PadRight(2);
                }
                Debug.WriteLine(line);
            }
        }

        private class Day18Instruction
        {
            public string Direction { get; set; }
            public int Length { get; set; }
        }

        private class Day18Position
        {
            public int Y { get; set; }
            public int X { get; set; }
            public int Index { get; set; }
            public bool IsEdge { get; set; }
        }

        private class Day18PositionLine
        {
            public bool IsHorizontal { get; set; }
            public int StartY { get; set; }
            public int StartX { get; set; }
            public int EndY { get; set; }
            public int EndX { get; set; }
            public int Length { get; set; }
            public int FillDirection { get; set; }
        }
    }
}