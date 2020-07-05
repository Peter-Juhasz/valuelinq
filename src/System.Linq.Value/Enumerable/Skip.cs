using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableExtensions
    {
        public static SkipEnumerable<T> ValueSkip<T>(this IEnumerable<T> source, int count) =>
            new SkipEnumerable<T>(source, count);


        public readonly struct SkipEnumerable<T> : IEnumerable<T>
        {
            public SkipEnumerable(IEnumerable<T> source, int count)
            {
                this.source = source;
                this.count = count;
            }

            private readonly IEnumerable<T> source;
            private readonly int count;

            public Enumerator GetEnumerator() => new Enumerator(source, count);

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.GetEnumerator();

            public struct Enumerator : IEnumerator<T>
            {
                public Enumerator(IEnumerable<T> source, int count)
                {
                    Current = default!;
                    enumerator = source.GetEnumerator();
                    this.count = count;
                    index = 0;
                }

                private readonly IEnumerator<T> enumerator;
                private readonly int count;

                public T Current { get; private set; }

                object? IEnumerator.Current => this.Current;

                private int index;

                public bool MoveNext()
                {
                    while (index < count)
                    {
                        if (!enumerator.MoveNext())
                            return false;

                        index++;
                    }

                    if (enumerator.MoveNext())
                    {
                        Current = enumerator.Current;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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
