using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode
{
    internal abstract class BaseProblem
    {
        internal int Year;
        internal int Day;

        internal abstract string SolvePart1();
        internal abstract string SolvePart2();

        protected string[] InputLines;

        public BaseProblem(int year, int day)
        {
            this.Year = year;
            this.Day = day;
            ReadInput();
        }

        internal void ReadInput()
        {
            InputLines = File.ReadAllLines(Path.Join("Inputs", Year.ToString(), Day.ToString(), "Input.txt"));
        }
    }
}
