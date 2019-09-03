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
                for (int t = -10000; t < 10001; t++)
                {
                    H.Clear();
                    int i = 1;
                    foreach (long val in input)
                    {
                        if (!H.ContainsKey(val))
                        {
                            H.Add(val, i);
                            i++;
                            if (H.Contains(t - val) && t != val)
                            {
                                twoSums++;
                                break;
                            }
                            //Console.Write("{0} \t {1} \n", i, twoSums);
                        }
                    }
                }


                //for (int t = -10000; t < 10001; t++)
                //{
                //    Console.Write("{0} \n", t);
                //    int i = 1;
                //    H.Clear();
                //    foreach (long val in input)
                //    {
                //        if (!H.ContainsKey(val))
                //        {
                //            H.Add(val, i);
                //            i++;
                //            if (H.ContainsKey(t - val) && t != val)
                //            {
                //                twoSums++;
                //                break;
                //            }
                //        }
                //    }
                //}



                //while ((line = file.ReadLine()) != null)
                //{
                //    long val = Int64.Parse(line);
                //    if (!H.ContainsKey(val))
                //    {
                //        H.Add(val, i);
                //        i++;
                //        for (int t = -10000; t < 10001; t++)
                //        {
                //            if (H.ContainsKey(t - val) && t != val)
                //            {
                //                twoSums++;
                //                //break;
                //            }
                //        }
                //    }
                //}
                //}
                return (twoSums, input.Count());
            }
        }
    }
}
