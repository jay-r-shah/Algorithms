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
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //List<string> input = System.IO.File.ReadAllLines(args[0]).ToList();
            Graph = new Dictionary<int, List<int>>();
            //foreach (string data in input)
            //{
            //    List<int> neighbors = data.Split('\t').ToList().Select(s => String.IsNullOrEmpty(s) ? 0 : Int32.Parse(s)).ToList();
            //    int vertex = neighbors.First();
            //    neighbors.Remove(vertex);
            //    neighbors.RemoveWhere(x => x == 0);
            //    Graph.Add(vertex, neighbors);
            //}

            Graph.Add(1, new List<int>() { 2, 3 });
            Graph.Add(2, new List<int>() { 1, 3, 4 });
            Graph.Add(3, new List<int>() { 1, 2, 4 });
            Graph.Add(4, new List<int>() { 2, 3 });
            // Assume graph is valid

            int cuts = ComputeCuts(Graph);
        }

        private static int ComputeCuts(Dictionary<int, List<int>> graph)
        {
            var vertices = graph.Keys;
            int nVertices = graph.Count();
            while (nVertices > 2)
            {
                int selectVertex = graph.Keys.ElementAt(_rand.Next(vertices.Count()));
                var neighbors = graph[selectVertex];
                int selectNeighbor = neighbors.ElementAt(_rand.Next(neighbors.Count()));
                graph = Contract(graph, selectVertex, selectNeighbor);
                nVertices = graph.Count;
            }

            return 0;
        }

        private static Dictionary<int, List<int>> Contract(Dictionary<int, List<int>> graph, int selectVertex, int selectNeighbor)
        {
            List<int> firstVertexNeighbors = graph[selectVertex];
            List<int> secondVertexNeighbors = graph[selectNeighbor];
            
            // second vertex will be replaced as neighbor by first vertex.
            foreach (List<int> neighbors in graph.Values.Where(x => x.Contains(selectNeighbor)))
            {
                neighbors.Add(selectVertex);
                neighbors.Remove(selectNeighbor);
            }

            // neighbors of second vertex become neighbors of first vertex.
            firstVertexNeighbors = firstVertexNeighbors.Concat(secondVertexNeighbors).ToList();
            firstVertexNeighbors.RemoveAll(x => x == selectVertex); // remove self loops
            graph[selectVertex] = firstVertexNeighbors;


            graph.Remove(selectNeighbor);

            return graph;
        }
    }
}
