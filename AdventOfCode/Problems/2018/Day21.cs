using AdventOfCode.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Problems._2018
{
    internal class Day21 : BaseProblem
    {
        public Day21() : base(2018, 21)
        {
        }

        internal override string SolvePart1()
        {
            ElfCPU cpu = new ElfCPU(InputLines);
            cpu.Registers[0] = 10332277;
            cpu.RunToIP(28);
            cpu.Step();
            var answer = cpu.Registers[1];
            return answer.ToString();
        }

        internal override string SolvePart2()
        {
            ElfCPU cpu = new ElfCPU(InputLines);
            cpu.Registers[0] = 0;
            List<int> haltValues = new List<int>();
            int haltValue;
            while (true)
            {
                cpu.RunToIP(29);
                haltValue = cpu.Registers[1];
                if (haltValues.Contains(haltValue))
                {
                    /* we've looped! */
                    break;
                } else
                {
                    haltValues.Add(haltValue);
                }
            }
            return haltValues.Last().ToString();
        }
    }
}
