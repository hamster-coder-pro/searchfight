using System.Collections.Generic;
using System.Linq;

namespace Search.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Safe<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }
    }
}