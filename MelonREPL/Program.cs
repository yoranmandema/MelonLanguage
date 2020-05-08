using MelonLanguage;
using MelonLanguage.Compiling;
using MelonLanguage.Native;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MelonREPL {
    public static class Program {

        private static MelonObject print(MelonObject self, Arguments arguments) {
            Console.WriteLine(string.Join(",", (object[])arguments.Values));

            return null;
        }

        public static void Main(string[] args) {
            MelonEngine engine = new MelonEngine();

            engine.FastAdd("print", new NativeFunctionInstance("print", engine, print));

            const int runs = 10000;
            string _file = "";

            if (args.Any()) {
                _file = Path.Combine(Directory.GetCurrentDirectory(), args[0]);
            }

            if (!string.IsNullOrEmpty(_file)) {

                string code = System.IO.File.ReadAllText(_file);
                var parseContext = engine.Parse(code);

                var context = engine.CreateContext(parseContext);
                var printer = new ByteCodePrinter(engine);
                printer.Print(parseContext);

                engine.Execute(context);

                Console.WriteLine();

                Console.WriteLine($"Completion value: {engine.CompletionValue}");
                Console.WriteLine($"Benchmarking {runs} times");
                Stopwatch sw = Stopwatch.StartNew();

                for (int i = 0; i < runs; i++) {
                    engine.Execute(context);
                }

                sw.Stop();
                Console.WriteLine($"Done in {sw.Elapsed.TotalMilliseconds}ms, {sw.Elapsed.TotalMilliseconds / runs}ms average per run");
                Console.WriteLine($"{1d / (sw.Elapsed.TotalSeconds / runs)} ops/s");
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
