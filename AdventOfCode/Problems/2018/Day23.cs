using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            var total = Bots.Where(b => b.DistanceTo(largestRadius) <= largestRadius.R).Count();

            return total.ToString();
        }

        /* Algorithm/explanation: https://www.reddit.com/r/adventofcode/comments/a8sqov/help_day_23_part_2_any_provably_correct_fast/ecfag7f */
        internal override string SolvePart2()
        {
            PriorityQueue<NanoBot> q = new PriorityQueue<NanoBot>();
            /* create a bot that has every other bot in range */
        var maxRadius = Bots.Select(b => Math.Abs(b.X) + Math.Abs(b.Y) + Math.Abs(b.Z)).Max();
            var searchBot = new NanoBot() { X = 0, Y = 0, Z = 0, R = maxRadius };

            var intersectCount = Bots.Where(b => b.Intersects(searchBot)).Count();

            q.AddOrUpdate(searchBot, intersectCount);

            while (q.Count > 0)
            {
                var maxPriority = q._priorities.Max();
                searchBot = q._dataByPriority[maxPriority].OrderBy(d => d.DistanceTo(0, 0, 0)).OrderBy(d => d.R).First();

                q.DequeueItem(searchBot);

                if (searchBot.R <= 0)
                {
                    return searchBot.DistanceTo(0, 0, 0).ToString();
                } else
                {
                    foreach(var bot in searchBot.SplitBot())
                    {
                        intersectCount = Bots.Where(b => b.Intersects(bot)).Count();

                        q.AddOrUpdate(bot, intersectCount);
                    }
                }
            }

            return "??";
        }
    }

    internal class NanoBot
    {
        internal int X;
        internal int Y;
        internal int Z;
        internal int R;

        internal bool Intersects(NanoBot otherBot)
        {
            return DistanceTo(otherBot) <= R + otherBot.R;
        }

        internal int DistanceTo(int x, int y, int z)
        {
            int dist = Math.Abs(this.X - x);
            dist += Math.Abs(this.Y - y);
            dist += Math.Abs(this.Z - z);

            return dist;
        }
        internal int DistanceTo (NanoBot otherBot)
        {
            int dist = Math.Abs(this.X - otherBot.X);
            dist += Math.Abs(this.Y - otherBot.Y);
            dist += Math.Abs(this.Z - otherBot.Z);

            return dist;
        }

        internal List<NanoBot> SplitBot()
        {
            List<NanoBot> bots = new List<NanoBot>();
            int[] offsets = new int[2];
            if (R > 2)
            {
                var newR = (int)(Math.Ceiling(2 * R / 3.0));
                var start = (int)(-(Math.Floor(R / 3.0)));
                var end = (int)((Math.Floor(R / 3.0)));
                offsets[0] = start;
                offsets[1] = end;
                foreach(var d in offsets)
                {
                    bots.Add(new NanoBot() { X = this.X + d, Y = this.Y, Z = this.Z, R = newR });
                    bots.Add(new NanoBot() { X = this.X, Y = this.Y + d, Z = this.Z, R = newR });
                    bots.Add(new NanoBot() { X = this.X + d, Y = this.Y, Z = this.Z + d, R = newR });
                }

            } else
            {
                for (var x = -R; x < R +1; x++)
                {
                    for (var y = -(R - Math.Abs(x)); y < R+1-Math.Abs(x); y++)
                    {
                        for (var z = -(R - Math.Abs(x) - Math.Abs(y)); z < R + 1 - Math.Abs(x) - Math.Abs(y); z++)
                        {
                            bots.Add(new NanoBot() { X = this.X + x, Y = this.Y + y, Z = this.Z + z });
                        }
                    }
                }
            }

            return bots;
        }
    }
}
