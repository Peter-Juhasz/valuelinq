using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableExtensions
    {
        public static PrependEnumerable<T> ValuePrepend<T>(this IEnumerable<T> source, T value) =>
            new PrependEnumerable<T>(source, value);

        public readonly struct PrependEnumerable<T> : IEnumerable<T>
        {
            public PrependEnumerable(IEnumerable<T> source, T value)
            {
                this.source = source;
                this.value = value;
            }

            private readonly IEnumerable<T> source;
            private readonly T value;

            public Enumerator GetEnumerator() => new Enumerator(source, value);

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.GetEnumerator();

            public struct Enumerator : IEnumerator<T>
            {
                public Enumerator(IEnumerable<T> source, T value)
                {
                    Current = default!;
                    enumerator = source.GetEnumerator();
                    this.value = value;
                    state = 0;
                }

                private readonly IEnumerator<T> enumerator;
                private readonly T value;

                public T Current { get; private set; }

                object? IEnumerator.Current => this.Current;

                private int state;

                public bool MoveNext()
                {
                    switch (state)
                    {
                        case 0:
                            Current = value;
                            state = 1;
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
