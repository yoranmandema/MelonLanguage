using MelonLanguage.Runtime;
using System;

namespace MelonLanguage.Compiling {
    public class ByteCodePrinter {

        private readonly MelonEngine _engine;
        private readonly Context _context;
        private readonly ParseContext _parseContext;

        public ByteCodePrinter(MelonEngine engine, ParseContext parseContext) {
            _engine = engine;
            _parseContext = parseContext;
            _context = engine.CreateContext(parseContext);
        }

        private double GetDecimalValue(int left, int right) {
            long longVal = (long)left << 32 | (uint)right;
            var decimalVal = BitConverter.Int64BitsToDouble(longVal);

            return decimalVal;
        }

        public void PrintHex() {
            //byte[] result = new byte[_instructions.Length * sizeof(int)];
            //Buffer.BlockCopy(_instructions, 0, result, 0, result.Length);

            //Console.WriteLine(BitConverter.ToString(result).Replace("-", ""));

            for (int i = 0; i < _context.Instructions.Length; i++) {
                Console.WriteLine(_context.Instructions[i].ToString("X8"));
            }
        }

        public void Print() {

            Console.WriteLine("Locals {");

            for (int i = 0; i < _context.LocalNames.Length; i++) {
                var type = _engine.Types[_context.LocalTypes[i]];

                Console.WriteLine($"\t{i}: {type.Name}");
            }

            Console.WriteLine("}\n");

            int line = 0;

            for (int instrNum = 0; _context.InstrCounter < _context.Instructions.Length; instrNum++) {
                Console.Write($"MLN_{instrNum:x4}: ");

                string instructionString = ((OpCode)_context.Instruction).ToString();
                Console.Write(instructionString + " ");

                switch (_context.Instruction) {
                    case (int)OpCode.LDBOOL:
                        _context.Next();

                        Console.Write(_context.Instruction == 1);

                        break;
                    case (int)OpCode.LDINT:
                        _context.Next();

                        Console.Write(_context.Instruction);

                        break;
                    case (int)OpCode.LDFLO:
                        _context.Next();

                        int left = _context.Instruction;

                        _context.Next();

                        int right = _context.Instruction;

                        Console.Write(GetDecimalValue(left, right).ToString("0.0##############################"));
                        break;
                    case (int)OpCode.LDSTR:
                        _context.Next();

                        Console.Write(_engine.Strings[_context.Instruction]);
                        break;
                    case (int)OpCode.STLOC:
                        _context.Next();

                        Console.Write(_context.Instruction);
                        break;
                    case (int)OpCode.LDLOC:
                        _context.Next();

                        Console.Write(_context.Instruction);
                        break;
                    case (int)OpCode.LDTYP:
                        _context.Next();

                        Console.Write(_context.Instruction);
                        break;
                    case (int)OpCode.GTMEM:
                        _context.Next();

                        Console.Write(_engine.Strings[_context.Instruction]);
                        break;
                    case (int)OpCode.BR:
                    case (int)OpCode.BRTRUE:
                        _context.Next();

                        Console.Write($"MLN_{_parseContext.branchLines[_context.Instruction]:x4}");
                        break;
                }

                Console.WriteLine();

                _context.Next();

                line++;
            }

            Console.WriteLine();

            Console.WriteLine($"Instructions: {line}");

            _context.Reset();
        }
    }
}
