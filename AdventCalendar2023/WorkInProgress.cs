using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace AdventCalendar2023
{
    [TestClass]
    public class WorkInProgress
    {
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
                    //Debug.WriteLine(test);
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
        public void Day12_1_3()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<string> inputList = File.ReadAllLines(@"Input\Day12Test.txt").ToList();
            List<Day12Group> groups = new List<Day12Group>();
            foreach (string groupInput in inputList)
            {
                List<string> splitInput = groupInput.Split(' ').ToList();
                Day12Group group = new Day12Group { DamageList = new List<int>(), Springs = splitInput[0] };
                group.DamageList = splitInput[1].Split(',').Select(int.Parse).ToList();
                group.UnknownList = splitInput[0].Split('.').Where(w => !string.IsNullOrWhiteSpace(w)).Select(s => s.Count()).ToList();
                groups.Add(group);
            }
            int arrangements = 0;
            int tests = 0;
            int groupCount = groups.Count();
            int currentGroup = 0;
            bool firstGroup = true;
            foreach (Day12Group group in groups)
            {
                int maxLength = group.Springs.Length;
                string test = string.Empty;
                int[] operationalList = new int[group.DamageList.Count + 1];
                for (int i = 0; i < operationalList.Length; i++)
                    operationalList[i] = 1;
                operationalList[0] = group.Springs.IndexOfAny(new char[] { '?', '#' });
                int maxOperationalStart = group.Springs.IndexOf('#');
                maxOperationalStart = maxOperationalStart == -1 ? group.Springs.Count() : maxOperationalStart;
                int maxOperational = group.Springs.Length - group.DamageList.Sum();
                //int minEndingOperationalSpring = group.Springs.Length - (group.Springs.LastIndexOfAny(new char[] { '?', '#' }) + 1);
                //maxOperational -= minEndingOperationalSpring;
                int oIndex = operationalList.Length - 1;
                Day12PrefillOperationalList(group, operationalList);
                int[] operationalPrefillList = new int[operationalList.Count()];
                for (int i = 0; i < operationalList.Count(); i++)
                    operationalPrefillList[i] = operationalList[i];
                //Debug.WriteLine("operationalList: " + string.Join(',', operationalList));
                while (true)
                {
                    tests++;
                    test = string.Empty;
                    for (int i = 0; i < group.DamageList.Count; i++)
                        test += "".PadRight(operationalList[i], '.') + "".PadRight(group.DamageList[i], '#');
                    operationalList[oIndex] = maxLength - test.Length;
                    test += "".PadRight(operationalList[operationalList.Count() - 1], '.'); // test.PadRight(minEndingOperationalSpring + test.Length, '.');
                    bool isValid = false;
                    if (test.Length <= maxLength)
                    {
                        isValid = true;
                        test = test.PadRight(maxLength, '.');
                        for (int i = 0; i < group.Springs.Length; i++)
                        {
                            if (test[i] == group.Springs[i] || ((test[i] == '#' || test[i] == '.') && group.Springs[i] == '?'))
                                continue;
                            else
                            {
                                isValid = false;
                                break;
                            }
                        }
                        if (isValid)
                            arrangements++;
                    }
                    //if (firstGroup)

                    //Debug.WriteLine(test + (isValid ? " M" : ""));
                    operationalList[oIndex]++;
                    while (operationalList.Sum() > maxOperational && oIndex > 0)
                    {
                        for (int k = oIndex; k < operationalList.Length; k++)
                            operationalList[k] = operationalPrefillList[k];
                        oIndex--;
                        operationalList[oIndex]++;
                    }
                    if (operationalList.Sum() > maxOperational || operationalList[0] > maxOperationalStart)
                        break;
                    oIndex = operationalList.Length - 1;
                }
                currentGroup++;
                firstGroup = false;
                Debug.WriteLine("Group: " + currentGroup + " Arrangements: " + arrangements + " Tests: " + tests + " Elapsed: " + stopwatch.Elapsed);
            }
            Debug.WriteLine("Arrangements: " + arrangements + " Tests: " + +tests + " Elapsed: " + stopwatch.Elapsed);
        }

        [TestMethod]
        public void Day12_2()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<string> inputList = File.ReadAllLines(@"Input\Day12Test.txt").ToList();
            List<Day12Group> groups = new List<Day12Group>();
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
                Day12Group group = new Day12Group { DamageList = new List<int>(), Springs = springInput };
                group.DamageList = damageInput.Split(',').Select(int.Parse).ToList();
                group.UnknownList = springInput.Split('.').Where(w => !string.IsNullOrWhiteSpace(w)).Select(s => s.Count()).ToList();
                groups.Add(group);
            }
            int arrangements = 0;
            long tests = 0;
            int groupCount = groups.Count();
            int currentGroup = 0;
            bool firstGroup = true;
            foreach (Day12Group group in groups)
            {
                int maxLength = group.Springs.Length;
                string test = string.Empty;
                int[] operationalList = new int[group.DamageList.Count + 1];
                for (int i = 0; i < operationalList.Length; i++)
                    operationalList[i] = 1;
                operationalList[0] = group.Springs.IndexOfAny(new char[] { '?', '#' });
                int maxOperationalStart = group.Springs.IndexOf('#');
                maxOperationalStart = maxOperationalStart == -1 ? group.Springs.Count() : maxOperationalStart;
                int maxOperational = group.Springs.Length - group.DamageList.Sum();
                int minEndingOperationalSpring = group.Springs.Length - (group.Springs.LastIndexOfAny(new char[] { '?', '#' }) + 1);
                maxOperational -= minEndingOperationalSpring;
                int oIndex = operationalList.Length - 1;
                Day12PrefillOperationalList(group, operationalList);
                while (true)
                {
                    tests++;
                    test = string.Empty;
                    for (int i = 0; i < group.DamageList.Count - 1; i++)
                        test += "".PadRight(operationalList[i], '.') + "".PadRight(group.DamageList[i], '#');
                    test = test.PadRight(operationalList[group.DamageList.Count - 1], '.'); // test.PadRight(minEndingOperationalSpring + test.Length, '.');
                    if (test.Length <= maxLength)
                    {
                        test = test.PadRight(maxLength, '.');
                        bool isValid = true;
                        for (int i = 0; i < group.Springs.Length; i++)
                        {
                            if (test[i] == group.Springs[i] || ((test[i] == '#' || test[i] == '.') && group.Springs[i] == '?'))
                                continue;
                            else
                            {
                                isValid = false;
                                break;
                            }
                        }
                        if (isValid)
                            arrangements++;
                    }
                    //if (firstGroup)
                    //    Debug.WriteLine(test);
                    operationalList[oIndex]++;
                    while (operationalList.Sum() > maxOperational && oIndex > 0)
                    {
                        for (int k = oIndex; k < operationalList.Length; k++)
                            operationalList[k] = 1;
                        oIndex--;
                        operationalList[oIndex]++;
                    }
                    if (operationalList.Sum() > maxOperational || operationalList[0] > maxOperationalStart)
                        break;
                    oIndex = operationalList.Length - 1;
                    if (tests % 10000000 == 0)
                        Debug.WriteLine("Ongoing Group: " + currentGroup + " Arrangements: " + arrangements + " Tests: " + tests + " Elapsed: " + stopwatch.Elapsed + " String: " + test);
                }
                currentGroup++;
                firstGroup = false;
                Debug.WriteLine("Group: " + currentGroup + " Arrangements: " + arrangements + " Tests: " + tests + " Elapsed: " + stopwatch.Elapsed);
            }
            Debug.WriteLine("Arrangements: " + arrangements + " Tests: " + +tests + " Elapsed: " + stopwatch.Elapsed);
        }

        [TestMethod]
        public void Day12_2_1()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<string> inputList = File.ReadAllLines(@"Input\Day12Test.txt").ToList();
            List<Day12Group> groups = new List<Day12Group>();
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
                Day12Group group = new Day12Group { DamageList = new List<int>(), Springs = springInput };
                group.DamageList = damageInput.Split(',').Select(int.Parse).ToList();
                group.UnknownList = springInput.Split('.').Where(w => !string.IsNullOrWhiteSpace(w)).Select(s => s.Count()).ToList();
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
                    //Debug.WriteLine(test);
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
                    if (tests % 10000000 == 0)
                        Debug.WriteLine("Ongoing Group: " + currentGroup + " Arrangements: " + arrangements + " Tests: " + tests + " Elapsed: " + stopwatch.Elapsed + " String: " + test);
                }
                currentGroup++;
                Debug.WriteLine("Group: " + currentGroup + " Arrangements: " + arrangements + " Tests: " + tests);
            }
            Debug.WriteLine(arrangements + " " + tests);
            Debug.WriteLine("Arrangements: " + arrangements + " Tests: " + +tests + " Elapsed: " + stopwatch.Elapsed);
        }

        private void Day12PrefillOperationalList(Day12Group group, int[] operationalList)
        {
            int unknownIndex = 0;
            int currentOperationalCount = 0;
            int operationalIndex = 0;
            for (int i = 0; i < group.Springs.Count(); i++)
            {
                char c = group.Springs[i];
                if (c == '.')
                    currentOperationalCount++;
                if (c == '?' || c == '#')
                {
                    operationalList[operationalIndex] = currentOperationalCount;
                    currentOperationalCount = 0;
                    if (group.UnknownList[unknownIndex] <= 2)
                        i += (group.UnknownList[unknownIndex] - 1);
                    else
                        break;
                    unknownIndex++;
                    operationalIndex++;
                    if (operationalIndex >= operationalList.Count())
                        break;
                }
            }
            unknownIndex = group.UnknownList.Count() - 1;
            currentOperationalCount = 0;
            operationalIndex = operationalList.Count() - 1;
            for (int i = group.Springs.Count() - 1; i >= 0; i--)
            {
                char c = group.Springs[i];
                if (c == '.')
                    currentOperationalCount++;
                if (c == '?' || c == '#')
                {
                    operationalList[operationalIndex] = currentOperationalCount;
                    currentOperationalCount = 0;
                    if (group.UnknownList[unknownIndex] <= 2)
                        i -= (group.UnknownList[unknownIndex] - 1);
                    else
                        break;
                    unknownIndex--;
                    operationalIndex--;
                    if (operationalIndex < 0)
                        break;
                }
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

        private class Day12Group
        {
            public string Springs { get; set; }
            //public List<Day12PartGroup> PartGroupList { get; set; }
            public List<int> DamageList { get; set; }
            public List<int> UnknownList { get; set; }
        }

        //private class Day12PartGroup
        //{
        //    public List<char> Springs { get; set; }
        //}

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