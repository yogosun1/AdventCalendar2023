using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using System;
using System.Collections.Generic;
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
    public class WorkInProgressDay12
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

        //[TestMethod]
        //public void Day12_1_3()
        //{
        //    Stopwatch stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    List<string> inputList = File.ReadAllLines(@"Input\Day12Test.txt").ToList();
        //    List<Day12Group> groups = new List<Day12Group>();
        //    foreach (string groupInput in inputList)
        //    {
        //        List<string> splitInput = groupInput.Split(' ').ToList();
        //        Day12Group group = new Day12Group { DamageList = new List<int>(), Springs = splitInput[0] };
        //        group.DamageList = splitInput[1].Split(',').Select(int.Parse).ToList();
        //        group.UnknownList = splitInput[0].Split('.').Where(w => !string.IsNullOrWhiteSpace(w)).Select(s => s.Count()).ToList();
        //        groups.Add(group);
        //    }
        //    int arrangements = 0;
        //    int tests = 0;
        //    int groupCount = groups.Count();
        //    int currentGroup = 0;
        //    bool firstGroup = true;
        //    foreach (Day12Group group in groups)
        //    {
        //        int maxLength = group.Springs.Length;
        //        string test = string.Empty;
        //        int[] operationalList = new int[group.DamageList.Count + 1];
        //        for (int i = 0; i < operationalList.Length; i++)
        //            operationalList[i] = 1;
        //        operationalList[0] = group.Springs.IndexOfAny(new char[] { '?', '#' });
        //        int maxOperationalStart = group.Springs.IndexOf('#');
        //        maxOperationalStart = maxOperationalStart == -1 ? group.Springs.Count() : maxOperationalStart;
        //        int maxOperational = group.Springs.Length - group.DamageList.Sum();
        //        //int minEndingOperationalSpring = group.Springs.Length - (group.Springs.LastIndexOfAny(new char[] { '?', '#' }) + 1);
        //        //maxOperational -= minEndingOperationalSpring;
        //        int oIndex = operationalList.Length - 1;
        //        Day12PrefillOperationalList(group, operationalList);
        //        int[] operationalPrefillList = new int[operationalList.Count()];
        //        for (int i = 0; i < operationalList.Count(); i++)
        //            operationalPrefillList[i] = operationalList[i];
        //        //Debug.WriteLine("operationalList: " + string.Join(',', operationalList));
        //        while (true)
        //        {
        //            tests++;
        //            test = string.Empty;
        //            for (int i = 0; i < group.DamageList.Count; i++)
        //                test += "".PadRight(operationalList[i], '.') + "".PadRight(group.DamageList[i], '#');
        //            operationalList[oIndex] = maxLength - test.Length;
        //            test += "".PadRight(operationalList[operationalList.Count() - 1], '.'); // test.PadRight(minEndingOperationalSpring + test.Length, '.');
        //            bool isValid = false;
        //            if (test.Length <= maxLength)
        //            {
        //                isValid = true;
        //                test = test.PadRight(maxLength, '.');
        //                for (int i = 0; i < group.Springs.Length; i++)
        //                {
        //                    if (test[i] == group.Springs[i] || ((test[i] == '#' || test[i] == '.') && group.Springs[i] == '?'))
        //                        continue;
        //                    else
        //                    {
        //                        isValid = false;
        //                        break;
        //                    }
        //                }
        //                if (isValid)
        //                    arrangements++;
        //            }
        //            //if (firstGroup)

        //            //Debug.WriteLine(test + (isValid ? " M" : ""));
        //            operationalList[oIndex]++;
        //            while (operationalList.Sum() > maxOperational && oIndex > 0)
        //            {
        //                for (int k = oIndex; k < operationalList.Length; k++)
        //                    operationalList[k] = operationalPrefillList[k];
        //                oIndex--;
        //                operationalList[oIndex]++;
        //            }
        //            if (operationalList.Sum() > maxOperational || operationalList[0] > maxOperationalStart)
        //                break;
        //            oIndex = operationalList.Length - 1;
        //        }
        //        currentGroup++;
        //        firstGroup = false;
        //        Debug.WriteLine("Group: " + currentGroup + " Arrangements: " + arrangements + " Tests: " + tests + " Elapsed: " + stopwatch.Elapsed);
        //    }
        //    Debug.WriteLine("Arrangements: " + arrangements + " Tests: " + +tests + " Elapsed: " + stopwatch.Elapsed);
        //}

        //[TestMethod]
        //public void Day12_2()
        //{
        //    Stopwatch stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    List<string> inputList = File.ReadAllLines(@"Input\Day12Test.txt").ToList();
        //    List<Day12Group> groups = new List<Day12Group>();
        //    foreach (string groupInput in inputList)
        //    {
        //        List<string> splitInput = groupInput.Split(' ').ToList();
        //        string springInput = splitInput[0];
        //        string damageInput = splitInput[1];
        //        for (int i = 0; i < 4; i++)
        //        {
        //            springInput += "?" + splitInput[0];
        //            damageInput += "," + splitInput[1];
        //        }
        //        Day12Group group = new Day12Group { DamageList = new List<int>(), Springs = springInput };
        //        group.DamageList = damageInput.Split(',').Select(int.Parse).ToList();
        //        group.UnknownList = springInput.Split('.').Where(w => !string.IsNullOrWhiteSpace(w)).Select(s => s.Count()).ToList();
        //        groups.Add(group);
        //    }
        //    int arrangements = 0;
        //    long tests = 0;
        //    int groupCount = groups.Count();
        //    int currentGroup = 0;
        //    bool firstGroup = true;
        //    foreach (Day12Group group in groups)
        //    {
        //        int maxLength = group.Springs.Length;
        //        string test = string.Empty;
        //        int[] operationalList = new int[group.DamageList.Count + 1];
        //        for (int i = 0; i < operationalList.Length; i++)
        //            operationalList[i] = 1;
        //        operationalList[0] = group.Springs.IndexOfAny(new char[] { '?', '#' });
        //        int maxOperationalStart = group.Springs.IndexOf('#');
        //        maxOperationalStart = maxOperationalStart == -1 ? group.Springs.Count() : maxOperationalStart;
        //        int maxOperational = group.Springs.Length - group.DamageList.Sum();
        //        int minEndingOperationalSpring = group.Springs.Length - (group.Springs.LastIndexOfAny(new char[] { '?', '#' }) + 1);
        //        maxOperational -= minEndingOperationalSpring;
        //        int oIndex = operationalList.Length - 1;
        //        Day12PrefillOperationalList(group, operationalList);
        //        while (true)
        //        {
        //            tests++;
        //            test = string.Empty;
        //            for (int i = 0; i < group.DamageList.Count - 1; i++)
        //                test += "".PadRight(operationalList[i], '.') + "".PadRight(group.DamageList[i], '#');
        //            test = test.PadRight(operationalList[group.DamageList.Count - 1], '.'); // test.PadRight(minEndingOperationalSpring + test.Length, '.');
        //            if (test.Length <= maxLength)
        //            {
        //                test = test.PadRight(maxLength, '.');
        //                bool isValid = true;
        //                for (int i = 0; i < group.Springs.Length; i++)
        //                {
        //                    if (test[i] == group.Springs[i] || ((test[i] == '#' || test[i] == '.') && group.Springs[i] == '?'))
        //                        continue;
        //                    else
        //                    {
        //                        isValid = false;
        //                        break;
        //                    }
        //                }
        //                if (isValid)
        //                    arrangements++;
        //            }
        //            //if (firstGroup)
        //            //    Debug.WriteLine(test);
        //            operationalList[oIndex]++;
        //            while (operationalList.Sum() > maxOperational && oIndex > 0)
        //            {
        //                for (int k = oIndex; k < operationalList.Length; k++)
        //                    operationalList[k] = 1;
        //                oIndex--;
        //                operationalList[oIndex]++;
        //            }
        //            if (operationalList.Sum() > maxOperational || operationalList[0] > maxOperationalStart)
        //                break;
        //            oIndex = operationalList.Length - 1;
        //            if (tests % 10000000 == 0)
        //                Debug.WriteLine("Ongoing Group: " + currentGroup + " Arrangements: " + arrangements + " Tests: " + tests + " Elapsed: " + stopwatch.Elapsed + " String: " + test);
        //        }
        //        currentGroup++;
        //        firstGroup = false;
        //        Debug.WriteLine("Group: " + currentGroup + " Arrangements: " + arrangements + " Tests: " + tests + " Elapsed: " + stopwatch.Elapsed);
        //    }
        //    Debug.WriteLine("Arrangements: " + arrangements + " Tests: " + +tests + " Elapsed: " + stopwatch.Elapsed);
        //}

        [TestMethod]
        public void Day12_2_1()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<string> inputList = File.ReadAllLines(@"Input\Day12.txt").ToList();
            List<Day12Group> groups = new List<Day12Group>();
            foreach (string groupInput in inputList)
            {
                List<string> splitInput = groupInput.Split(' ').ToList();
                string springInput = splitInput[0];
                string damageInput = splitInput[1];
                //for (int i = 0; i < 4; i++)
                //{
                //    springInput += "?" + splitInput[0];
                //    damageInput += "," + splitInput[1];
                //}
                Day12Group group = new Day12Group { DamageList = new List<int>(), Springs = springInput };
                group.DamageList = damageInput.Split(',').Select(int.Parse).ToList();
                //group.UnknownList = springInput.Split('.').Where(w => !string.IsNullOrWhiteSpace(w)).Select(s => s.Count()).ToList();
                groups.Add(group);
            }
            long arrangements = 0;
            long tests = 0;
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
                bool isValid = false;
                while (true)
                {
                    test = string.Empty;
                    if (Day12IsValid2(group, operationalList))
                    {
                        tests++;
                        for (int i = 0; i < group.DamageList.Count; i++)
                            test += "".PadRight(operationalList[i], '.') + "".PadRight(group.DamageList[i], '#');
                        test = test.PadRight(minEndingOperationalSpring + test.Length, '.');
                        if (test.Length <= maxLength)
                        {
                            test = test.PadRight(maxLength, '.');
                            isValid = Day12IsValid(test, group.Springs);
                            if (isValid)
                                arrangements++;
                        }
                        //Debug.WriteLine(test + (isValid ? " M" : ""));
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
                    //if (tests % 10000000 == 0)
                    //    Debug.WriteLine("Ongoing Group: " + currentGroup + " Arrangements: " + arrangements + " Tests: " + tests + " Elapsed: " + stopwatch.Elapsed + " String: " + test);
                }
                currentGroup++;
                //File.AppendAllText(@"C:\Temp\AdventOfCoreDay12Test.txt", "Group: " + currentGroup + " Arrangements: " + arrangements + " Tests: " + tests + " Elapsed: " + stopwatch.Elapsed + " String: " + test);
                //Debug.WriteLine("Group: " + currentGroup + " Arrangements: " + arrangements + " Tests: " + tests + " Elapsed: " + stopwatch.Elapsed + " String: " + test);
            }
            Debug.WriteLine(arrangements + " " + tests);
            Debug.WriteLine("Arrangements: " + arrangements + " Tests: " + +tests + " Elapsed: " + stopwatch.Elapsed);
            //File.WriteAllText(@"C:\Temp\AdventOfCoreDay12Test.txt", "Arrangements: " + arrangements + " Tests: " + +tests + " Elapsed: " + stopwatch.Elapsed);
        }

        [TestMethod]
        public void Day12_2_2()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<string> inputList = File.ReadAllLines(@"Input\Day12.txt").ToList();
            List<Day12Group> groups = new List<Day12Group>();
            foreach (string groupInput in inputList)
            {
                List<string> splitInput = groupInput.Split(' ').ToList();
                string springInput = splitInput[0];
                string damageInput = splitInput[1];
                //for (int i = 0; i < 4; i++)
                //{
                //    springInput += "?" + splitInput[0];
                //    damageInput += "," + splitInput[1];
                //}
                Day12Group group = new Day12Group { DamageList = new List<int>(), Springs = springInput };
                group.DamageList = damageInput.Split(',').Select(int.Parse).ToList();
                //group.UnknownList = springInput.Split('.').Where(w => !string.IsNullOrWhiteSpace(w)).Select(s => s.Count()).ToList();
                groups.Add(group);
            }
            long arrangements = 0;
            long tests = 0;
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
                    if (Day12IsValid2(group, operationalList))
                        arrangements++;
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
                    //if (tests % 10000000 == 0)
                    //    Debug.WriteLine("Ongoing Group: " + currentGroup + " Arrangements: " + arrangements + " Tests: " + tests + " Elapsed: " + stopwatch.Elapsed + " String: " + test);
                }
                currentGroup++;
                //File.AppendAllText(@"C:\Temp\AdventOfCoreDay12Test.txt", "Group: " + currentGroup + " Arrangements: " + arrangements + " Tests: " + tests + " Elapsed: " + stopwatch.Elapsed + " String: " + test);
                //Debug.WriteLine("Group: " + currentGroup + " Arrangements: " + arrangements + " Tests: " + tests + " Elapsed: " + stopwatch.Elapsed + " String: " + test);
            }
            Debug.WriteLine(arrangements + " " + tests);
            Debug.WriteLine("Arrangements: " + arrangements + " Tests: " + +tests + " Elapsed: " + stopwatch.Elapsed);
            //File.WriteAllText(@"C:\Temp\AdventOfCoreDay12Test.txt", "Arrangements: " + arrangements + " Tests: " + +tests + " Elapsed: " + stopwatch.Elapsed);
        }

        [TestMethod]
        public void Day12_2_3Current()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day12.txt").ToList();
            List<Day12Group> groups = new List<Day12Group>();
            int i = 0, p = 0, k = 0;
            foreach (string groupInput in inputList)
            {
                List<string> splitInput = groupInput.Split(' ').ToList();
                string springInput = splitInput[0];
                string damageInput = splitInput[1];
                //for (i = 0; i < 4; i++)
                //{
                //    springInput += "?" + splitInput[0];
                //    damageInput += "," + splitInput[1];
                //}
                Day12Group group = new Day12Group { DamageList = new List<int>(), Springs = springInput, OriginalSprings = springInput };
                group.DamageList = damageInput.Split(',').Select(int.Parse).ToList();
                group.UnknownList = springInput.Split('.').Where(w => !string.IsNullOrWhiteSpace(w)).ToList();
                groups.Add(group);
            }
            Day12SolvePartOfSprings(groups);
            Day12SolvePartOfSprings2(groups);
            Day12SolvePartOfSprings(groups);
            foreach (Day12Group group in groups)
                Debug.WriteLine("Before: " + group.OriginalSprings + " After: " + group.Springs + " " + string.Join(',', group.DamageList));

            long arrangements = 0;
            long tests = 0;
            int groupCount = groups.Count();
            int currentGroup = 0, currentIndex = 0;
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
                bool print = false;
                bool isValid = false;
                //if (group.Springs == "?.?..#?????##??##?#.")
                //{
                //    print = false;
                //}
                Day12PrefillOperationalList(group, operationalList);
                int[] operationalPrefillList = new int[operationalList.Count()];
                for (i = 0; i < operationalList.Count(); i++)
                    operationalPrefillList[i] = operationalList[i];
                //Debug.WriteLine("operationalPrefillList: " + string.Join(',', operationalPrefillList));
                stopwatchInitGroup.Stop();
                while (true)
                {
                    //isValid = false;
                    //testCount++;
                    tests++;
                    stopwatchValidation.Start();
                    if (Day12IsValid2(group, operationalList))
                    {
                        arrangements++;
                        //isValid = true;
                    }
                    //if (print)
                    //Day12PrintCurrentString(operationalList, group.DamageList, group.Springs, isValid);
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
                    //if (tests % 10000000 == 0)
                    //    Debug.WriteLine("Ongoing Group: " + currentGroup + " Arrangements: " + arrangements + " Tests: " + tests + " Elapsed: " + stopwatch.Elapsed + " String: " + test);
                }
                currentGroup++;
                arrangementList.Add(arrangements);
                //File.AppendAllText(@"C:\Temp\AdventOfCoreDay12_2.txt", "Group: " + currentGroup + " Arrangements: " + arrangements + Environment.NewLine);
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
            //File.AppendAllText(@"C:\Temp\AdventOfCoreDay12_2.txt", "Arrangements: " + arrangementList.Sum() + Environment.NewLine);
        }

        [TestMethod]
        public void Day12_2_4Testing()
        {
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
            //foreach (Day12Group group in groups)
            //    Debug.WriteLine("Before: " + group.OriginalSprings + " After: " + group.Springs + " " + string.Join(',', group.DamageList));

            long arrangements = 0;
            long tests = 0;
            int groupCount = groups.Count();
            int currentGroup = 0, currentIndex = 0;
            Stopwatch stopwatchInitGroup = new Stopwatch();
            Stopwatch stopwatchValidation = new Stopwatch();
            Stopwatch stopwatchSwitchIndex = new Stopwatch();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<long> arrangementList = new List<long>();
            foreach (Day12Group group in groups)
            {
                if (groups.IndexOf(group) == 1)
                {

                }
                arrangements = 0;
                stopwatchInitGroup.Stop();
                arrangements = Day12TestArrangements(group.Springs, group.DamageList);
                currentGroup++;
                arrangementList.Add(arrangements);
                //File.AppendAllText(@"C:\Temp\AdventOfCoreDay12_2.txt", "Group: " + currentGroup + " Arrangements: " + arrangements + Environment.NewLine);
                Debug.WriteLine("Group: " + currentGroup + " Arrangements: " + arrangements + " Springs: " + group.Springs + " " + string.Join(',', group.DamageList));
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
            //File.AppendAllText(@"C:\Temp\AdventOfCoreDay12_2.txt", "Arrangements: " + arrangementList.Sum() + Environment.NewLine);
        }

        private long Day12TestArrangements(string springs, List<int> damageList)
        {
            if (springs.Count(c => c == '#' || c == '?') < damageList.Sum())
                return 0;
            else if (damageList.Count() == 0 && !springs.Any(a => a == '#'))
                return 1;
            else if (damageList.Count() == 0 && springs.Any(a => a == '#'))
                return 0;
            else if (springs[0] == '.')
                return Day12TestArrangements(springs.Substring(1), damageList);
            else if (springs.Length < (damageList.Sum() + damageList.Count() - 1))
                return 0;
            else if (springs[0] == '#')
            {
                if ((springs.Length > damageList.First() && springs[damageList.First()] == '#')
                    || springs.Take(damageList.First()).Any(a => a == '.'))
                    return 0;
                return Day12TestArrangements(
                    springs.Length == damageList.First() ? springs.Substring(damageList.First()) : '.' + springs.Substring(damageList.First() + 1)
                    , damageList.Count() > 0 ? damageList.Skip(1).ToList() : null);
            }
            else if (springs[0] == '?')
                return Day12TestArrangements('.' + springs.Substring(1), damageList)
                    + Day12TestArrangements('#' + springs.Substring(1), damageList);
            return 0;
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
            //if (group.Springs == "?#???#?#?????.?#")
            //{

            //}
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
                //Debug.WriteLine("Before: " + group.OriginalSprings + " After: " + group.Springs + " " + string.Join(',', group.DamageList));
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
            public string Springs { get; set; }
            public string OriginalSprings { get; set; }
            //public List<Day12PartGroup> PartGroupList { get; set; }
            public List<int> DamageList { get; set; }
            public List<string> UnknownList { get; set; }
        }

        //private class Day12PartGroup
        //{
        //    public List<char> Springs { get; set; }
        //}
    }
}

