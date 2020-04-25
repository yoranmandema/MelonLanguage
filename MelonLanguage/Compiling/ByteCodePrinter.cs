using MelonLanguage.Runtime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MelonLanguage.Compiling {
    public class ByteCodePrinter {

        private readonly MelonEngine _engine;
        private readonly Context _context;

        public ByteCodePrinter (MelonEngine engine, Context context) {
            _engine = engine;
            _context = context;
        }

        private double GetDecimalValue (int left, int right) {
            long longVal = (long)left << 32 | (uint)right;
            var decimalVal = BitConverter.Int64BitsToDouble(longVal);

            return decimalVal;
        }

        public void PrintHex () {
            //byte[] result = new byte[_instructions.Length * sizeof(int)];
            //Buffer.BlockCopy(_instructions, 0, result, 0, result.Length);

            //Console.WriteLine(BitConverter.ToString(result).Replace("-", ""));

            for (int i = 0; i < _context.Instructions.Length; i++) {
                Console.WriteLine(_context.Instructions[i].ToString("X8"));
            }
        }

        public void Print () {
            Console.WriteLine($"Instructions: {_context.Instructions.Length}");

            Console.WriteLine("Locals {");

            for (int i = 0; i < _context.LocalNames.Length; i++) {
                var type = _engine.Types[_context.LocalTypes[i]];

                Console.WriteLine($"\t{i}: {type.Name}");
            }

            Console.WriteLine("}\n");

            for (int instrNum = 0; _context.InstrCounter < _context.Instructions.Length; instrNum++) {
                Console.Write($"MLN_{instrNum:x4}: ");

                switch (_context.Instruction) {
                    case (int)OpCode.LDBOOL:
                        _context.Next();

                        Console.Write("LDBOOL ");
                        Console.Write(_context.Instruction == 1);
                        Console.WriteLine();

                        break;
                    case (int)OpCode.LDINT:
                        _context.Next();

                        Console.Write("LDINT ");
                        Console.Write(_context.Instruction);
                        Console.WriteLine();

                        break;
                    case (int)OpCode.LDFLO:
                        _context.Next();

                        int left = _context.Instruction;

                        _context.Next();

                        int right = _context.Instruction;

                        Console.Write("LDDEC ");
                        Console.Write(GetDecimalValue(left, right).ToString("0.0##############################"));
                        Console.WriteLine();
                        break;
                    case (int)OpCode.LDSTR:
                        _context.Next();

                        Console.Write("LDSTR ");
                        Console.Write(_engine.Strings[_context.Instruction]);
                        Console.WriteLine();
                        break;
                    case (int)OpCode.STLOC:
                        _context.Next();

                        Console.Write("STLOC ");
                        Console.Write(_context.Instruction);
                        Console.WriteLine();
                        break;
                    case (int)OpCode.LDLOC:
                        _context.Next();

                        Console.Write("LDLOC ");
                        Console.Write(_context.Instruction);
                        Console.WriteLine();
                        break;
                    case (int)OpCode.ADD:
                        Console.WriteLine("ADD");
                        break;
                    case (int)OpCode.MUL:
                        Console.WriteLine("MUL");
                        break;
                }

                _context.Next();
            }

            _context.Reset();
        }
    }
}
