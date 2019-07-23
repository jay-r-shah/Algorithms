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
        private static int nComparisions = 0;
        private static int totalComp = 0;
        private enum PivotMethod { First, Last, MedianOfThree, Random };
        private static PivotMethod _pivotMethod = PivotMethod.MedianOfThree;
        /// <summary>
        /// Solution to Week 3 assignment of Coursera Algorithms specialization
        /// Find number of comparisions required for QuickSort Algorithm
        /// -------------------------------------------------------------------
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            double[] A = System.IO.File.ReadAllLines(args[0]).Select<string, double>(s => Double.Parse(s)).ToArray<double>();
            double[] B = QuickSort(A);
            Console.WriteLine("Sorted array: [{0}]\n", string.Join(", ", B));
            Console.WriteLine("Pivot method: {0}", _pivotMethod.ToString());
            Console.WriteLine("Number of comparisions: {0}", Convert.ToString(nComparisions));
            Console.Read();
        }

        private static double[] QuickSort(double[] A)
        {
            int n = A.Count();
            if (n < 1)
                return A;
            nComparisions += (n - 1);

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
                totalComp += 1;
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
            switch (_pivotMethod)
            {
                case PivotMethod.First:
                    return 0;
                case PivotMethod.Last:
                    return A.Count() - 1;
                case PivotMethod.MedianOfThree:
                    if (A.Count() < 2)
                    {
                        return 0;
                    }

                    if (A.Count() == 2)
                    {
                        if (A.First() <= A.Last())
                        {
                            return 0;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    double first = A.First();
                    double last = A.Last();
                    double middle = A[(2 * A.Count() - 1) / 4];

                    double[] x = new double[3] { first, last, middle };
                    int median = 0;

                    if ((last > first && last < middle) || (last < first && last > middle))
                        median = A.Count() - 1;

                    if ((middle > first && middle < last) || (middle < first && middle > last))
                        median = (2 * A.Count() - 1) / 4;

                    return median;
                case PivotMethod.Random:
                    return rand.Next(A.Count());
                default:
                    return 0;
            }
        }
    }
}
