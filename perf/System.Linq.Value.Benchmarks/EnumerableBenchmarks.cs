using BenchmarkDotNet.Attributes;

namespace System.Linq.Value.Benchmarks
{
    [MemoryDiagnoser]
    public class EnumerableBenchmarks
    {
        private readonly int[] numbers = new int[] { 3, 5, 2, 5, 1, 6, 7, 43, 0, 4, 213, 0, 4, 635, 4, 3 };

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
    }
}
