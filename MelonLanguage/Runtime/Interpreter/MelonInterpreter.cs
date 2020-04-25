using MelonLanguage.Compiling;
using MelonLanguage.Native;
using System;
using System.Collections.Generic;

namespace MelonLanguage.Runtime.Interpreter {
    public class MelonInterpreter {
        private readonly ExpressionSolver _expressionSolver;
        private readonly MelonEngine _engine;

        public MelonInterpreter(MelonEngine engine) {
            _engine = engine;

            _expressionSolver = new ExpressionSolver(engine);
        }

        public MelonObject Execute(Context context) {

            while (context.InstrCounter < context.Instructions.Length) {
                switch (context.Instruction) {
                    case (int)OpCode.LDBOOL:
                        LoadBoolean(context);
                        break;
                    case (int)OpCode.LDINT:
                        LoadInteger(context);
                        break;
                    case (int)OpCode.LDFLO:
                        LoadDecimal(context);
                        break;
                    case (int)OpCode.LDSTR:
                        LoadString(context);
                        break;
                    case (int)OpCode.ADD:
                        AddOperation(context);
                        break;
                    case (int)OpCode.MUL:
                        MulOperation(context);
                        break;
                    case (int)OpCode.LDLOC:
                        LDLOC(context);
                        break;
                    case (int)OpCode.STLOC:
                        STLOC(context);
                        break;
                    default:
                        throw new MelonException($"Unknown instruction '{context.Instruction:X4}'");
                }

                context.Next();
            }

            return context.Last();
        }
        private void LoadString(Context context) {
            context.Next();

            // Retrieve string from string table
            context.Push(new StringInstance(_engine.Strings[context.Instruction]));
        }

        private void LoadBoolean(Context context) {
            context.Next();

            context.Push(new BooleanInstance(context.Instruction == 1));
        }

        private void LoadInteger(Context context) {
            context.Next();

            context.Push(_engine.CreateInteger(context.Instruction));
        }

        private void LoadDecimal(Context context) {
            context.Next();
            var left = context.Instruction;
            context.Next();
            var right = context.Instruction;

            // Converts 2 int32s into a single double
            long longVal = (long)left << 32 | (uint)right;
            var decimalVal = BitConverter.Int64BitsToDouble(longVal);

            context.Push(new FloatInstance(decimalVal));
        }

        private void AddOperation(Context context) {
            var right = context.Pop();
            var left = context.Pop();

            context.Push(_expressionSolver.Add(left, right));
        }

        private void MulOperation(Context context) {
            var right = context.Pop();
            var left = context.Pop();

            context.Push(_expressionSolver.Mul(left, right));
        }

        private void STLOC(Context context) {
            context.Next();

            var value = context.Last();

            context.LocalValues[context.Instruction] = value;
        }

        private void LDLOC(Context context) {
            context.Next();

            context.Push(context.LocalValues[context.Instruction]);
        }
    }
}
