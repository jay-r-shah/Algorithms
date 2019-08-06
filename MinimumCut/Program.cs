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
            List<string> input = System.IO.File.ReadAllLines(args[0]).ToList();
            Graph = new Dictionary<int, List<int>>();
            foreach (string data in input)
            {
                List<int> neighbors = data.Split('\t').ToList().Select(s => String.IsNullOrEmpty(s) ? 0 : Int32.Parse(s)).ToList();
                int vertex = neighbors.First();
                neighbors.Remove(vertex);
                neighbors.RemoveAll(x => x == 0);
                Graph.Add(vertex, neighbors);
            }

            //Graph.Add(1, new List<int>() { 2, 3 });
            //Graph.Add(2, new List<int>() { 1, 3, 4 });
            //Graph.Add(3, new List<int>() { 1, 2, 4 });
            //Graph.Add(4, new List<int>() { 2, 3 });
            // Assume graph is valid

            int cuts; bool status;
            (cuts, status) = ComputeCuts(Graph);

            List<int> nCuts = new List<int>();
            int nIterations = 10000;

            for (int i = 0; i < nIterations; i++)
            {
                var x = ComputeCuts(Graph);
                nCuts.Add(x.Item1);
            }

            Console.Write(nCuts.Min());
            Console.Read();
        }

        private static (int, bool) ComputeCuts(Dictionary<int, List<int>> graph)
        {
            // create a copy of the graph to work with
            Dictionary<int, List<int>> _graph = graph.ToDictionary(dwItem => dwItem.Key, dwItem => dwItem.Value.ToList() );
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
                _graph = Contract(_graph, selectVertex, selectNeighbor);
                nVertices = _graph.Count;
            }
            int nCuts = _graph.First().Value.Count();

            foreach (var item in _graph.Values)
            {
                if (item.Count != nCuts)
                    return (nCuts, false);
            }

            return (nCuts, true);
        }

        private static Dictionary<int, List<int>> Contract(Dictionary<int, List<int>> graph, int selectVertex, int selectNeighbor)
        {
            Dictionary<int, List<int>> _graph = graph.ToDictionary(dwItem => dwItem.Key, dwItem => dwItem.Value.ToList());

            List<int> firstVertexNeighbors = _graph[selectVertex];
            List<int> secondVertexNeighbors = _graph[selectNeighbor];
            
            // second vertex will be replaced as neighbor by first vertex.
            foreach (List<int> neighbors in _graph.Where(x => x.Value.Contains(selectNeighbor)).Select(item => item.Value))
            {
                int nEdgesToReplace = neighbors.Count(x => x == selectNeighbor);
                neighbors.RemoveAll(x => x == selectNeighbor);
                neighbors.AddRange(Enumerable.Repeat(selectVertex, nEdgesToReplace).ToList());
            }

            // neighbors of second vertex become neighbors of first vertex.
            firstVertexNeighbors = firstVertexNeighbors.Concat(secondVertexNeighbors).ToList();
            firstVertexNeighbors.RemoveAll(x => x == selectVertex || x == selectNeighbor); // remove self loops & actual selected edge
            _graph[selectVertex] = firstVertexNeighbors;

            _graph.Remove(selectNeighbor);

            return _graph;
        }
    }
}
