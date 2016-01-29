using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Debug
{
    public static class ObjectLevenshteinDistance
    {
        /// <summary>
        /// Compute the distance between two strings.
        /// </summary>
        public static float Compute<TValue>(TValue[] s, TValue[] t, Func<TValue, TValue, float> equalityProbability)
        {
            int n = s.Length;
            int m = t.Length;
            var d = new float[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    float cost = 1 - equalityProbability(t[j - 1], s[i - 1]);

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(
                            d[i - 1, j] + 1, 
                            d[i, j - 1] + 1
                        ),
                        d[i - 1, j - 1] + cost
                    );
                }
            }
            // Step 7
            return d[n, m];
        }
    }
}
