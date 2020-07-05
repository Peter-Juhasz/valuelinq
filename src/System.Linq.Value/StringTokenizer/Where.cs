using Microsoft.Extensions.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class StringTokenizerExtensions
    {
        public static WhereEnumerable ValueWhere(this StringTokenizer source, Func<StringSegment, bool> predicate) =>
            new WhereEnumerable(in source, predicate);

        public static WhereWithIndexEnumerable ValueWhere(this StringTokenizer source, Func<StringSegment, int, bool> predicate) =>
            new WhereWithIndexEnumerable(in source, predicate);

        public readonly struct WhereEnumerable : IEnumerable<StringSegment>
        {
            public WhereEnumerable(in StringTokenizer source, Func<StringSegment, bool> predicate)
            {
                this.source = source;
                this.predicate = predicate;
            }

            private readonly StringTokenizer source;
            private readonly Func<StringSegment, bool> predicate;

            public Enumerator GetEnumerator() => new Enumerator(in source, predicate);

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            IEnumerator<StringSegment> IEnumerable<StringSegment>.GetEnumerator() => this.GetEnumerator();

            public struct Enumerator : IEnumerator<StringSegment>
            {
                public Enumerator(in StringTokenizer source, Func<StringSegment, bool> predicate)
                {
                    Current = default!;
                    enumerator = source.GetEnumerator();
                    this.predicate = predicate;
                }

                private StringTokenizer.Enumerator enumerator;
                private readonly Func<StringSegment, bool> predicate;

                public StringSegment Current { get; private set; }

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

        public readonly struct WhereWithIndexEnumerable : IEnumerable<StringSegment>
        {
            public WhereWithIndexEnumerable(in StringTokenizer source, Func<StringSegment, int, bool> predicate)
            {
                this.source = source;
                this.predicate = predicate;
            }

            private readonly StringTokenizer source;
            private readonly Func<StringSegment, int, bool> predicate;

            public Enumerator GetEnumerator() => new Enumerator(in source, predicate);

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            IEnumerator<StringSegment> IEnumerable<StringSegment>.GetEnumerator() => this.GetEnumerator();

            public struct Enumerator : IEnumerator<StringSegment>
            {
                public Enumerator(in StringTokenizer source, Func<StringSegment, int, bool> predicate)
                {
                    Current = default!;
                    enumerator = source.GetEnumerator();
                    this.predicate = predicate;
                    index = -1;
                }

                private StringTokenizer.Enumerator enumerator;
                private readonly Func<StringSegment, int, bool> predicate;

                public StringSegment Current { get; private set; }

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
