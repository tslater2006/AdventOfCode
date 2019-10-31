using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Problems._2018
{
    internal class Day23 : BaseProblem
    {
        List<NanoBot> Bots = new List<NanoBot>();
        public Day23() : base(2018, 23)
        {
            Regex botParse = new Regex(".*?<(-?\\d+),(-?\\d+),(-?\\d+)>, r=(-?\\d+)");
            foreach (var s in InputLines)
            {
                var match = botParse.Match(s);
                NanoBot bot = new NanoBot();
                bot.X = int.Parse(match.Groups[1].Value);
                bot.Y = int.Parse(match.Groups[2].Value);
                bot.Z = int.Parse(match.Groups[3].Value);
                bot.R = int.Parse(match.Groups[4].Value);
                Bots.Add(bot);
            }
        }

        internal override string SolvePart1()
        {
            var largestRadius = Bots.OrderBy(b => b.R).Last();

            var total = Bots.Where(b => BotBotDistance(b, largestRadius) <= largestRadius.R).Count();

            return "";
        }
        internal int BotXYZDistance(NanoBot bot, int x, int y, int z)
        {
            int dist = Math.Abs(bot.X - x);
            dist += Math.Abs(bot.Y - y);
            dist += Math.Abs(bot.Z - z);

            return dist;
        }
        internal int BotBotDistance(NanoBot bot1, NanoBot bot2)
        {
            int dist = Math.Abs(bot1.X - bot2.X);
            dist += Math.Abs(bot1.Y - bot2.Y);
            dist += Math.Abs(bot1.Z - bot2.Z);

            return dist;
        }
        internal override string SolvePart2()
        {
           
            return "";
        }
    }

    internal class NanoBot
    {
        internal int X;
        internal int Y;
        internal int Z;
        internal int R;
    }
}
