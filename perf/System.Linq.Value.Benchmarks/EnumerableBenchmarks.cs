using BenchmarkDotNet.Attributes;

namespace System.Linq.Value.Benchmarks
{
    [MemoryDiagnoser]
    public class EnumerableBenchmarks
    {
        private readonly int[] numbers;

        public EnumerableBenchmarks()
        {
            Random random = new Random(0);
            numbers = Enumerable.Range(0, 16).Select(d => random.Next(0, 500)).ToArray();
        }

        [Benchmark]
        public void Select()
        {
            int sum = 0;

            foreach (var i in numbers.Select(n => n * 2))
            {
                sum += i;
            }
        }

        [Benchmark]
        public void ValueSelect()
        {
            int sum = 0;

            foreach (var i in numbers.ValueSelect(n => n * 2))
            {
                sum += i;
            }
        }

        [Benchmark]
        public void SelectIterative()
        {
            int sum = 0;

            foreach (var n in numbers)
            {
                int i = n * 2;
                sum += i;
            }
        }


        [Benchmark]
        public void Where()
        {
            int sum = 0;

            foreach (var i in numbers.Where(n => n % 2 == 0))
            {
                sum += i;
            }
        }

        [Benchmark]
        public void ValueWhere()
        {
            int sum = 0;

            foreach (var i in numbers.ValueWhere(n => n % 2 == 0))
            {
                sum += i;
            }
        }

        [Benchmark]
        public void WhereIterative()
        {
            int sum = 0;

            foreach (var i in numbers)
            {
                if (i % 2 == 0)
                {
                    sum += i;
                }
            }
        }

    }
}
