using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static class ValueEnumerable
    {
        public static Value1Enumerable<T> Create<T>(T value) => new Value1Enumerable<T>(value);

        public static Value2Enumerable<T> Create<T>(T value1, T value2) => new Value2Enumerable<T>(value1, value2);

        public static EmptyEnumerable<T> Empty<T>() => new EmptyEnumerable<T>();

        public readonly struct Value1Enumerable<T> : IEnumerable<T>, IReadOnlyCollection<T>, IReadOnlyList<T>
        {
            public Value1Enumerable(T value) => Value = value;

            public T Value { get; }

            public int Count => 1;

            public T this[int index] => index switch
            {
                0 => Value,
                _ => throw new ArgumentOutOfRangeException(nameof(index))
            };

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

        public readonly struct Value2Enumerable<T> : IEnumerable<T>, IReadOnlyCollection<T>, IReadOnlyList<T>
        {
            public Value2Enumerable(T value1, T value2)
            {
                Value1 = value1;
                Value2 = value2;
            }

            public T Value1 { get; }
            public T Value2 { get; }

            public int Count => 2;

            public T this[int index] => index switch
            {
                0 => Value1,
                1 => Value2,
                _ => throw new ArgumentOutOfRangeException(nameof(index))
            };

            public IEnumerator<T> GetEnumerator() => new Value2Enumerator(Value1, Value2);

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public struct Value2Enumerator : IEnumerator<T>
            {
                public Value2Enumerator(T value1, T value2)
                {
                    _value1 = value1;
                    _value2 = value2;
                    _index = 0;
                    Current = default!;
                }

                private T _value1;
                private T _value2;

                private byte _index;

                public T Current { get; private set; }

                object IEnumerator.Current => Current!;

                public bool MoveNext()
                {
                    switch (_index)
                    {
                        case 0:
                            _index = 1;
                            Current = _value1;
                            return true;

                        case 1:
                            _index = 2;
                            Current = _value2;
                            return true;
                    }

                    return false;
                }

                public void Reset()
                {
                    _index = 0;
                    Current = default!;
                }

                public void Dispose() { }
            }
        }

        public readonly struct EmptyEnumerable<T> : IEnumerable<T>, IReadOnlyCollection<T>, IReadOnlyList<T>
        {
            public IEnumerator<T> GetEnumerator() => new EmptyEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public int Count => 0;

            public T this[int index] => throw new ArgumentOutOfRangeException(nameof(index));

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
