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
                string first = lineParsed.Where(w => char.IsDigit(w)).First().ToString();
                string last = lineParsed.Where(w => char.IsDigit(w)).Last().ToString();
                int number = int.Parse(first + last);
                sum += number;
            }
        }
    }
}