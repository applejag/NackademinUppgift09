using System;
using System.Collections.Generic;
using System.Linq;

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

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> list, bool filter, Func<T, bool> predicate)
        {
            return filter 
                ? list.Where(predicate) 
                : list;
        }
    }
}