using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq.Value
{
    public static partial class EnumerableExtensions
    {
        public static SelectEnumerable<T, TResult> ValueSelect<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector) =>
            new SelectEnumerable<T, TResult>(source, selector);

        public static SelectWithIndexEnumerable<T, TResult> ValueSelect<T, TResult>(this IEnumerable<T> source, Func<T, int, TResult> selector) =>
            new SelectWithIndexEnumerable<T, TResult>(source, selector);


        public readonly struct SelectEnumerable<T, TResult> : IEnumerable<TResult>
        {
            public SelectEnumerable(IEnumerable<T> source, Func<T, TResult> selector)
            {
                this.source = source;
                this.selector = selector;
            }

            private readonly IEnumerable<T> source;
            private readonly Func<T, TResult> selector;

            public Enumerator GetEnumerator() => new Enumerator(source, selector);

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => this.GetEnumerator();

            public struct Enumerator : IEnumerator<TResult>
            {
                public Enumerator(IEnumerable<T> source, Func<T, TResult> selector)
                {
                    Current = default!;
                    enumerator = source.GetEnumerator();
                    this.selector = selector;
                }

                private readonly IEnumerator<T> enumerator;
                private readonly Func<T, TResult> selector;

                public TResult Current { get; private set; }

                object? IEnumerator.Current => this.Current;

                public bool MoveNext()
                {
                    if (enumerator.MoveNext())
                    {
                        Current = selector(enumerator.Current);
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
                }

                public void Dispose() => enumerator.Dispose();
            }
        }

        public readonly struct SelectWithIndexEnumerable<T, TResult> : IEnumerable<TResult>
        {
            public SelectWithIndexEnumerable(IEnumerable<T> source, Func<T, int, TResult> selector)
            {
                this.source = source;
                this.selector = selector;
            }

            private readonly IEnumerable<T> source;
            private readonly Func<T, int, TResult> selector;

            public Enumerator GetEnumerator() => new Enumerator(source, selector);

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => this.GetEnumerator();

            public struct Enumerator : IEnumerator<TResult>
            {
                public Enumerator(IEnumerable<T> source, Func<T, int, TResult> selector)
                {
                    Current = default!;
                    enumerator = source.GetEnumerator();
                    this.selector = selector;
                    index = -1;
                }

                private readonly IEnumerator<T> enumerator;
                private readonly Func<T, int, TResult> selector;

                public TResult Current { get; private set; }

                object? IEnumerator.Current => this.Current;

                private int index;

                public bool MoveNext()
                {
                    if (enumerator.MoveNext())
                    {
                        index++;
                        Current = selector(enumerator.Current, index);
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
