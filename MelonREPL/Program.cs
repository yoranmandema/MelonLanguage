using MelonLanguage;
using MelonLanguage.Compiling;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MelonREPL {
    public static class Program {
        public static void Main(string[] args) {
            MelonEngine engine = new MelonEngine();

            const int runs = 1000000;
            string _file = "";

            if (args.Any()) {
                _file = Path.Combine(Directory.GetCurrentDirectory(), args[0]);
            }

            if (!string.IsNullOrEmpty(_file)) {

                string code = System.IO.File.ReadAllText(_file);
                var parseContext = engine.Parse(code);

                var context = engine.CreateContext(parseContext);
                var printer = new ByteCodePrinter(engine, parseContext);
                printer.Print();

                Console.WriteLine();
                Console.WriteLine($"Benchmarking {runs} times");
                Stopwatch sw = Stopwatch.StartNew();

                for (int i = 0; i < runs; i++) {
                    engine.Execute(context);
                }

                sw.Stop();
                Console.WriteLine($"Done in {sw.Elapsed.TotalMilliseconds}ms, {sw.Elapsed.TotalMilliseconds / runs}ms average per run");
                Console.WriteLine($"{1d / (sw.Elapsed.TotalSeconds / runs)} ops/s");
                Console.WriteLine($"Completion value: {engine.CompletionValue}");
            }

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
