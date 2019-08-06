using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimumCut
{
    class Program
    {
        private static Dictionary<int, List<int>> Graph;
        private static Random _rand = new Random();

        /// <summary>
        /// Solution to Week 4 assignment of Coursera Algorithms specialization
        /// 
        /// Compute the minimum cut of a graph using Karger's (Random Contraction) Algorithm
        /// --------------------------------------------------------------------------------
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            List<string> input = System.IO.File.ReadAllLines(args[0]).ToList();
            Graph = new Dictionary<int, List<int>>();
            
            // Assume input graph is valid and populate graph object
            foreach (string data in input)
            {
                List<int> neighbors = data.Split('\t').ToList().Select(s => String.IsNullOrEmpty(s) ? 0 : Int32.Parse(s)).ToList();
                int vertex = neighbors.First();
                neighbors.Remove(vertex);
                neighbors.RemoveAll(x => x == 0);
                Graph.Add(vertex, neighbors);
            }

            List<int> nCuts = new List<int>();
            int nIterations = 1000;

            for (int i = 0; i < nIterations; i++)
            {
                (int, bool) x = ComputeCut(Graph);
                if (x.Item2)
                    nCuts.Add(x.Item1);
            }

            Console.Write("Minimum number of crossing edges caluculated in {0} iterations: {1} \n", nIterations, nCuts.Min());
            Console.Write("Probablity: {0}% \n",((double) nCuts.Count(x => x == nCuts.Min()) * 100 / nIterations));
            Console.Write("Press any key to continue...");
            Console.Read();
        }

        /// <summary>
        /// Calculate minimum cut iteratively
        /// </summary>
        /// <param name="graph">Dictionary representation of the graph</param>
        /// <returns></returns>
        private static (int, bool) ComputeCut(Dictionary<int, List<int>> graph)
        {
            // create a copy of the graph to work with since dictionaries cannot be passed by value
            Dictionary<int, List<int>> _graph = graph.ToDictionary(dwItem => dwItem.Key, dwItem => dwItem.Value.ToList());

            int nVertices = _graph.Count();
            List<int> selectedList = new List<int>();
            List<int> selectedNeighbor = new List<int>();
            while (nVertices > 2)
            {
                var vertices = _graph.Keys;
                int selectVertex = _graph.Keys.ElementAt(_rand.Next(vertices.Count()));
                var neighbors = _graph[selectVertex];
                int selectNeighbor = neighbors.ElementAt(_rand.Next(neighbors.Count()));
                selectedList.Add(selectVertex);
                selectedNeighbor.Add(selectNeighbor);
                Contract(_graph, selectVertex, selectNeighbor);
                nVertices = _graph.Count;
            }

            int nCrossingEdges = _graph.First().Value.Count();

            foreach (var item in _graph.Values)
            {
                if (item.Count != nCrossingEdges)
                    return (nCrossingEdges, false);
            }

            return (nCrossingEdges, true);
        }

        /// <summary>
        /// Contraction routine for Karger's algorithm
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="selectVertex">First vertex that will absorb the second vertex</param>
        /// <param name="selectNeighbor">Second vertex</param>
        /// <returns></returns>
        private static Dictionary<int, List<int>> Contract(Dictionary<int, List<int>> graph, int selectVertex, int selectNeighbor)
        {
            List<int> firstVertexNeighbors = graph[selectVertex];
            List<int> secondVertexNeighbors = graph[selectNeighbor];
            
            // second vertex will be replaced as neighbor by first vertex.
            foreach (List<int> neighbors in graph.Where(x => x.Value.Contains(selectNeighbor)).Select(item => item.Value))
            {
                int nEdgesToReplace = neighbors.Count(x => x == selectNeighbor);
                neighbors.RemoveAll(x => x == selectNeighbor);
                neighbors.AddRange(Enumerable.Repeat(selectVertex, nEdgesToReplace).ToList());
            }

            // neighbors of second vertex become neighbors of first vertex.
            firstVertexNeighbors = firstVertexNeighbors.Concat(secondVertexNeighbors).ToList();
            firstVertexNeighbors.RemoveAll(x => x == selectVertex || x == selectNeighbor); // remove self loops & actual selected edge
            graph[selectVertex] = firstVertexNeighbors;

            graph.Remove(selectNeighbor); // remove second vertex from the graph

            return graph;
        }
    }
}
