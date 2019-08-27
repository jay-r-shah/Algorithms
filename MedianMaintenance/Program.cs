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
                    Console.Write("Correct = {0:F2}% \t {1}/{2} \t nNodes = {3} \t time = {4:F2}", (double)correct * 100 / total, total, totalInputFiles, result, (double)start.ElapsedMilliseconds / 1000);
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

            internal static int Calculate(string[] args)
            {
                return 0;
            }
        }
    }
}
