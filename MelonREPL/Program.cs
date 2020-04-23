using MelonLanguage;
using System;
using System.Diagnostics;

namespace MelonREPL {
    public static class Program {
        public static void Main(string[] args) {
            MelonEngine engine = new MelonEngine();

            const int runs = (int)1e6;
            const string code = "((3 + 4.5) * 5) + 10";
            var instructions = engine.Parse(code);

            Console.WriteLine(string.Join(",", instructions));
            Console.WriteLine($"Result {engine.Execute(instructions).CompletionValue}");
            Console.WriteLine($"Benchmarking '{code}' @ {runs} times");
            Console.WriteLine("Benchmarking only instructions...");
            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < runs; i++) {
                engine.Execute(instructions);
            }

            sw.Stop();
            Console.WriteLine($"Done in {sw.Elapsed.TotalMilliseconds}ms, {sw.Elapsed.TotalMilliseconds / runs}ms average per run");
            Console.WriteLine($"{1d / (sw.Elapsed.TotalSeconds / runs)} ops/s");

            while (true) {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(">");

                string line = Console.ReadLine();

                if (line == "exit") {
                    return;
                }

                try {
                    engine.Execute(line);

                    Console.ForegroundColor = ConsoleColor.Magenta;

                    Console.WriteLine(engine.CompletionValue?.ToString() ?? "null");
                }
                catch (Exception e) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e);
                }
            }
        }
    }
}
