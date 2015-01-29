using System;
using System.Numerics;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace Fibs
{
    using Console;

    class Program
    {

        static void Main(string[] args)
        {
            PrintFibonacciNumbers(1000).Wait();
        }

        private static Task PrintFibonacciNumbers(int quantity)
        {
            var fibGen = Observable.Generate(
                initialState:   new { OldN = (BigInteger) 0, N = (BigInteger) 1, Count = 1 },
                condition:      s => s.Count <= quantity,
                iterate:        s => new { OldN = s.N, N = s.OldN + s.N, Count = s.Count + 1 },
                resultSelector: s => s.N.ToString());

            fibGen.Subscribe(WriteLine);

            return fibGen.ToTask();
        }
    }
}
