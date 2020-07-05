using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableExtensions
    {
        public static TakeEnumerable<T> ValueTake<T>(this IEnumerable<T> source, int count) =>
            new TakeEnumerable<T>(source, count);


        public readonly struct TakeEnumerable<T> : IEnumerable<T>
        {
            public TakeEnumerable(IEnumerable<T> source, int count)
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
                    if (index == count)
                    {
                        return false;
                    }

                    if (enumerator.MoveNext())
                    {
                        index++;
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
