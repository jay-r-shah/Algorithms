using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedianMaintenance
{
    class Program
    {
        /// <summary>
        /// Solution to Week 3 programming assignment of the Graph Search, Shortest Paths, and Data Structures
        /// course.
        /// 
        /// Implementation of Median Maintenance algorithm. Output the sum the medians, modulo 10000 (i.e. only
        /// the last four digits.
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
                    int result = MedianMaintenance.Calculate(new string[] { inputFile.FullName });
                    start.Stop();
                    string outputFile = inputFile.FullName.Replace("input", "output");
                    int expectedResult = Convert.ToInt32(System.IO.File.ReadAllText(outputFile).Trim());
                    if (result == expectedResult)
                    {
                        correct++;
                    }
                    Console.Write("Correct = {0:F2}% \t {1}/{2} \t Answer = {3} \t time = {4:F2}", (double)correct * 100 / total, total, totalInputFiles, result, (double)start.ElapsedMilliseconds / 1000);
                    Console.Write("\t{0} \n", result == expectedResult);
                }
            }

            var start1 = Stopwatch.StartNew();
            var solution = MedianMaintenance.Calculate(args);
            start1.Stop();
            Console.Write("Solution = {0}", solution);
            Console.Write("\t time={0:F2}s", (double)start1.ElapsedMilliseconds / 1000);
            Console.Read();
        }

        public static class MedianMaintenance
        {
            private static List<int> Medians = new List<int>();
            private static MaxHeap Hlow;
            private static MinHeap Hhigh;
            internal static int Calculate(string[] args)
            {
                Dictionary<int, int> lowItems = new Dictionary<int, int>();
                Dictionary<int, int> highItems = new Dictionary<int, int>();
                List<int> input = System.IO.File.ReadAllLines(args[0]).ToList().Select(int.Parse).ToList();
                Medians.Clear();
                Hlow = new MaxHeap(lowItems);
                Hhigh = new MinHeap(highItems);
                Medians.Add(input[0]);
                Hhigh.Insert(input[0]);

                int count = 1;
                foreach (int item in input.Skip(1))
                {
                    if (item < Medians.Last())
                    {
                        Hlow.Insert(item);
                    }
                    else
                    {
                        Hhigh.Insert(item);
                    }
                    count++;

                    switch (count % 2)
                    {
                        case 0:
                            if (Hhigh.GetCount() != Hlow.GetCount()) // both should be equal
                            {
                                if (Hhigh.GetCount() > Hlow.GetCount()) // move from hhigh to hlow
                                {
                                    Hlow.Insert(Hhigh.Delete());
                                }
                                else
                                {
                                    Hhigh.Insert(Hlow.Delete());
                                }
                            }
                            Medians.Add(Hlow.Extract_Max());
                            break;
                        case 1:
                            if (Hlow.GetCount() > Hhigh.GetCount())
                            {
                                Medians.Add(Hlow.Extract_Max());
                            }
                            else
                            {
                                Medians.Add(Hhigh.Extract_Min());
                            }
                            break;
                    }
                }

                return Medians.Sum() % 10000;

            }
        }

        public class MinHeap
        {
            Dictionary<int, int> Items;

            public MinHeap(Dictionary<int, int> items)
            {
                Items = items ?? throw new ArgumentNullException(nameof(items));
            }

            public int Extract_Min()
            {
                if (Items.Values.Count() == 0)
                {
                    return 999999999;
                }
                return Items.Values.Min();
            }

            public int Insert(int item)
            {
                if (Items.Count() == 0)
                {
                    Items.Add(0, item);
                }
                else
                {
                    Items.Add(Items.Keys.Max() + 1 ,item);
                }
                return Items.Keys.Max();
            }

            public int Delete()
            {
                int minVal = Items.Values.Min();
                int minKey = Items.First(x => x.Value == minVal).Key;
                Items.Remove(minKey);
                return minVal;
            }

            public int GetCount()
            {
                return Items.Count();
            }
        }

        public class MaxHeap
        {
            Dictionary<int, int> Items;

            public MaxHeap(Dictionary<int, int> items)
            {
                Items = items ?? throw new ArgumentNullException(nameof(items));
            }

            public int Extract_Max()
            {
                if (Items.Values.Count() == 0)
                {
                    return 0;
                }
                return Items.Values.Max();
            }

            public int Insert(int item)
            {
                if (Items.Count() == 0)
                {
                    Items.Add(0, item);
                }
                else
                {
                    Items.Add(Items.Keys.Max() + 1, item);
                }
                return Items.Keys.Max();
            }

            public int Delete()
            {
                int maxVal = Items.Values.Max();
                int maxKey = Items.First(x => x.Value == maxVal).Key;
                Items.Remove(maxKey);
                return maxVal;
            }

            public int GetCount()
            {
                return Items.Count();
            }
        }
    }
}
