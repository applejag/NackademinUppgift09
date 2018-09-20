using System;
using System.Collections.Generic;
using System.Linq;

namespace Nauktion.Helpers
{
    public static class MathHelpers
    {
        public static int DivCeil(int a, int b)
        {
            if (b == 0) throw new DivideByZeroException();

            int flooredDiv = Math.DivRem(a, b, out int remainder);

            // Divided evenly
            if (remainder == 0)
                return flooredDiv;
            
            bool sameSign = a < 0 == b < 0;

            if (sameSign)
                return flooredDiv + 1;
            
            return flooredDiv;
        }

        public static double Median(this IEnumerable<int> source)
        {
            int[] array = source.OrderBy(n => n).ToArray();
            if (array.Length == 0)
                return 0;
            if (array.Length == 1)
                return array[0];

            int mid = Math.DivRem(array.Length, 2, out int rem);
            if (rem == 0)
                return array[mid];

            return (array[mid] + array[mid - 1]) / 2d;
        }
    }
}