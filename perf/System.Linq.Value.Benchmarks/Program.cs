using BenchmarkDotNet.Running;
using System;

namespace System.Linq.Value.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ReadOnlyListBenchmarks>();
        }
    }
}
