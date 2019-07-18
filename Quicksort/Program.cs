using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicksort
{
    class Program
    {
        /// <summary>
        /// Solution to Week 3 assignment of Coursera Algorithms specialization
        /// 
        /// -------------------------------------------------------------------
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            double[] A = System.IO.File.ReadAllLines(args[0]).Select<string, double>(s => Double.Parse(s)).ToArray<double>();

        }
    }
}
