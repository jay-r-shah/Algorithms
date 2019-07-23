using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicksort
{
    class Program
    {
        private static Random rand = new Random();

        /// <summary>
        /// Solution to Week 3 assignment of Coursera Algorithms specialization
        /// 
        /// -------------------------------------------------------------------
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            double[] A = new double[12] { 3, 8, 2, 5, 1, 4, 7, 6, 4, 4, 6, 3 };
            //double[] A = new double[2] { 1, 2 };
            //double[] A = System.IO.File.ReadAllLines(args[0]).Select<string, double>(s => Double.Parse(s)).ToArray<double>();
            double[] B = QuickSort(A);
            Console.WriteLine("Merged array: [{0}]\n", string.Join(", ", B));
            Console.Read();
        }

        private static double[] QuickSort(double[] A)
        {
            int n = A.Count();
            if (n < 1)
                return A;

            int p = SelectPivotElement(A);
            int pivotIndex;
            (A, pivotIndex) = Partition(A, p);
            
            // QuickSort(A[:p - 1]) + A[p] + QuickSort(A[p + 1:]) - in python parlance

            return QuickSort(A.Take(pivotIndex).ToArray()).Append(A[pivotIndex]).Concat(QuickSort(A.Skip(pivotIndex + 1).ToArray())).ToArray();
        }

        /// <summary>
        /// Partition array around a pivot element
        /// </summary>
        /// <param name="A">Input array</param>
        /// <param name="l">Pivot index</param>
        /// <returns>Partitioned array</returns>
        private static (double[], int) Partition(double[] A, int l = 0)
        {
            if (A.Count() == 0)
            {
                return (A, 0);
            }
            if (l != 0) // swap A[l] and A[0]
            {
                double temp1 = A[l];
                A[l] = A[0];
                A[0] = temp1;
                l = 0;
            }

            int i = l + 1;
            double pivotElement = A[l];
            int r = A.Count();
            for (int j = l + 1; j < r; j++) // boundary between what you've seen and what you haven't
            {
                if (A[j] < pivotElement)
                {
                    double temp0 = A[j];
                    A[j] = A[i];
                    A[i] = temp0;
                    i++;
                } // else do nothing
            }

            // reswap first element
            double temp = A[l];
            A[l] = A[i - 1];
            A[i - 1] = temp;
            return (A, i - 1);
        }

        private static int SelectPivotElement(double[] A)
        {
            return rand.Next(A.Count());
            //return 0;
        }
    }
}
