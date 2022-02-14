using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mouse_Hunter.MovementSimulators
{
    public class BezierCurve
    {
        private static double[] FactorialLookup = new double[]
{
1.0,
1.0,
2.0,
6.0,
24.0,
120.0,
720.0,
5040.0,
40320.0,
362880.0,
3628800.0,
39916800.0,
479001600.0,
6227020800.0,
87178291200.0,
1307674368000.0,
20922789888000.0,
355687428096000.0,
6402373705728000.0,
121645100408832000.0,
2432902008176640000.0,
51090942171709440000.0,
1124000727777607680000.0,
25852016738884976640000.0,
620448401733239439360000.0,
15511210043330985984000000.0,
403291461126605635584000000.0,
10888869450418352160768000000.0,
304888344611713860501504000000.0,
8841761993739701954543616000000.0,
265252859812191058636308480000000.0,
8222838654177922817725562880000000.0,
263130836933693530167218012160000000.0
};


        public void Bezier2D(double[] b, int cpts, double[] p)
        {
            int npts = (b.Length) / 2;
            int icount, jcount;
            double step, t;

            // Calculate points on curve

            icount = 0;
            t = 0;
            step = (double)1.0 / (cpts - 1);

            for (int i1 = 0; i1 != cpts; i1++)
            {
                if ((1.0 - t) < 5e-6)
                    t = 1.0;

                jcount = 0;
                p[icount] = 0.0;
                p[icount + 1] = 0.0;
                for (int i = 0; i != npts; i++)
                {
                    double basis = Bernstein(npts - 1, i, t);
                    p[icount] += basis * b[jcount];
                    p[icount + 1] += basis * b[jcount + 1];
                    jcount = jcount + 2;
                }

                icount += 2;
                t += step;
            }
        }

        private static double Bernstein(int n, int i, double t)
        {
            double basis;
            double ti; /* t^i */
            double tni; /* (1 - t)^i */

            /* Prevent problems with pow */

            if (t == 0.0 && i == 0)
                ti = 1.0;
            else
                ti = Math.Pow(t, i);

            if (n == i && t == 1.0)
                tni = 1.0;
            else
                tni = Math.Pow((1 - t), (n - i));

            //Bernstein basis
            basis = Ni(n, i) * ti * tni;
            return basis;
        }
        private static double Ni(int n, int i)
        {
            double ni;
            double a1 = FactorialLookup[n];
            double a2 = FactorialLookup[i];
            double a3 = FactorialLookup[n - i];
            //double a1 = factorial(n);
            //double a2 = factorial(i);
            //double a3 = factorial(n - i);
            ni = a1 / (a2 * a3);
            return ni;
        }
    }
}
