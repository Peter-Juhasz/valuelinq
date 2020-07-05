using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableExtensions
    {
        public static AppendEnumerable<T> ValueAppend<T>(this IEnumerable<T> source, T value) =>
            new AppendEnumerable<T>(source, value);

        public readonly struct AppendEnumerable<T> : IEnumerable<T>
        {
            public AppendEnumerable(IEnumerable<T> source, T value)
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
                            if (enumerator.MoveNext())
                            {
                                Current = enumerator.Current;
                                return true;
                            }
                            else
                            {
                                state = 1;
                                Current = value;
                                return true;
                            }
                            break;

                        case 1:
                            return false;
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
