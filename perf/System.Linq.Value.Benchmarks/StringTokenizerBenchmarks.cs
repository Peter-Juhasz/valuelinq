using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Primitives;

namespace System.Linq.Value.Benchmarks
{
    [MemoryDiagnoser]
    public class StringTokenizerBenchmarks
    {
        private readonly string str = "A,B,234,C,43,D,23,1,fggs,$GGPS,a,,dfd";
        private readonly char[] sep = new char[] { ',' };

        [Benchmark]
        public void Select()
        {
            var tokenizer = new StringTokenizer(str, sep);
            int sum = 0;

            foreach (var i in tokenizer.Select(n => n.Length))
            {
                sum += i;
            }
        }

        [Benchmark]
        public void ValueSelect()
        {
            var tokenizer = new StringTokenizer(str, sep);
            int sum = 0;

            foreach (var i in tokenizer.ValueSelect(n => n.Length))
            {
                sum += i;
            }
        }

        [Benchmark]
        public void SelectIterative()
        {
            var tokenizer = new StringTokenizer(str, sep);
            int sum = 0;

            foreach (var n in tokenizer)
            {
                int i = n.Length;
                sum += i;
            }
        }

        [Benchmark]
        public void Where()
        {
            var tokenizer = new StringTokenizer(str, sep);
            int sum = 0;

            foreach (var i in tokenizer.Where(n => n.Length > 1))
            {
                sum += i.Length;
            }
        }

        [Benchmark]
        public void ValueWhere()
        {
            var tokenizer = new StringTokenizer(str, sep);
            int sum = 0;

            foreach (var i in tokenizer.ValueWhere(n => n.Length > 1))
            {
                sum += i.Length;
            }
        }

        [Benchmark]
        public void WhereIterative()
        {
            var tokenizer = new StringTokenizer(str, sep);
            int sum = 0;

            foreach (var i in tokenizer)
            {
                if (i.Length > 1)
                {
                    sum += i.Length;
                }
            }
        }
    }
}
