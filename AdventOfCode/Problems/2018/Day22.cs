using AdventOfCode.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AdventOfCode.Problems._2018
{
    internal class Day22 : BaseProblem
    {
        public int Depth;
        public Point Target;
        public int[,] Map; /* stores geologic index values */
        public Day22() : base(2018, 22)
        {
            Depth = int.Parse(InputLines[0].Split(' ').Last());
            var TargetXY = InputLines[1].Split(' ').Last().Split(',');
            Target = new Point(int.Parse(TargetXY[0]), int.Parse(TargetXY[1]));
            Map = new int[Target.X * 3 + 1, Target.Y * 3 + 1];

            GenerateIndexValues();
        }

        private void GenerateIndexValues()
        {

            for (var x = 1; x <= Target.X * 3; x++)
            {
                Map[x, 0] = ((x * 16807) + Depth) % 20183;
            }

            for (var y = 1; y <= Target.Y * 3; y++)
            {
                Map[0, y] = ((y * 48271) + Depth) % 20183;
            }

            for (var x = 1; x <= Target.X * 3; x++)
            {
                for (var y = 1; y <= Target.Y * 3; y++)
                {
                    Map[x, y] = ((Map[x - 1, y] * Map[x, y - 1]) + Depth) % 20183;
                }
            }
            Map[0, 0] = 0;
            Map[Target.X, Target.Y] = 0;

            for (var y = 0; y <= Target.Y * 3; y++)
            {
                for (var x = 0; x <= Target.X * 3; x++)
                {
                    Map[x,y] = (Map[x, y] % 3);
                }
            }
        }

        internal override string SolvePart1()
        {
            int risk = 0;
            for (var y = 0; y <= Target.Y; y++)
            {
                for (var x = 0; x <= Target.X; x++)
                {
                    risk += Map[x, y];
                }
            }

            return risk.ToString();
        }

        internal override string SolvePart2()
        {
            Dictionary<CaveNode, Dictionary<CaveNode, int>> graph = new Dictionary<CaveNode, Dictionary<CaveNode, int>>();

            for (var x = 0; x < Target.X * 3; x++)
            {
                for (var y = 0; y < Target.Y * 3; y++)
                {
                    var tools = PossibleTools(x, y);

                    AddEdge(graph, new CaveNode(x, y, tools[0]), new CaveNode(x, y, tools[1]), 7);

                    foreach (var tool in tools)
                    {
                        foreach (var point in PointsAround(x,y))
                        {
                            if (PossibleTools(point.X, point.Y).Contains(tool)) {
                                AddEdge(graph, new CaveNode(x, y, tool), new CaveNode(point.X, point.Y, tool), 1);
                            }
                        }
                    }
                }
            }

            var distances = dijkstra(graph, new CaveNode(0, 0, CaveTool.TORCH));
            var result = distances[new CaveNode(Target.X, Target.Y, CaveTool.TORCH)];

            return result.ToString();
        }

        internal Dictionary<CaveNode, int> dijkstra(Dictionary<CaveNode, Dictionary<CaveNode, int>> graph, CaveNode start)
        {
            Dictionary<CaveNode, int> dist = new Dictionary<CaveNode, int>();
            PriorityQueue<CaveNode> q2 = new PriorityQueue<CaveNode>();
            HashSet<CaveNode> seen = new HashSet<CaveNode>();
            dist.Add(start, 0);
            foreach (CaveNode v in graph.Keys)
            {
                if (v.Equals(start) == false)
                {
                    dist.Add(v, int.MaxValue);
                }

                q2.AddOrUpdate(v, dist[v]);
            }

            while(q2.Count != 0)
            {

                /* get the smallest vertex */
                //var abc = q.Min(z => dist[z]);
                //var u = q.Where(z => dist[z] == abc).First();
                
                //q.Remove(u);
                CaveNode u;
                if (q2.TryDequeueMin(out u) == false)
                {
                    Debugger.Break();
                }

                seen.Add(u);

                foreach (CaveNode v in graph[u].Keys)
                {
                    if (seen.Contains(v))
                    {
                        continue;
                    }

                    var alt = dist[u] + graph[u][v];
                    if (alt < dist[v])
                    {
                        dist[v] = alt;
                        q2.AddOrUpdate(v,dist[v]);
                    }
                }

            }
            return dist;
        }

        internal List<Point> PointsAround(int x, int y)
        {
            List<Point> results = new List<Point>();

            if (x - 1 >= 0 && y >= 0 && (x-1) < Target.X*2 && y < Target.Y *2)
            {
                results.Add(new Point(x - 1, y));
            }
            if (x + 1 >= 0 && y >= 0 && (x + 1) < Target.X * 3 && y < Target.Y * 3)
            {
                results.Add(new Point(x + 1, y));
            }
            if (x >= 0 && y - 1 >= 0 && x < Target.X * 3 && y - 1 < Target.Y * 3)
            {
                results.Add(new Point(x, y - 1));
            }
            if (x >= 0 && y + 1 >= 0 && x < Target.X * 3 && y + 1 < Target.Y * 3)
            {
                results.Add(new Point(x, y + 1));
            }

            return results;
        }

        internal void AddEdge(Dictionary<CaveNode, Dictionary<CaveNode, int>> graph, CaveNode n1, CaveNode n2, int weight)
        {
            if (graph.ContainsKey(n1) == false)
            {
                graph.Add(n1, new Dictionary<CaveNode, int>());
            }

            if (graph[n1].ContainsKey(n2))
            {
                graph[n1][n2] = weight;
            } else
            {
                graph[n1].Add(n2, weight);
            }

            if (graph.ContainsKey(n2) == false)
            {
                graph.Add(n2, new Dictionary<CaveNode, int>());
            }

            if (graph[n2].ContainsKey(n1))
            {
                graph[n2][n1] = weight;
            }
            else
            {
                graph[n2].Add(n1, weight);
            }

        }

        internal List<CaveTool> PossibleTools(int x, int y)
        {
            List<CaveTool> results = new List<CaveTool>();
            if (Map[x, y] == 0)
            {
                results.Add(CaveTool.CLIMBING);
                results.Add(CaveTool.TORCH);
            }
            else if (Map[x, y] == 1)
            {
                results.Add(CaveTool.CLIMBING);
                results.Add(CaveTool.NONE);
            }
            else if (Map[x, y] == 2)
            {
                results.Add(CaveTool.TORCH);
                results.Add(CaveTool.NONE);
            }

            return results;
        }
    }

    internal struct CaveNode
    {
        internal int X;
        internal int Y;
        internal CaveTool Tool;  
        
        internal CaveNode(int x, int y, CaveTool tool)
        {
            X = x;
            Y = y;
            Tool = tool;
        }

        public override string ToString()
        {
            return X + ", " + Y + ", " + Tool.ToString();
        }
    }

    internal enum CaveTool
    {
        CLIMBING, TORCH, NONE
    }

    
}
