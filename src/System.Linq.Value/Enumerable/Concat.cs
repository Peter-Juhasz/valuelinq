using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableExtensions
    {
        public static ConcatEnumerable<T> ValueConcat<T>(this IEnumerable<T> source, IEnumerable<T> second) =>
            new ConcatEnumerable<T>(source, second);


        public readonly struct ConcatEnumerable<T> : IEnumerable<T>
        {
            public ConcatEnumerable(IEnumerable<T> source, IEnumerable<T> second)
            {
                this.source = source;
                this.second = second;
            }

            private readonly IEnumerable<T> source;
            private readonly IEnumerable<T> second;

            public Enumerator GetEnumerator() => new Enumerator(source, second);

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.GetEnumerator();

            public struct Enumerator : IEnumerator<T>
            {
                public Enumerator(IEnumerable<T> source, IEnumerable<T> second)
                {
                    Current = default!;
                    enumerator = source.GetEnumerator();
                    this.second = second.GetEnumerator();
                    _state = 0;
                }

                private readonly IEnumerator<T> enumerator;
                private readonly IEnumerator<T> second;

                public T Current { get; private set; }

                object? IEnumerator.Current => this.Current;

                private int _state;

                public bool MoveNext()
                {
                    switch (_state)
                    {
                        case 0:
                            if (enumerator.MoveNext())
                            {
                                Current = enumerator.Current;
                                return true;
                            }
                            else
                            {
                                _state = 1;
                                goto case 1;
                            }
                            break;

                        case 1:
                            if (second.MoveNext())
                            {
                                Current = second.Current;
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                            break;
                    }

                    return false;
                }

                public void Reset()
                {
                    enumerator.Reset();
                    second.Reset();
                    _state = 0;
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
