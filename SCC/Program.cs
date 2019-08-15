using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC
{
    class Program
    {
        public static void Main(string[] args)
        {
            string folder = @"C:\Users\jaysh\source\repos\stanford-algs\testCases\course2\assignment1SCC\";
            DirectoryInfo dinfo = new DirectoryInfo(folder);
            FileInfo[] Files = dinfo.GetFiles("*.txt");
            int correct = 0;
            int total = 0;
            int totalInputFiles = Files.Count(x => x.Name.StartsWith("input"));
            foreach (var inputFile in Files.Where(x => x.Name.StartsWith("input")))
            {
                total++;
                var start = Stopwatch.StartNew();
                var result = ComputeSCCs.Calculate(new string[] { inputFile.FullName });
                start.Stop();
                string outputFile = inputFile.FullName.Replace("input", "output");
                string expectedResult = System.IO.File.ReadAllText(outputFile).Trim();
                if (result.Item1 == expectedResult)
                {
                    correct++;
                }
                Console.Write("Correct = {0:F2}% \t {1}/{2} \t nNodes = {3} \t time = {4:F2}", (double)correct * 100 / total, total, totalInputFiles, result.Item2, (double)start.ElapsedMilliseconds / 1000 / 60);
                Console.Write("\t{0} \n", result.Item1 == expectedResult);
            }
        }
    }

    public static class ComputeSCCs
    {
        private static List<(int, int)> Graph = new List<(int, int)>(); // reversed graph
        private static Dictionary<int, List<(int, int)>> edges = new Dictionary<int, List<(int, int)>>();
        private static int nNodes = 0;
        private static HashSet<int> exploredNodes = new HashSet<int>();
        private static Dictionary<int, int> finishingTime = new Dictionary<int, int>();

        private static List<bool> visited = new List<bool>();
        private static List<(int, int)> newGraph = new List<(int, int)>(); // graph for 2nd pass
        private static List<int> SCCCount = new List<int>();
        
        /// <summary>
        /// Solution to Week 1 programming assignment of the Graph Search, Shortest Paths, and Data Structures
        /// course.
        /// 
        /// Compute the sizes of the five largest Strongly Connected Components (SCCs) of the given directed
        /// graph.
        /// --------------------------------------------------------------------------------------------------
        /// </summary>
        /// <param name="args"></param>
        public static (string,int) Calculate(string[] args)
        {
            List<string> input = System.IO.File.ReadAllLines(args[0]).ToList();
            SCCCount.Clear();
            Graph.Clear();
            nNodes = 0;
            exploredNodes.Clear();
            finishingTime.Clear();
            newGraph.Clear();
            edges.Clear();
            foreach (var item in input)
            {
                int tail = Convert.ToInt32(item.Split(' ')[0]);
                int head = Convert.ToInt32(item.Split(' ')[1]);
                if (tail > nNodes)
                {
                    nNodes = tail;
                }
                if (head > nNodes)
                {
                    nNodes = head;
                }
                Graph.Add((head, tail)); // reverse the graph
                if (!edges.TryGetValue(head, out var _))
                {
                    edges.Add(head, new List<(int, int)>() { (head, tail) });
                }
                else
                {
                    edges[head].Add((head, tail));
                }
            }
            visited = Enumerable.Repeat(false, nNodes).ToList();
            DFS_Loop();
            edges.Clear();
            PostProcessFinishingTime(finishingTime, ref newGraph);
            visited = Enumerable.Repeat(false, nNodes).ToList();
            exploredNodes.Clear();
            finishingTime.Clear();
            DFS_Loop(true);
            SCCCount.Sort();
            SCCCount.Reverse();
            if (SCCCount.Count() < 5)
            {
                SCCCount.AddRange(Enumerable.Repeat(0, 5).ToList());
            }
            return (string.Join(",", SCCCount.Take(5)), nNodes);
        }

        private static void PostProcessFinishingTime(Dictionary<int, int> finishingTime, ref List<(int, int)> newGraph)
        {
            newGraph.Clear();

            foreach (var arc in Graph)
            {
                newGraph.Add((finishingTime[arc.Item2],finishingTime[arc.Item1]));
                if (!edges.TryGetValue(finishingTime[arc.Item2], out var _))
                {
                    edges.Add(finishingTime[arc.Item2], new List<(int, int)>() { (finishingTime[arc.Item2], finishingTime[arc.Item1]) });
                }
                else
                {
                    edges[finishingTime[arc.Item2]].Add((finishingTime[arc.Item2], finishingTime[arc.Item1]));
                }
            }
            Graph = newGraph.ToList();
            newGraph.Clear();
        }

        private static void DFS_Loop(bool countSCCs = false)
        {
            int t = 0; //# of variables processed so far
            int tStart = t;
            int S; // Current source vertex;

            for (int i = nNodes + 1; i-- > 1;)
            {
                if (!visited[i - 1]) // !visited[i - 1]
                {
                    S = i;
                    DFS(i, ref S, ref t);
                    if (countSCCs)
                    {
                        SCCCount.Add(t - tStart);
                        tStart = t;
                    }
                }
            }
        }

        private static void DFS(int i, ref int S, ref int t)
        {
            //exploredNodes.Add(i);
            Stack<int> stack = new Stack<int>();
            stack.Push(i); // start vertex

            while (stack.Count() > 0)
            {
                int v = stack.Pop();
                if (!visited[v - 1])
                {
                    visited[v - 1] = true;
                    stack.Push(v);
                    if (edges.TryGetValue(v, out var edgeList))
                    {
                        foreach (var edge in edgeList)
                        {
                            int w = edge.Item2;

                            if (!visited[w - 1])
                            {
                                stack.Push(w);
                            }
                        }
                    }
                }
                else
                {
                    if (!finishingTime.TryGetValue(v, out int _))
                    {
                        t++;
                        finishingTime.Add(v, t);
                    }
                }
            }
        }
    }
}
