using Microsoft.Extensions.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq.Value
{
    public static partial class StringTokenizerExtensions
    {
        public static SelectEnumerable<TResult> ValueSelect<TResult>(this StringTokenizer source, Func<StringSegment, TResult> selector) =>
            new SelectEnumerable<TResult>(in source, selector);

        public static SelectWithIndexEnumerable<TResult> ValueSelect<TResult>(this StringTokenizer source, Func<StringSegment, int, TResult> selector) =>
            new SelectWithIndexEnumerable<TResult>(in source, selector);

        public readonly struct SelectEnumerable<TResult> : IEnumerable<TResult>
        {
            public SelectEnumerable(in StringTokenizer source, Func<StringSegment, TResult> selector)
            {
                this.source = source;
                this.selector = selector;
            }

            private readonly StringTokenizer source;
            private readonly Func<StringSegment, TResult> selector;

            public Enumerator GetEnumerator() => new Enumerator(in source, selector);

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => this.GetEnumerator();

            public struct Enumerator : IEnumerator<TResult>
            {
                public Enumerator(in StringTokenizer source, Func<StringSegment, TResult> selector)
                {
                    Current = default!;
                    enumerator = source.GetEnumerator();
                    this.selector = selector;
                }

                private StringTokenizer.Enumerator enumerator;
                private readonly Func<StringSegment, TResult> selector;

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

                public void Reset() => enumerator.Reset();

                public void Dispose() => enumerator.Dispose();
            }
        }

        public readonly struct SelectWithIndexEnumerable<TResult> : IEnumerable<TResult>
        {
            public SelectWithIndexEnumerable(in StringTokenizer source, Func<StringSegment, int, TResult> selector)
            {
                this.source = source;
                this.selector = selector;
            }

            private readonly StringTokenizer source;
            private readonly Func<StringSegment, int, TResult> selector;

            public Enumerator GetEnumerator() => new Enumerator(in source, selector);

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => this.GetEnumerator();

            public struct Enumerator : IEnumerator<TResult>
            {
                public Enumerator(in StringTokenizer source, Func<StringSegment, int, TResult> selector)
                {
                    Current = default!;
                    enumerator = source.GetEnumerator();
                    this.selector = selector;
                    index = -1;
                }

                private StringTokenizer.Enumerator enumerator;
                private readonly Func<StringSegment, int, TResult> selector;

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
