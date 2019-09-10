using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoSum
{
    class Program
    {
        /// <summary>
        /// Solution to Week 4 programming assignment of the Graph Search, Shortest Paths, and Data Structures
        /// course.
        /// 
        /// Compute the number of target values t in the interval [-10000,10000] (inclusive) such that there 
        /// are distinct numbers x,y in the input file that satisfy x + y = t. 
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
        /// where output_{filename}.txt is a text file containing the sizes of the five larges SCCs of the graph described in
        /// input_{filename}.txt
        /// 
        /// --------------------------------------------------------------------------------------------------
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
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
                    var result = TwoSum.Calculate(new string[] { inputFile.FullName });
                    start.Stop();
                    string outputFile = inputFile.FullName.Replace("input", "output");
                    int expectedResult = Convert.ToInt32(System.IO.File.ReadAllText(outputFile).Trim());
                    if (result.Item1 == expectedResult)
                    {
                        correct++;
                    }
                    Console.Write("Correct = {0:F2}% \t {1}/{2} \t Answer = {3} \t n = {4} \t time = {5:F2}", (double)correct * 100 / total, total, totalInputFiles, result.Item1, result.Item2, (double)start.ElapsedMilliseconds / 1000);
                    Console.Write("\t{0} \n", result.Item1 == expectedResult);
                }
            }

            var start1 = Stopwatch.StartNew();
            var solution = TwoSum.Calculate(args);
            start1.Stop();
            Console.Write("Solution = {0}", solution);
            Console.Write("\t time={0:F2}s", (double)start1.ElapsedMilliseconds / 1000);
            Console.Read();
        }

        public static class TwoSum
        {
            private static Hashtable H = new Hashtable();
            internal static (int, int) Calculate(string[] args)
            {
                int twoSums = 0;

                List<long> input = System.IO.File.ReadAllLines(args[0]).ToList().Select(Int64.Parse).ToList();
                H.Clear();
                List<bool> Tset = new List<bool>();
                for (int t = -10000; t < 10001; t++)
                {
                    Tset.Add(false);
                }
                int i = 1;
                foreach (long val in input)
                {
                    if (!H.ContainsKey(val))
                    {
                        H.Add(val, i);
                        i++;
                        for (int t = -10000; t < 10001; t++)
                        {
                            if (Tset[t + 10000])
                            {
                                continue;
                            }
                            if (H.Contains(t - val) && t != val)
                            {
                                Tset[t + 10000] = true;
                            }
                        }
                    }
                }
                twoSums = Tset.Count(x => x);

                return (twoSums, input.Count());
            }
        }
    }
}
