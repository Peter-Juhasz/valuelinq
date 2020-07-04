using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq.Value
{
    public static partial class EnumerableExtensions
    {
        public static SkipWhileEnumerable<T> ValueSkipWhile<T>(this IEnumerable<T> source, Func<T, bool> predicate) =>
            new SkipWhileEnumerable<T>(source, predicate);

        public readonly struct SkipWhileEnumerable<T> : IEnumerable<T>
        {
            public SkipWhileEnumerable(IEnumerable<T> source, Func<T, bool> predicate)
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
                    state = 0;
                }

                private readonly IEnumerator<T> enumerator;
                private readonly Func<T, bool> predicate;

                public T Current { get; private set; }

                object? IEnumerator.Current => this.Current;

                private int state;

                public bool MoveNext()
                {
                    switch (state)
                    {
                        case 0:
                            bool hasNext;
                            do
                            {
                                hasNext = enumerator.MoveNext();
                            } while (hasNext && predicate(enumerator.Current));

                            if (!hasNext)
                            {
                                return false;
                            }

                            state = 1;
                            Current = enumerator.Current;
                            return true;
                            break;

                        case 1:
                            if (enumerator.MoveNext())
                            {
                                Current = enumerator.Current;
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                            break;

                        default: throw new InvalidOperationException();
                    }
                }

                public void Reset()
                {
                    enumerator.Reset();
                    state = 0;
                }

                public void Dispose() => enumerator.Dispose();
            }
        }

    }
}
