using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode.Problems._2018
{
    struct ConstellationPoint
    {
        public int X, Y, Z, T;
        public ConstellationPoint(int [] points)
        {
            X = points[0];
            Y = points[1];
            Z = points[2];
            T = points[3];
        }

        public int DistanceTo(ConstellationPoint p2)
        {
            return Math.Abs(X - p2.X) + Math.Abs(Y - p2.Y) + Math.Abs(Z - p2.Z) + Math.Abs(T - p2.T) ;
        }

    }
    internal class Day25 : BaseProblem
    {
        List<ConstellationPoint> Points = new List<ConstellationPoint>();
        public Day25() : base(2018, 25)
        {
            foreach(var s in InputLines)
            {
                var points = s.Split(',').Select(c => int.Parse(c)).ToArray();
                Points.Add(new ConstellationPoint(points));
            }
        }

        internal override string SolvePart1()
        {
            Dictionary<ConstellationPoint, List<ConstellationPoint>> EdgeMap = new Dictionary<ConstellationPoint, List<ConstellationPoint>>();

            for (var x = 0; x < Points.Count; x++)
            {
                for (var y = 0; y < Points.Count; y++)
                {
                    if (Points[x].DistanceTo(Points[y]) <= 3)
                    {
                        if (EdgeMap.ContainsKey(Points[x]) == false)
                        {
                            EdgeMap.Add(Points[x], new List<ConstellationPoint>());
                        }
                        EdgeMap[Points[x]].Add(Points[y]);
                    }
                }
            }
            var constellationCount = 0;

            HashSet<ConstellationPoint> seen = new HashSet<ConstellationPoint>();
            foreach(var point in EdgeMap.Keys)
            {
                if (seen.Contains(point))
                {
                    continue;
                }
                constellationCount++;

                Queue<ConstellationPoint> q = new Queue<ConstellationPoint>();
                q.Enqueue(point);

                while (q.Count > 0)
                {
                    var current = q.Dequeue();
                    if (seen.Contains(current))
                    {
                        continue;
                    }

                    seen.Add(current);
                    foreach(var e in EdgeMap[current])
                    {
                        q.Enqueue(e);
                    }
                }

            }

            return constellationCount.ToString();
        }

        internal override string SolvePart2()
        {
            throw new NotImplementedException();
        }
    }
}
