using MelonLanguage;
using System;
using System.Diagnostics;

namespace MelonREPL {
    static class Program {
        static void Main(string[] args) {
            MelonEngine engine = new MelonEngine();

            var runs = 1e6;
            var code = "3 + 4 * 5 + 10";
            var instructions = engine.Parse(code);

            Console.WriteLine($"Benchmarking '{code}' @ {runs} times");
            Console.WriteLine("Benchmarking full parse...");
            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < 1e4; i++) {
                engine.Execute(code);
            }

            sw.Stop();

            Console.WriteLine($"Done in {sw.Elapsed.TotalMilliseconds}ms, {sw.Elapsed.TotalMilliseconds / runs}ms average per run");

            Console.WriteLine("Benchmarking only instructions...");
            sw = Stopwatch.StartNew();

            for (int i = 0; i < 1e4; i++) {
                engine.Execute(code);
            }

            sw.Stop();
            Console.WriteLine($"Done in {sw.Elapsed.TotalMilliseconds}ms, {sw.Elapsed.TotalMilliseconds / runs}ms average per run");

            while (true) {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(">");

                string line = Console.ReadLine();

                if (line == "exit") return;

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
