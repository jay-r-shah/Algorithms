﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC
{
    class Program
    {
        private static List<(int, int)> Graph = new List<(int, int)>(); // reversed graph
        private static int nNodes = 0;
        private static HashSet<int> exploredNodes = new HashSet<int>();
        private static Dictionary<int, int> finishingTime = new Dictionary<int, int>();


        private static List<(int, int)> newGraph = new List<(int, int)>(); // graph for 2nd pass
        private static Dictionary<int, int> leaders = new Dictionary<int, int>();
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
        static void Main(string[] args)
        {
            List<string> input = System.IO.File.ReadAllLines(args[0]).ToList();
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
            }
            Console.Write("Number of nodes = {0} \n", nNodes);
            //Graph = new Dictionary<int, List<int>>();
            DFS_Loop(Graph);
            PostProcessFinishingTime(finishingTime, ref newGraph);
            leaders.Clear();
            exploredNodes.Clear();
            finishingTime.Clear();
            DFS_Loop(newGraph, true);
            SCCCount.Sort();
            SCCCount.Reverse();
            Console.Write(string.Join(",", SCCCount));
            Console.Write("\n");
            Console.Read();
        }

        private static void PostProcessFinishingTime(Dictionary<int, int> finishingTime, ref List<(int, int)> newGraph)
        {
            newGraph.Clear();

            foreach (var arc in Graph)
            {
                newGraph.Add((finishingTime[arc.Item2],finishingTime[arc.Item1]));
            }
        }

        private static void DFS_Loop(List<(int, int)> graph, bool countSCCs = false)
        {
            int t = 0; //# of variables processed so far
            int tStart = t;
            int S; // Current source vertex;

            for (int i = nNodes + 1; i-- > 1;)
            {
                if (!exploredNodes.Contains(i))
                {
                    S = i;
                    DFS(graph, i, ref S, ref t);
                    if (countSCCs)
                    {
                        SCCCount.Add(t - tStart);
                        tStart = t;
                    }
                }
            }
        }

        private static void DFS(List<(int, int)> graph, int i, ref int S, ref int t)
        {
            exploredNodes.Add(i);
            leaders.Add(i, S);
            foreach (var arc in graph.Where(x => x.Item1 == i))
            {
                int j = arc.Item2;
                if (!exploredNodes.Contains(j))
                {
                    DFS(graph, j, ref S, ref t);
                }
            }
            t++;
            finishingTime.Add(i, t);
        }
    }
}
