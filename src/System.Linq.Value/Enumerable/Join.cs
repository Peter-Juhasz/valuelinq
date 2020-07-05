using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableExtensions
    {
        public static JoinEnumerable<TLeft, TRight, TKey, TResult> ValueJoin<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> source,
            IEnumerable<TRight> second,
            Func<TLeft, TKey> leftKeySelector,
            Func<TRight, TKey> rightKeySelector,
            Func<TLeft, TRight, TResult> resultSelector,
            IEqualityComparer<TKey> keyComparer
        ) =>
            new JoinEnumerable<TLeft, TRight, TKey, TResult>(source, second, leftKeySelector, rightKeySelector, resultSelector, keyComparer);

        public static JoinEnumerable<TLeft, TRight, TKey, TResult> ValueJoin<TLeft, TRight, TKey, TResult>(
           this IEnumerable<TLeft> source,
           IEnumerable<TRight> second,
           Func<TLeft, TKey> leftKeySelector,
           Func<TRight, TKey> rightKeySelector,
           Func<TLeft, TRight, TResult> resultSelector
       ) =>
            source.ValueJoin(second, leftKeySelector, rightKeySelector, resultSelector, EqualityComparer<TKey>.Default);


        public readonly struct JoinEnumerable<TLeft, TRight, TKey, TResult> : IEnumerable<TResult>
        {
            public JoinEnumerable(
                IEnumerable<TLeft> source,
                IEnumerable<TRight> second,
                Func<TLeft, TKey> leftKeySelector,
                Func<TRight, TKey> rightKeySelector,
                Func<TLeft, TRight, TResult> resultSelector,
                IEqualityComparer<TKey> keyComparer
            )
            {
                this.source = source;
                this.second = second;
                this.leftKeySelector = leftKeySelector;
                this.rightKeySelector = rightKeySelector;
                this.resultSelector = resultSelector;
                this.keyComparer = keyComparer;
            }

            private readonly IEnumerable<TLeft> source;
            private readonly IEnumerable<TRight> second;
            private readonly Func<TLeft, TKey> leftKeySelector;
            private readonly Func<TRight, TKey> rightKeySelector;
            private readonly Func<TLeft, TRight, TResult> resultSelector;
            private readonly IEqualityComparer<TKey> keyComparer;

            public Enumerator GetEnumerator() => new Enumerator(source, second, leftKeySelector, rightKeySelector, resultSelector, keyComparer);

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => this.GetEnumerator();

            public struct Enumerator : IEnumerator<TResult>
            {
                public Enumerator(
                    IEnumerable<TLeft> source,
                    IEnumerable<TRight> second,
                    Func<TLeft, TKey> leftKeySelector,
                    Func<TRight, TKey> rightKeySelector,
                    Func<TLeft, TRight, TResult> resultSelector,
                    IEqualityComparer<TKey> keyComparer
                )
                {
                    Current = default!;
                    this.enumerator = source.GetEnumerator();
                    this.second = second;
                    this.leftKeySelector = leftKeySelector;
                    this.rightKeySelector = rightKeySelector;
                    this.resultSelector = resultSelector;
                    this.keyComparer = keyComparer;
                }

                private readonly IEnumerator<TLeft> enumerator;
                private readonly IEnumerable<TRight> second;
                private readonly Func<TLeft, TKey> leftKeySelector;
                private readonly Func<TRight, TKey> rightKeySelector;
                private readonly Func<TLeft, TRight, TResult> resultSelector;
                private readonly IEqualityComparer<TKey> keyComparer;

                public TResult Current { get; private set; }

                object? IEnumerator.Current => this.Current;


                public bool MoveNext()
                {
                    var keyComparer = this.keyComparer;
                    var rightKeySelector = this.rightKeySelector;

                    if (enumerator.MoveNext())
                    {
                        var current = enumerator.Current;
                        var key = leftKeySelector(current);
                        var other = second.SingleOrDefault(s => keyComparer.Equals(key, rightKeySelector(s)));
                        Current = resultSelector(current, other);
                        return true;
                    }

                    return false;
                }

                public void Reset()
                {
                    enumerator.Reset();
                }

                public void Dispose()
                {
                    enumerator.Dispose();
                }
            }
        }
    }
}
