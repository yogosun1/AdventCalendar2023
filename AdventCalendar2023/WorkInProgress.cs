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
            List<string> inputList = File.ReadAllLines(@"Input\Day24Test.txt").ToList();
            List<Day24HailStone> hailstones = new List<Day24HailStone>();
            foreach (string input in inputList)
            {
                List<string> inputSplit = input.Split('@').ToList();
                List<string> leftSideSplit = inputSplit[0].Split(',').ToList();
                List<string> rightSideSplit = inputSplit[1].Split(',').ToList();
                Day24HailStone hailstone = new Day24HailStone
                {
                    X = decimal.Parse(leftSideSplit[0].Trim()),
                    Y = decimal.Parse(leftSideSplit[1].Trim()),
                    Z = decimal.Parse(leftSideSplit[2].Trim()),
                    Vx = decimal.Parse(rightSideSplit[0].Trim()),
                    Vy = decimal.Parse(rightSideSplit[1].Trim()),
                    Vz = decimal.Parse(rightSideSplit[2].Trim()),
                };
                hailstone.Slope = hailstone.Vy / hailstone.Vx;
                hailstone.YIntercept = hailstone.Slope * (hailstone.X + hailstone.Y);
                hailstones.Add(hailstone);
            }

            int intersectCount = 0;
            decimal xyMin = 200000000000000;
            decimal xyMax = 400000000000000;
            Day24HailStone line1, line2;
            for (int h = 0; h < hailstones.Count(); h++)
            {
                line1 = hailstones[h];
                for (int i = h + 1; i < hailstones.Count(); i++)
                {
                    line2 = hailstones[i];
                    decimal x = (line2.YIntercept - line1.YIntercept) / (line1.Slope - line2.Slope);


                    decimal A1 = line1.Vy;
                    decimal B1 = line1.Vx;
                    decimal C1 = A1 * line1.X + B1 * line1.Y;

                    decimal A2 = line2.Vy;
                    decimal B2 = line2.Vx;
                    decimal C2 = A2 * line2.X + B2 * line2.Y;

                    decimal delta = A1 * B2 - A2 * B1;

                    if (delta == 0)
                        throw new ArgumentException("Lines are parallel");

                    decimal xI = (B2 * C1 - B1 * C2) / delta;
                    decimal yI = (A1 * C2 - A2 * C1) / delta;
                }
            }
            foreach (Day24HailStone hailstone in hailstones)
            {

            }
        }

        private class Day24HailStone
        {
            public decimal X { get; set; }
            public decimal Y { get; set; }
            public decimal Z { get; set; }
            public decimal Vx { get; set; }
            public decimal Vy { get; set; }
            public decimal Vz { get; set; }
            public decimal Slope { get; set; }
            public decimal YIntercept { get; set; }
        }

        [TestMethod]
        public void Day25()
        {
            List<string> inputList = File.ReadAllLines(@"Input\Day25.txt").ToList();
        }
    }
}
