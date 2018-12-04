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
            SimplexConstant[] nss = new SimplexConstant[] { new SimplexConstant(0, 1), new SimplexConstant(0, 1), new SimplexConstant(0, 1), new SimplexConstant(0, 1), new SimplexConstant(1, 1), new SimplexConstant(1, 1) };
            double[] tt = { 5.0, 10.0, 15.0, 20.0, 25.0};
            double[] Y = { 0.001770949, 0.008396027, 0.013860686, 0.019379306, 0.023731833 };
            double tolerance = 1e-6;
            int maxEvals = 1000;
            ObjectiveFunctionDelegate objFunction = new ObjectiveFunctionDelegate(_objFunction1);
            RegressionResult result = NelderMeadSimplex.Regress(nss, tt, Y, tolerance, maxEvals, objFunction);
            _printResult(result);
        }

        private static double _objFunction1(double[] x, double[] tt, double[] Y)
        {
            Console.Write("Called with beta0={0} beta1={1} beta2={2} beta3={3} tau0={4} tau1={5}", x[0], x[1], x[2], x[3], x[4], x[5]);
            if (!(x[4] > 0.0 && x[5] > 0.0))
                return 100.0;
            else
            {
                double ssq = 0;
                for (int i = 0; i < tt.Length; i++)
                {
                    double yRegress = delta_func(x, tt[i]);
                    ssq += Math.Pow((Y[i] - yRegress), 2);
                }
                Console.WriteLine("  SSQ={0}", ssq);
                return ssq;
            }

            double delta_func(double[] nss, double t)
            {

                if (t < Double.Epsilon)
                    return nss[0] + nss[1];
                else
                    return nss[0] + nss[1] * (1 - Math.Exp(-t / nss[4])) / (t / nss[4])
                                + nss[2] * ((1 - Math.Exp(-t / nss[4])) / (t / nss[4]) - Math.Exp(-t / nss[4]))
                                + nss[3] * ((1 - Math.Exp(-t / nss[5])) / (t / nss[5]) - Math.Exp(-t / nss[5]));
            }
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
