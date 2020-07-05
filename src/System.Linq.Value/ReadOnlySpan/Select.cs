namespace System.Linq.Value
{
    public static partial class ReadOnlySpanExtensions
    {
        public static SelectEnumerable<T, TResult> ValueSelect<T, TResult>(this ReadOnlySpan<T> source, Func<T, TResult> selector) =>
            new SelectEnumerable<T, TResult>(in source, selector);

        public static SelectWithIndexEnumerable<T, TResult> ValueSelect<T, TResult>(this ReadOnlySpan<T> source, Func<T, int, TResult> selector) =>
            new SelectWithIndexEnumerable<T, TResult>(in source, selector);

        public readonly ref struct SelectEnumerable<T, TResult>
        {
            public SelectEnumerable(in ReadOnlySpan<T> source, Func<T, TResult> selector)
            {
                this.source = source;
                this.selector = selector;
            }

            private readonly ReadOnlySpan<T> source;
            private readonly Func<T, TResult> selector;

            public Enumerator GetEnumerator() => new Enumerator(in source, selector);

            public ref struct Enumerator
            {
                public Enumerator(in ReadOnlySpan<T> source, Func<T, TResult> selector)
                {
                    Current = default!;
                    enumerator = source.GetEnumerator();
                    this.selector = selector;
                }

                private ReadOnlySpan<T>.Enumerator enumerator;
                private readonly Func<T, TResult> selector;

                public TResult Current { get; private set; }

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
            }
        }

        public readonly ref struct SelectWithIndexEnumerable<T, TResult>
        {
            public SelectWithIndexEnumerable(in ReadOnlySpan<T> source, Func<T, int, TResult> selector)
            {
                this.source = source;
                this.selector = selector;
            }

            private readonly ReadOnlySpan<T> source;
            private readonly Func<T, int, TResult> selector;

            public Enumerator GetEnumerator() => new Enumerator(in source, selector);

            public ref struct Enumerator
            {
                public Enumerator(in ReadOnlySpan<T> source, Func<T, int, TResult> selector)
                {
                    Current = default!;
                    enumerator = source.GetEnumerator();
                    this.selector = selector;
                    index = -1;
                }

                private ReadOnlySpan<T>.Enumerator enumerator;
                private readonly Func<T, int, TResult> selector;

                public TResult Current { get; private set; }

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
                    index = -1;
                }
            }
        }
    }
}
