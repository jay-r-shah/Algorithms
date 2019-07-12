using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inversions
{
    class Program
    {
        /// <summary>
        /// Solution to Week 2 assignment of Coursera Algorithms specialization
        /// -------------------------------------------------------------------
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            double[] A = new double[5] { 1, 4, 8, 12, 13 };
            double[] B = new double[5] { 2, 3, 8, 15, 19 };
            Console.WriteLine("Merged array: [{0}]\n", string.Join(", ", MergeAndCountSplitInv(A, B, out int nSplitInv)));
            Console.Write("Number of inversions: {0}", nSplitInv.ToString());
            Console.Read();
        }

        private static double[] MergeAndCountSplitInv(double[] A, double[] B, out int nSplitInv)
        {
            double[] MergedArray = new double[A.Count() + B.Count()];

            int j = 0;
            int k = 0;
            nSplitInv = 0;
            for (int i = 0; i < MergedArray.Count(); i++)
            {
                if (j == A.Count())
                {
                    for (int l = i; l < MergedArray.Count(); l++)
                    {
                        MergedArray[l] = B[k];
                        k++;
                    }
                    break;
                }

                if (k == B.Count())
                {
                    for (int l = i; l < MergedArray.Count(); l++)
                    {
                        MergedArray[l] = A[j];
                        j++;
                    }
                    break;
                }

                if (A[j] < B[k])
                {
                    MergedArray[i] = A[j];
                    j++;
                }
                else if (B[k] < A[j])
                {
                    MergedArray[i] = B[k];
                    nSplitInv += A.Count() - j;
                    k++;
                }
                else if (A[j] == B[k])
                {
                    MergedArray[i] = A[j];
                    MergedArray[i + 1] = B[k];
                    j++;
                    k++;
                    i++;
                }
            }

            return MergedArray;
        }
    }
}
