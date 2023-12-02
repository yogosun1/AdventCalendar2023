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
    }
}