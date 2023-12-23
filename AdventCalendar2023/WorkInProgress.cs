using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.ExceptionServices;

namespace AdventCalendar2023
{
    [TestClass]
    public class WorkInProgress
    {
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
