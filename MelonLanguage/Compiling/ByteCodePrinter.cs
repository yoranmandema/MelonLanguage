using MelonLanguage.Native;
using System;

namespace MelonLanguage.Compiling {
    public class ByteCodePrinter {
        private readonly MelonEngine _engine;
        public ByteCodePrinter(MelonEngine engine) {
            _engine = engine;
        }

        private double GetDecimalValue(int left, int right) {
            long longVal = (long)left << 32 | (uint)right;
            var decimalVal = BitConverter.Int64BitsToDouble(longVal);

            return decimalVal;
        }

        //public void PrintHex() {
        //    //byte[] result = new byte[_instructions.Length * sizeof(int)];
        //    //Buffer.BlockCopy(_instructions, 0, result, 0, result.Length);

        //    //Console.WriteLine(BitConverter.ToString(result).Replace("-", ""));

        //    for (int i = 0; i < _context.Instructions.Length; i++) {
        //        Console.WriteLine(_context.Instructions[i].ToString("X8"));
        //    }
        //}

        public void Print(ParseContext parseContext, MelonObject parent = null) {
            var context = _engine.CreateContext(parseContext);

            foreach (var kv in parseContext.Variables) {
                if (kv.Value.Variable.value is ScriptFunctionInstance scriptFunctionInstance && scriptFunctionInstance != parent) {
                    //var functionNameBuilder = new StringBuilder("fn ");
                    //if (scriptFunctionInstance.ReturnType != null) {
                    //    functionNameBuilder.Append(scriptFunctionInstance.ReturnType).Append(' ');
                    //}

                    //functionNameBuilder.Append(scriptFunctionInstance.Name).Append(' ');
                    //functionNameBuilder.Append('(').Append(scriptFunctionInstance.ParameterTypes != null ? string.Join(",", (object[])scriptFunctionInstance.ParameterTypes.Select(x => x.Name)) : "").Append(')');

                    //Console.WriteLine(functionNameBuilder.ToString());
                    Console.WriteLine(scriptFunctionInstance.ToString());
                    Print(scriptFunctionInstance.ParseContext, kv.Value.Variable.value);
                }
            }

            Console.WriteLine("Variables {");

            foreach (var kv in parseContext.Variables) {
                //var type = _engine.Types[context.LocalTypes[i]];

                var genericStr = "";

                if (kv.Value.Variable.type.GenericTypes?.Length > 0) {
                    genericStr = $"<{string.Join(",", (object[])kv.Value.Variable.type.GenericTypes)}>";
                }

                Console.WriteLine($"\t{kv.Key}: ({kv.Value.Type}) {kv.Value.Variable.name}: {(kv.Value.Variable.type.Type == _engine.functionType ? kv.Value.Variable.value.ToString() : kv.Value.Variable.type.ToString())}");
            }

            Console.WriteLine("}\n");

            int line = 0;

            for (int instrNum = 0; context.InstrCounter < context.Instructions.Length; instrNum++) {
                Console.Write($"MLN_{instrNum:x4}: ");

                string instructionString = ((OpCode)context.Instruction).ToString();
                Console.Write(instructionString + " ");

                switch (context.Instruction) {
                    case (int)OpCode.LDBOOL:
                        context.Next();

                        Console.Write(context.Instruction == 1);

                        break;
                    case (int)OpCode.LDINT:
                        context.Next();

                        Console.Write(context.Instruction);

                        break;
                    case (int)OpCode.LDFLO:
                        context.Next();

                        int left = context.Instruction;

                        context.Next();

                        int right = context.Instruction;

                        Console.Write(GetDecimalValue(left, right).ToString("0.0##############################"));
                        break;
                    case (int)OpCode.LDSTR:
                        context.Next();

                        Console.Write(_engine.Strings[context.Instruction]);
                        break;
                    case (int)OpCode.STLOC:
                        context.Next();

                        Console.Write(context.Instruction);
                        break;
                    case (int)OpCode.LDLOC:
                        context.Next();

                        Console.Write(context.Instruction);
                        break;
                    case (int)OpCode.STELEM:
                        break;
                    case (int)OpCode.LDELEM:
                        break;
                    case (int)OpCode.LDTYP:
                        context.Next();

                        Console.Write(context.Instruction);
                        break;
                    case (int)OpCode.LDPRP:
                        context.Next();

                        Console.Write(_engine.Strings[context.Instruction]);
                        break;
                    case (int)OpCode.BR:
                    case (int)OpCode.BRTRUE:
                        context.Next();

                        Console.Write($"MLN_{parseContext.BranchLines[context.Instruction]:x4}");
                        break;
                    case (int)OpCode.DUP:
                        break;
                }

                Console.WriteLine();

                context.Next();

                line++;
            }

            Console.WriteLine();

            context.Reset();
        }
    }
}
