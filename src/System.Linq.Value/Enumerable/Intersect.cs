using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableExtensions
    {
        public static WhereEnumerable<T> ValueIntersect<T>(this IEnumerable<T> source, IEnumerable<T> second) =>
            source.ValueIntersect(second, EqualityComparer<T>.Default);

        public static WhereEnumerable<T> ValueIntersect<T>(this IEnumerable<T> source, IEnumerable<T> second, IEqualityComparer<T> comparer) =>
            source.ValueWhere(p => second.Contains(p, comparer));
    }
}
