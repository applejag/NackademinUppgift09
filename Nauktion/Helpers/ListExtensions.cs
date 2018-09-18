using System.Collections.Generic;

namespace Nauktion.Helpers
{
    public static class ListExtensions
    {
        public static IEnumerable<T> EveryOther<T>(this IEnumerable<T> list, int skipEvery = 2)
        {
            var counter = 0;

            foreach (T value in list)
            {
                counter++;

                if (counter == skipEvery)
                    counter = 0;
                else
                    yield return value;
            }
        }
    }
}