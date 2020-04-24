using MelonLanguage.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace MelonLanguage.Compiling {
    public class ByteCodePrinter {
        private readonly int[] _instructions;
        private readonly MelonEngine _engine;

        public ByteCodePrinter (MelonEngine engine, int[] instructions) {
            _engine = engine;

            _instructions = instructions;
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

            for (int i = 0; i < _instructions.Length; i++) {
                Console.WriteLine(_instructions[i].ToString("X8"));
            }
        }

        public void Print () {
            Context context = new Context(_instructions);
            int instrNum = 0;

            while (context.InstrCounter < _instructions.Length) {
                Console.Write($"MLN_{instrNum:x4}: ");

                switch (context.Instruction) {
                    case (int)OpCode.LDBOOL:
                        context.Next();

                        Console.Write("LDBOOL ");
                        Console.Write(context.Instruction == 1);
                        Console.WriteLine();

                        break;
                    case (int)OpCode.LDINT:
                        context.Next();

                        Console.Write("LDINT ");
                        Console.Write(context.Instruction);
                        Console.WriteLine();

                        break;
                    case (int)OpCode.LDDEC:
                        context.Next();

                        int left = context.Instruction;

                        context.Next();

                        int right = context.Instruction;

                        Console.Write("LDDEC ");
                        Console.Write(GetDecimalValue(left,right).ToString("0.0##############################"));
                        Console.WriteLine();
                        break;
                    case (int)OpCode.LDSTR:
                        context.Next();

                        Console.Write("LDSTR ");
                        Console.Write(_engine.Strings[context.Instruction]);
                        Console.WriteLine();
                        break;
                    case (int)OpCode.ADD:
                        Console.WriteLine("ADD");
                        break;
                    case (int)OpCode.MUL:
                        Console.WriteLine("MUL");
                        break;
                }

                context.Next();
                instrNum++;
            }
        }
    }
}
