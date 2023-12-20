using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;

namespace AdventCalendar2023
{
    [TestClass]
    public class WorkInProgress
    {
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