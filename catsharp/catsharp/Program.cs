using System;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace catsharp
{
    using Console;
    using Observable;

    static class Program
    {
        const string StdInFlag = "-";

        /* The state is dead, long live the state. */
        static void Main(string[] args)
        {
            Using(OpenStandardOutput, stdOut => 
                args.ToObservable()
                    /* We want to mirror STDIN if no flags are given */
                    .DefaultIfEmpty(StdInFlag)
                    /* Monad join + Using/Return gives us resource management for free @_@ */
                    .SelectMany(f => f.Equals(StdInFlag)
                            ? /* Use STDIN if the alias is used */
                            Using(OpenStandardInput, Return)
                            : /* Otherwise, the standard FileStream specified */
                            Using(() => File.OpenRead(f), Return)
                                /* Sometimes, files don't exist on disk - chin up :-( */
                                .Catch<Stream, IOException>(ex =>
                                    Error.WriteLineAsync("cat#: " + ex.Message).ContinueWith(_=>Empty<Stream>()).Result))
                    .Do(onNext: s => s.CopyTo(stdOut))
             ).Wait();
        }
    }
}
