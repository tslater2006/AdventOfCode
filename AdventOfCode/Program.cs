using AdventOfCode.Misc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {

            int year = 2018;
            int day = 23;


            IEnumerable<BaseProblem> problems = typeof(BaseProblem).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(BaseProblem)) && !t.IsAbstract).Select(t => (BaseProblem)Activator.CreateInstance(t));
            foreach (var p in problems)
            {
                if (p.Year == year && (p.Day == day) || day == 0)
                {
                    Console.WriteLine($"Running Year {p.Year}, Day {p.Day}...");
                    Console.WriteLine($"  Part 1: {p.SolvePart1()}");
                    Console.WriteLine($"  Part 2: {p.SolvePart2()}");
                }
            }
        }
    }
}
