using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableExtensions
    {
        public static TakeWhileEnumerable<T> ValueTakeWhile<T>(this IEnumerable<T> source, Func<T, bool> predicate) =>
            new TakeWhileEnumerable<T>(source, predicate);

        public readonly struct TakeWhileEnumerable<T> : IEnumerable<T>
        {
            public TakeWhileEnumerable(IEnumerable<T> source, Func<T, bool> predicate)
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
                    if (enumerator.MoveNext())
                    {
                        var current = enumerator.Current;
                        if (predicate(current))
                        {
                            Current = current;
                            return true;
                        }
                        else
                        {
                            return false;
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

    }
}
