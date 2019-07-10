using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Algorithms
{
    class Program
    {
        /// <summary>
        /// Karatsuba multiplication. Enter the two integers to be multiplied as command line arguments.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.Write("Karatsuba Algorithm:\t");
            string karatsuba = Multiply(args[0], args[1]);
            Console.Write(karatsuba);

            try
            {
                string actual = Convert.ToString(Convert.ToInt64(args[0]) * Convert.ToInt64(args[1]));
                Console.Write("\nActual value:\t\t");
                Console.Write(actual);
            }
            catch (Exception ex)
            {
                // Assuming exception is because of large input
                Console.Write("\nInput too large to calculate actual");
            }
            Console.Write("\nPress any key to continue...");
            Console.Read();
        }

        private static string Multiply(string X, string Y)
        {
            if (X.Length != Y.Length)
            {
                // if the lengths of the numbers are different, pad the shorter number with 0s
                if (X.Length < Y.Length)
                    X = string.Concat(Enumerable.Repeat('0', Y.Length - X.Length)) + X;
                else
                    Y = string.Concat(Enumerable.Repeat('0', X.Length - Y.Length)) + Y;
            }


            // Base case
            if (X.Length == 1 && Y.Length == 1)
                return Convert.ToString(Convert.ToInt32(X) * Convert.ToInt32(Y));

            int n = X.Length;

            string A = X.Substring(0, n / 2);
            string B = X.Substring(A.Length);
            string C = Y.Substring(0, n / 2);
            string D = Y.Substring(C.Length);

            // STEP I
            string AC = Multiply(A, C);

            // STEP II
            string BD = Multiply(B, D);

            // Convert to BigInteger to be able to add
            BigInteger AplB = BigInteger.Parse(A) + BigInteger.Parse(B);
            BigInteger CplD = BigInteger.Parse(C) + BigInteger.Parse(D);

            // STEP III
            string AplBtimesCplD = Multiply(Convert.ToString(AplB),Convert.ToString(CplD));

            string ADplBC = Convert.ToString(BigInteger.Parse(AplBtimesCplD) - BigInteger.Parse(AC) - BigInteger.Parse(BD));

            // Pad with 0's at the end
            string term1 = AC + string.Concat(Enumerable.Repeat('0', ((n + 1) / 2) * 2));
            string term2 = ADplBC + string.Concat(Enumerable.Repeat('0', (n + 1) / 2));
            string term3 = BD;

            BigInteger product = BigInteger.Parse(term1) + BigInteger.Parse(term2) + BigInteger.Parse(term3);

            return Convert.ToString(product);
        }
    }
}
