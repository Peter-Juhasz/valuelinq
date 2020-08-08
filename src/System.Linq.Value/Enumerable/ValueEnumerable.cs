using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static class ValueEnumerable
    {
        public static Value1Enumerable<T> Create<T>(T value) => new Value1Enumerable<T>(value);

        public static EmptyEnumerable<T> Empty<T>() => new EmptyEnumerable<T>();

        public readonly struct Value1Enumerable<T> : IEnumerable<T>
        {
            public Value1Enumerable(T value) => Value = value;

            public T Value { get; }

            public IEnumerator<T> GetEnumerator() => new Value1Enumerator(Value);

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public struct Value1Enumerator : IEnumerator<T>
            {
                public Value1Enumerator(T value)
                {
                    _value = value;
                    _started = false;
                    Current = default!;
                }

                private T _value;

                private bool _started;

                public T Current { get; private set; }

                object IEnumerator.Current => Current!;

                public bool MoveNext()
                {
                    if (!_started)
                    {
                        _started = true;
                        Current = _value;
                        return true;
                    }

                    return false;
                }

                public void Reset()
                {
                    _started = false;
                    Current = default!;
                }

                public void Dispose() { }
            }
        }

        public readonly struct EmptyEnumerable<T> : IEnumerable<T>
        {
            public IEnumerator<T> GetEnumerator() => new EmptyEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public struct EmptyEnumerator : IEnumerator<T>
            {
                public T Current => throw new InvalidOperationException();

                object IEnumerator.Current => Current!;

                public bool MoveNext() => false;

                public void Reset() { }

                public void Dispose() { }
            }
        }
    }
}
