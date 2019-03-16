using System;

using BenchmarkDotNet.Attributes;

namespace Chaining_decimal
{

    // See the 01 and 03 'Chaining' examples.

    public static class Chaining_01_Direct
    {
        public static decimal FahrenheitToCelsius(decimal temperatureInFahrenheit)
        {
            var returnValue = temperatureInFahrenheit - 32;
            returnValue *= 5;
            returnValue /= 9;
            returnValue = Math.Round(returnValue, 2);
            return returnValue;
        }    
    }

    public class Chaining_03_Functional
    {
        public static Func<decimal, Func<decimal, decimal>> _add = x => y => x + y;
        public static Func<decimal, Func<decimal, decimal>> _subtract = x => y => y - x;
        public static Func<decimal, Func<decimal, decimal>> _multiply = x => y => x * y;
        public static Func<decimal, Func<decimal, decimal>> _divide = x => y => y / x;


        public static decimal FahrenheitToCelsius(decimal input) =>
            input.ToIdentity().Bind(_subtract(32))
                .Bind(_multiply(5))
                .Bind(_divide(9))
                .Bind(x => Math.Round(x, 2));

    }

    public class Identity<T>
    {
        public T Value { get; }

        public Identity(T value)
        {
            Value = value;
        }

        public static implicit operator Identity<T> (T @this) => @this.ToIdentity();
        public static implicit operator T(Identity<T> @this) => @this.Value;
    }

    public static class FunctionalExtensions2
    {
        public static Identity<T> ToIdentity<T>(this T @this) => new Identity<T>(@this);

        public static Identity<TToType> Bind<TFromType, TToType>(this Identity<TFromType> @this,
            Func<TFromType, TToType> f) =>
            f(@this.Value).ToIdentity();
    }

    [RPlotExporter, RankColumn]
    public class Benchmark
    {

        [Params(10, 1000)]
        public int N ;

        [Benchmark(Baseline=true)]
        public decimal Direct ( ) 
        {
          decimal total = 0 ;
          for ( int i = 0 ; i < N ; i++ )
          {
            total += Chaining_01_Direct.FahrenheitToCelsius(i) ;
          }
          return total ;
        }

        [Benchmark]
        public decimal Functional ( ) 
        {
          decimal total = 0 ;
          for ( int i = 0 ; i < N ; i++ )
          {
            total += Chaining_03_Functional.FahrenheitToCelsius(i) ;
          }
          return total ;
        }
    }

}
