using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableExtensions
    {
        public static ZipEnumerable<T1, T2, TResult> ValueZip<T1, T2, TResult>(this IEnumerable<T1> source, IEnumerable<T2> second, Func<T1, T2, TResult> selector) =>
            new ZipEnumerable<T1, T2, TResult>(source, second, selector);


        public readonly struct ZipEnumerable<T1, T2, TResult> : IEnumerable<TResult>
        {
            public ZipEnumerable(IEnumerable<T1> source, IEnumerable<T2> second, Func<T1, T2, TResult> selector)
            {
                this.source = source;
                this.second = second;
                this.selector = selector;
            }

            private readonly IEnumerable<T1> source;
            private readonly IEnumerable<T2> second;
            private readonly Func<T1, T2, TResult> selector;

            public Enumerator GetEnumerator() => new Enumerator(source, second, selector);

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => this.GetEnumerator();

            public struct Enumerator : IEnumerator<TResult>
            {
                public Enumerator(IEnumerable<T1> source, IEnumerable<T2> second, Func<T1, T2, TResult> selector)
                {
                    Current = default!;
                    enumerator = source.GetEnumerator();
                    this.second = second.GetEnumerator();
                    this.selector = selector;
                }

                private readonly IEnumerator<T1> enumerator;
                private readonly IEnumerator<T2> second;
                private readonly Func<T1, T2, TResult> selector;

                public TResult Current { get; private set; }

                object? IEnumerator.Current => this.Current;


                public bool MoveNext()
                {
                    if (enumerator.MoveNext() && second.MoveNext())
                    {
                        Current = selector(enumerator.Current, second.Current);
                        return true;
                    }

                    return false;
                }

                public void Reset()
                {
                    enumerator.Reset();
                    second.Reset();
                }

                public void Dispose()
                {
                    enumerator.Dispose();
                    second.Dispose();
                }
            }
        }
    }
}
