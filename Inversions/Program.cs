using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Inversions
{
    class Program
    {
        /// <summary>
        /// Solution to Week 2 assignment of Coursera Algorithms specialization
        /// 
        /// Compute the number of inversions in the given text file.
        /// -------------------------------------------------------------------
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            double[] A = System.IO.File.ReadAllLines(args[0]).Select<string, double>(s => Double.Parse(s)).ToArray<double>();
            Console.WriteLine("Merged array: [{0}]\n", string.Join(", ", SortAndCount(A, out BigInteger nInversions)));
            Console.Write("Number of inversions: {0}", nInversions.ToString());
            Console.Read();
        }

        private static double[] SortAndCount(double[] A, out BigInteger nInversions)
        {
            int n = A.Count();
            nInversions = 0;
            if (n == 1)
            {
                return A;
            }
            // First half of A
            double[] A0 = A.Take(n / 2).ToArray();
            // Second half of A
            double[] A1 = A.Skip(n / 2).ToArray();

            double[] B = SortAndCount(A0, out BigInteger X);
            double[] C = SortAndCount(A1, out BigInteger Y);
            double[] D = MergeAndCountSplitInv(B, C, out BigInteger Z);

            nInversions = X + Y + Z;
            return D;
        }

        private static double[] MergeAndCountSplitInv(double[] A, double[] B, out BigInteger nSplitInv)
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
                    nSplitInv += A.Count() - j;
                }
            }

            return MergedArray;
        }
    }
}
