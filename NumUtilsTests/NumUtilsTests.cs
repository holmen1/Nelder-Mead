using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using RUT.NelderMeadSimplex;

namespace NumUtilsTests
{
    public class NumUtilsTests
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting first simplex test...");
            _simplexTest1();
            Console.WriteLine("Hit any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Test to see if we can fit a parabola
        /// </summary>
        private static void _simplexTest1()
        {
            Console.WriteLine("Starting SimplexTest1");
            SimplexConstant[] constants = new SimplexConstant[] { new SimplexConstant(3, 1), new SimplexConstant(5, 1) };
            double[] tt = { 1.0, 7.3 };
            double[] Y = { 15.0, 339.45 };
            double tolerance = 1e-6;
            int maxEvals = 1000;
            ObjectiveFunctionDelegate objFunction = new ObjectiveFunctionDelegate(_objFunction1);
            RegressionResult result = NelderMeadSimplex.Regress(constants, tt, Y, tolerance, maxEvals, objFunction);
            _printResult(result);
        }

        private static double _objFunction1(double[] constants, double[] tt, double[] Y)
        {
            double a = 5;
            double b = 10;

            Console.Write("Called with a={0} b={1}", constants[0], constants[1]);

            // evaluate it for some selected points, with a bit of noise
            double ssq = 0;
            for (int i = 0; i < tt.Length; i++)
            {
                double yRegress = constants[0] * Math.Pow(tt[i], 2) + constants[1] * tt[i];
                ssq += Math.Pow((Y[i] - yRegress), 2);
            }
            Console.WriteLine("  SSQ={0}", ssq);
            return ssq;
        }

        private static void _printResult(RegressionResult result)
        {
            // a bit of reflection fun, why not...
            PropertyInfo[] properties = result.GetType().GetProperties();
            foreach (PropertyInfo p in properties)
            {
                Console.WriteLine(p.Name + ": " + p.GetValue(result, null).ToString());
            }
        }
    }
}
