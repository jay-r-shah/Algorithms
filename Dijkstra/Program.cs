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
                    var result = DijkstraShortestPath.Calculate(new string[] { inputFile.FullName });
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
        }
    }

    public static class DijkstraShortestPath
    {
        internal static (string,int) Calculate(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
