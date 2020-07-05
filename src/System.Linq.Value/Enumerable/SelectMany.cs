using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableExtensions
    {
        public static SelectManyEnumerable<T, TResult> ValueSelectMany<T, TResult>(this IEnumerable<T> source, Func<T, IEnumerable<TResult>> selector) =>
            new SelectManyEnumerable<T, TResult>(source, selector);


        public readonly struct SelectManyEnumerable<T, TResult> : IEnumerable<TResult>
        {
            public SelectManyEnumerable(IEnumerable<T> source, Func<T, IEnumerable<TResult>> selector)
            {
                this.source = source;
                this.selector = selector;
            }

            private readonly IEnumerable<T> source;
            private readonly Func<T, IEnumerable<TResult>> selector;

            public Enumerator GetEnumerator() => new Enumerator(source, selector);

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => this.GetEnumerator();

            public struct Enumerator : IEnumerator<TResult>
            {
                public Enumerator(IEnumerable<T> source, Func<T, IEnumerable<TResult>> selector)
                {
                    Current = default!;
                    enumerator = source.GetEnumerator();
                    this.selector = selector;
                    _state = 0;
                    _second = null;
                }

                private readonly IEnumerator<T> enumerator;
                private readonly Func<T, IEnumerable<TResult>> selector;

                public TResult Current { get; private set; }

                object? IEnumerator.Current => this.Current;

                private int _state;
                private IEnumerator<TResult>? _second;

                public bool MoveNext()
                {
                    switch (_state)
                    {
                        case 0:
                            if (enumerator.MoveNext())
                            {
                                _second = selector(enumerator.Current).GetEnumerator();
                                _state = 1;
                                goto case 1;
                            }
                            else
                            {
                                return false;
                            }
                            break;

                        case 1:
                            if (_second!.MoveNext())
                            {
                                Current = _second.Current;
                                return true;
                            }
                            else
                            {
                                using (_second) ;
                                _second = null;
                                goto case 0;
                            }
                            break;
                    }

                    return false;
                }

                public void Reset()
                {
                    enumerator.Reset();
                    _state = 0;
                }

                public void Dispose()
                {
                    enumerator.Dispose();

                    if (_second != null)
                    {
                        _second.Dispose();
                    }
                }
            }
        }
    }
}
