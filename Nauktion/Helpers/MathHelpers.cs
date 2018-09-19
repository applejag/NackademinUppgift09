using System;

namespace Nauktion.Helpers
{
    public class MathHelpers
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
    }
}