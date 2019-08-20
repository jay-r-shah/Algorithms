using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra
{
    class Program
    {
        /// <summary>
        /// Solution to Week 2 programming assignment of the Graph Search, Shortest Paths, and Data Structures
        /// course.
        /// 
        /// Arguments:
        /// ----------
        /// 
        /// 1. Path to text file representing the directed graph
        /// 2. 0/1 flag indicating if test cases should be executed
        /// 3. if 1 - path to test cases folder. If this is empty, the current working directory is assumed.
        /// 
        /// Test cases format:
        /// input_{filename}.txt and output_{filename}.txt
        /// 
        /// --------------------------------------------------------------------------------------------------
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string folder = "";
            if (args.Count() > 2 && !string.IsNullOrEmpty(args[2]))
                folder = args[2];
            else
                folder = Directory.GetCurrentDirectory();

            DirectoryInfo dinfo = new DirectoryInfo(folder);
            FileInfo[] Files = dinfo.GetFiles("*.txt");
            int correct = 0;
            int total = 0;
            int totalInputFiles = Files.Count(x => x.Name.StartsWith("input"));
            if (args.Count() > 1 && args[1] == "1")
            {
                foreach (var inputFile in Files.Where(x => x.Name.StartsWith("input")))
                {
                    total++;
                    var start = Stopwatch.StartNew();
                    var result = DijkstraShortestPath.Calculate( inputFile.FullName );
                    start.Stop();
                    string outputFile = inputFile.FullName.Replace("input", "output");
                    string expectedResult = System.IO.File.ReadAllText(outputFile).Trim();
                    if (result.Item1 == expectedResult)
                    {
                        correct++;
                    }
                    Console.Write("Correct = {0:F2}% \t {1}/{2} \t nNodes = {3} \t time = {4:F2}", (double)correct * 100 / total, total, totalInputFiles, result.Item2, (double)start.ElapsedMilliseconds / 1000);
                    Console.Write("\t{0} \n", result.Item1 == expectedResult);
                }
            }

            var start1 = Stopwatch.StartNew();
            var solution = DijkstraShortestPath.Calculate(args[0]);
            start1.Stop();
            Console.Write("Solution = {0}", solution.Item1);
            Console.Write("\t time={0:F2}s", (double)start1.ElapsedMilliseconds / 1000);
            Console.Write("\t nNodes = {0} \n", solution.Item2);
            Console.Read();

        }
    }

    public static class DijkstraShortestPath
    {
        private static int[] reqdVertices = new int[10] { 7, 37, 59, 82, 99, 115, 133, 165, 188, 197 };
        private static List<int> X = new List<int>(); //Vertices processed so far - initialized with start vertex 1
        private static Dictionary<int, int> A = new Dictionary<int, int>(); // computed shortest path distances per vertex

        private static List<(int, int)> Graph = new List<(int, int)>(); // edges

        /// <summary>
        /// mapping of edges that start from vertex key
        /// </summary>
        private static Dictionary<int, List<(int, int)>> edges = new Dictionary<int, List<(int, int)>>();

        private static List<int> Lengths = new List<int>(); // List of path lengths corresponding to graph
        private static HashSet<int> Vertices = new HashSet<int>(); // set of vertices

        internal static (string, int) Calculate(string filename)
        {
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            Graph.Clear();
            edges.Clear();
            Lengths.Clear();
            Vertices.Clear();
            X.Clear();
            A.Clear();

            foreach (var line in input)
            {
                string[] items = line.Split('\t');
                int tail = Convert.ToInt32(items[0]);
                Vertices.Add(tail);
                for (int i = 1; i < items.Count(); i++)
                {
                    var head = items[i].Split(',');
                    if (String.IsNullOrEmpty(head[0]))
                    {
                        continue;
                    }
                    int headVertex = Convert.ToInt32(head[0]);
                    int length = Convert.ToInt32(head[1]);
                    Graph.Add((tail, headVertex));
                    Lengths.Add(length);
                    Vertices.Add(headVertex);
                    if (!edges.TryGetValue(tail, out var _))
                    {
                        edges.Add(tail, new List<(int, int)>() { (tail, headVertex) });
                    }
                    else
                    {
                        edges[tail].Add((tail, headVertex));
                    }
                }
            }

            int s = 1; // Start vertex;
            A.Add(s, 0);// initialize shortest path for start vertex;
            X.Add(s);

            while (X.Count != Vertices.Count())
            {
                int minLength = 0;
                int criterion = Int32.MaxValue;
                (int, int) minEdge = (0, 0);
                
                foreach (KeyValuePair<int, List<(int, int)>> edgeList in edges.Where(x => X.Contains(x.Key))) // edges that start in X
                {
                    int tempCriterion;
                    foreach ((int, int) edge in edgeList.Value.Where(x => !X.Contains(x.Item2)))
                    {
                        tempCriterion = A[edge.Item1] + Lengths[Graph.IndexOf(edge)];
                        if (tempCriterion < criterion)
                        {
                            criterion = tempCriterion;
                            minEdge = edge;
                            minLength = Lengths[Graph.IndexOf(edge)];
                        }

                    }
                }

                int vstar, wstar;
                (vstar, wstar) = minEdge;

                X.Add(wstar);
                A.Add(wstar, A[vstar] + minLength);
            }

            List<string> outputs = new List<string>();

            foreach (int item in reqdVertices)
            {
                outputs.Add(A[item].ToString());
            }

            return (String.Join(",",outputs.ToArray()), 0);
        }
    }
}
