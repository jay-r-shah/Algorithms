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
        static void Main(string[] args)
        {
            Console.Write(Multiply("125","15"));
            Console.Read();
        }

        private static string Multiply(string X, string Y)
        {
            if (X.Length <= 1 && Y.Length<=1)
            {
                if (String.IsNullOrEmpty(X))
                {
                    X = "0";
                }

                if (String.IsNullOrEmpty(Y))
                {
                    Y = "0";
                }
                return Convert.ToString(Convert.ToInt32(X) * Convert.ToInt32(Y));
            }


            string A = string.IsNullOrEmpty(X.Substring(0, X.Length / 2)) ? "0" : X.Substring(0, X.Length / 2);
            string B = string.IsNullOrEmpty(X.Substring(A.Length)) ? "0" : X.Substring(A.Length);
            string C = string.IsNullOrEmpty(Y.Substring(0, Y.Length / 2)) ? "0" : Y.Substring(0, Y.Length / 2);
            string D = string.IsNullOrEmpty(Y.Substring(C.Length)) ? "0" : Y.Substring(C.Length);



            string AC = Multiply(A, C);
            string BD = Multiply(B, D);

            BigInteger AplB = BigInteger.Parse(A) + BigInteger.Parse(B);
            BigInteger CplD = BigInteger.Parse(C) + BigInteger.Parse(D);

            string AplBtimesCplD = Multiply(Convert.ToString(AplB),Convert.ToString(CplD));

            string ADplBC = Convert.ToString(BigInteger.Parse(AplBtimesCplD) - BigInteger.Parse(AC) - BigInteger.Parse(BD));

            string term1 = AC + string.Concat(Enumerable.Repeat('0', X.Length));
            string term2 = ADplBC + string.Concat(Enumerable.Repeat('0', X.Length / 2));
            string term3 = BD;

            BigInteger product = BigInteger.Parse(term1) + BigInteger.Parse(term2) + BigInteger.Parse(term3);

            return Convert.ToString(product);

            //string AplB = Convert.ToString(Numerics.B)
            //string ApB = Convert.ToString()
            //string ApBtimesCpD = Multiply()

            return null;
        }
    }
}
