using System;
using System.Linq;

namespace PointGraph.Math
{
    public static class PolynomialSolver
    {
        /// <summary>
        /// Solves for coefficients of a polynomial of degree n passing through n+1 points.
        /// Returns coefficients [a_0, a_1, ..., a_n] where P(x) = a_0 + a_1*x + a_2*x^2 + ...
        /// Uses Newton's divided differences method. Complexity: O(n^2).
        /// </summary>
        public static double[] SolvePolynomialParameters(double[] x, double[] y)
        {
            if (x.Length != y.Length || x.Length == 0)
            {
                throw new ArgumentException("X and Y arrays must be of equal length and contain at least one element.");
            }

            int n = x.Length - 1;
            double[] coef = new double[y.Length];
            Array.Copy(y, coef, y.Length);

            for (int j = 1; j <= n; j++)
            {
                for (int i = n; i >= j; i--)
                {
                    double denom = x[i] - x[i - j];
                    if (System.Math.Abs(denom) < 1e-12)
                    {
                        denom = 1e-12;
                    }
                    coef[i] = (coef[i] - coef[i - 1]) / denom;
                }
            }

            double[] result = new double[n + 1];
            result[0] = coef[n];
            int currentDegree = 0;

            for (int i = n - 1; i >= 0; i--)
            {
                currentDegree++;
                for (int k = currentDegree; k > 0; k--)
                {
                    result[k] = result[k - 1] - result[k] * x[i];
                }
                result[0] = result[0] * (-x[i]) + coef[i];
            }

            return result;
        }

        /// <summary>
        /// Exact polynomial solving using Fraction type.
        /// </summary>
        public static Fraction[] SolvePolynomialParameters(Fraction[] x, Fraction[] y)
        {
            if (x.Length != y.Length || x.Length == 0)
            {
                throw new ArgumentException("X and Y arrays must be of equal length and contain at least one element.");
            }

            int n = x.Length - 1;
            Fraction[] coef = new Fraction[y.Length];
            Array.Copy(y, coef, y.Length);

            for (int j = 1; j <= n; j++)
            {
                for (int i = n; i >= j; i--)
                {
                    Fraction denom = x[i] - x[i - j];
                    if (denom.Numerator == 0)
                    {
                        // Avoid division by zero
                        denom = new Fraction(1, 1000000000000L);
                    }
                    coef[i] = (coef[i] - coef[i - 1]) / denom;
                }
            }

            // Fraction.Zero (0/1) で初期化することで、デフォルト値 (0/0) による演算エラーを防ぐ
            Fraction[] result = new Fraction[n + 1];
            for (int k = 0; k <= n; k++) result[k] = Fraction.Zero;

            result[0] = coef[n];
            int currentDegree = 0;

            for (int i = n - 1; i >= 0; i--)
            {
                currentDegree++;
                for (int k = currentDegree; k > 0; k--)
                {
                    result[k] = result[k - 1] - result[k] * x[i];
                }
                result[0] = result[0] * (-x[i]) + coef[i];
            }

            return result;
        }
    }
}
