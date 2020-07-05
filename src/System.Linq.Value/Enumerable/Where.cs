using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq.Value
{
    public static partial class EnumerableExtensions
    {
        public static WhereEnumerable<T> ValueWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate) =>
            new WhereEnumerable<T>(source, predicate);

        public static WhereWithIndexEnumerable<T> ValueWhere<T>(this IEnumerable<T> source, Func<T, int, bool> predicate) =>
            new WhereWithIndexEnumerable<T>(source, predicate);

        public readonly struct WhereEnumerable<T> : IEnumerable<T>
        {
            public WhereEnumerable(IEnumerable<T> source, Func<T, bool> predicate)
            {
                this.source = source;
                this.predicate = predicate;
            }

            private readonly IEnumerable<T> source;
            private readonly Func<T, bool> predicate;

            public Enumerator GetEnumerator() => new Enumerator(source, predicate);

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.GetEnumerator();

            public struct Enumerator : IEnumerator<T>
            {
                public Enumerator(IEnumerable<T> source, Func<T, bool> predicate)
                {
                    Current = default!;
                    enumerator = source.GetEnumerator();
                    this.predicate = predicate;
                }

                private readonly IEnumerator<T> enumerator;
                private readonly Func<T, bool> predicate;

                public T Current { get; private set; }

                object? IEnumerator.Current => this.Current;

                public bool MoveNext()
                {
                    while (enumerator.MoveNext())
                    {
                        var current = enumerator.Current;
                        if (predicate(current))
                        {
                            Current = current;
                            return true;
                        }
                    }

                    return false;
                }

                public void Reset()
                {
                    enumerator.Reset();
                }

                public void Dispose() => enumerator.Dispose();
            }
        }

        public readonly struct WhereWithIndexEnumerable<T> : IEnumerable<T>
        {
            public WhereWithIndexEnumerable(IEnumerable<T> source, Func<T, int, bool> predicate)
            {
                this.source = source;
                this.predicate = predicate;
            }

            private readonly IEnumerable<T> source;
            private readonly Func<T, int, bool> predicate;

            public Enumerator GetEnumerator() => new Enumerator(source, predicate);

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.GetEnumerator();

            public struct Enumerator : IEnumerator<T>
            {
                public Enumerator(IEnumerable<T> source, Func<T, int, bool> predicate)
                {
                    Current = default!;
                    enumerator = source.GetEnumerator();
                    this.predicate = predicate;
                    index = -1;
                }

                private readonly IEnumerator<T> enumerator;
                private readonly Func<T, int, bool> predicate;

                public T Current { get; private set; }

                object? IEnumerator.Current => this.Current;

                private int index;

                public bool MoveNext()
                {
                    while (enumerator.MoveNext())
                    {
                        index++;
                        var current = enumerator.Current;
                        if (predicate(current, index))
                        {
                            Current = current;
                            return true;
                        }
                    }

                    return false;
                }

                public void Reset()
                {
                    enumerator.Reset();
                    index = -1;
                }

                public void Dispose() => enumerator.Dispose();
            }
        }
    }
}
