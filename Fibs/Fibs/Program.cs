using System;
using System.Numerics;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace Fibs
{
    using Console;

    class Program
    {

        static void Main(string[] args)
        {
            var fibGen = Observable.Generate(
                initialState:   new Tuple<BigInteger, BigInteger>(0, 1),
                condition:      _   => true,
                iterate:        tup => new Tuple<BigInteger, BigInteger>(tup.Item2, tup.Item1 + tup.Item2),
                resultSelector: tup => tup.Item2);

            var tusenFibs = fibGen.Take(1000).Select(x => x.ToString());

            tusenFibs.Subscribe(WriteLine);

            tusenFibs.ToTask().Wait();
        }
    }
}
