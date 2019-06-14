using System ;
using BenchmarkDotNet.Attributes ;
using BenchmarkDotNet.Running ;

namespace SimpleBenchmark
{

    public class Program
    {

        public static void Main ( string[] args )
        {
            var summary_decimal = BenchmarkRunner.Run<Chaining_decimal.Benchmark>();
            var summary_double = BenchmarkRunner.Run<Chaining_double.Benchmark>();
        }

    }

}