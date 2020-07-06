using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class ReadOnlyListExtensions
    {
        public static SelectEnumerable<T, TResult> ValueSelect<T, TResult>(this IReadOnlyList<T> source, Func<T, TResult> selector) =>
            new SelectEnumerable<T, TResult>(source, selector);

        public static SelectWithIndexEnumerable<T, TResult> ValueSelect<T, TResult>(this IReadOnlyList<T> source, Func<T, int, TResult> selector) =>
            new SelectWithIndexEnumerable<T, TResult>(source, selector);


        public readonly struct SelectEnumerable<T, TResult> : IEnumerable<TResult>
        {
            public SelectEnumerable(IReadOnlyList<T> source, Func<T, TResult> selector)
            {
                this.source = source;
                this.selector = selector;
            }

            private readonly IReadOnlyList<T> source;
            private readonly Func<T, TResult> selector;

            public Enumerator GetEnumerator() => new Enumerator(source, selector);

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => this.GetEnumerator();

            public struct Enumerator : IEnumerator<TResult>
            {
                public Enumerator(IReadOnlyList<T> source, Func<T, TResult> selector)
                {
                    Current = default!;
                    enumerator = source;
                    this.selector = selector;
                    index = -1;
                }

                private readonly IReadOnlyList<T> enumerator;
                private readonly Func<T, TResult> selector;

                public TResult Current { get; private set; }

                object? IEnumerator.Current => this.Current;

                private int index;

                public bool MoveNext()
                {
                    if (++index < enumerator.Count)
                    {
                        Current = selector(enumerator[index]);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                public void Reset()
                {
                    index = -1;
                }

                public void Dispose() { }
            }
        }

        public readonly struct SelectWithIndexEnumerable<T, TResult> : IEnumerable<TResult>
        {
            public SelectWithIndexEnumerable(IReadOnlyList<T> source, Func<T, int, TResult> selector)
            {
                this.source = source;
                this.selector = selector;
            }

            private readonly IReadOnlyList<T> source;
            private readonly Func<T, int, TResult> selector;

            public Enumerator GetEnumerator() => new Enumerator(source, selector);

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => this.GetEnumerator();

            public struct Enumerator : IEnumerator<TResult>
            {
                public Enumerator(IReadOnlyList<T> source, Func<T, int, TResult> selector)
                {
                    Current = default!;
                    enumerator = source;
                    this.selector = selector;
                    index = -1;
                }

                private readonly IReadOnlyList<T> enumerator;
                private readonly Func<T, int, TResult> selector;

                public TResult Current { get; private set; }

                object? IEnumerator.Current => this.Current;

                private int index;

                public bool MoveNext()
                {
                    if (++index < enumerator.Count)
                    {
                        Current = selector(enumerator[index], index);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                public void Reset()
                {
                    index = -1;
                }

                public void Dispose() { }
            }
        }
    }
}
