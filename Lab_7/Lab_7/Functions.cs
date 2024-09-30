using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public static class Functions
    {
        public static double Function1(double x)
        {
            return (1 + 2 * Math.Pow(x, 2)) * Math.Exp(Math.Pow(x, 2));
        }

        public static double Function2(double x)
        {
            return Math.Sin(x) + Math.Cos(x);
        }

        public static double Function3(double x)
        {
            return Math.Log(x + 1) + Math.Sqrt(x);
        }

        public static double Function4(double x)
        {
            return Math.Sqrt(x) + Math.Sin(x);
        }

        public static double Function5(double x)
        {
            return Math.Pow(x, 3) * Math.Exp(2);
        }

        public static double Function6(double x)
        {
            return 4 * Math.PI * Math.Pow(x, 2);
        }

        public static double DefaultFunction(double x)
        {
            return Math.Pow(x, 2);
        }
    }
}
