using MelonLanguage.Compiling;
using MelonLanguage.Native;
using System;
using System.Collections.Generic;

namespace MelonLanguage.Runtime.Interpreter {
    public class MelonInterpreter {
        private readonly ExpressionSolver _expressionSolver;
        private readonly MelonEngine _engine;

        public MelonObject CompletionValue { get; private set; }

        public MelonInterpreter(MelonEngine engine) {
            _engine = engine;

            _expressionSolver = new ExpressionSolver(engine);
        }

        public int Execute(Context context) {
            while (context.InstrCounter < context.Instructions.Length) {
                bool goNext = true;

                switch ((OpCode)context.Instruction) {
                    case OpCode.LDBOOL:
                        LoadBoolean(context);
                        break;
                    case OpCode.LDINT:
                        LoadInteger(context);
                        break;
                    case OpCode.LDFLO:
                        LoadDecimal(context);
                        break;
                    case OpCode.LDSTR:
                        LoadString(context);
                        break;
                    case OpCode.ADD:
                    case OpCode.MUL:
                    case OpCode.CEQ:
                    case OpCode.CLT:
                    case OpCode.CGT:
                        SolveOperation(context, (OpCode)context.Instruction);
                        break;
                    case OpCode.LDLOC:
                        LDLOC(context);
                        break;
                    case OpCode.STLOC:
                        STLOC(context);
                        break;
                    case OpCode.LDTYP:
                        LDTYP(context);
                        break;
                    case OpCode.BR:
                        BR(context);
                        goNext = false;
                        break;
                    case OpCode.BRTRUE:
                        BRTRUE(context);
                        goNext = false;
                        break;
                    case OpCode.LDPRP:
                        LDPRP(context);
                        break;
                    case OpCode.CALL:
                        CALL(context);
                        break;
                    case OpCode.LDARG:
                        LDARG(context);
                        break;
                    default:
                        throw new MelonException($"Unknown instruction '{context.Instruction:X4}'");
                }

                if (context._stack.Count > 0) {
                    CompletionValue = context.Last();
                }

                if (goNext) {
                    context.Next();
                }
            }

            return 0;
        }
        private void LoadString(Context context) {
            context.Next();

            // Retrieve string from string table
            context.Push(_engine.CreateString(_engine.Strings[context.Instruction]));
        }

        private void LoadBoolean(Context context) {
            context.Next();

            context.Push(_engine.CreateBoolean(context.Instruction == 1));
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

            context.Push(_engine.CreateFloat(decimalVal));
        }

        private void SolveOperation(Context context, OpCode opCode) {
            var right = context.Pop();
            var left = context.Pop();

            context.Push(_expressionSolver.Solve(opCode, left, right));
        }

        private void STLOC(Context context) {
            context.Next();

            context.Variables[context.Instruction].Variable.value = context.Pop();
        }

        private void LDLOC(Context context) {
            context.Next();

            context.Push(context.Variables[context.Instruction].Variable.value);
        }

        private void LDTYP(Context context) {
            context.Next();

            context.Push(_engine.Types[context.Instruction]);
        }
        private void BR(Context context) {
            context.Next();

            context.Goto(context.Instruction);
        }

        private void LDPRP(Context context) {
            context.Next();

            var value = context.Last();

            context.Push(value.GetProperty(_engine.Strings[context.Instruction]).value);
        }

        private void LDARG(Context context) {
            context.Arguments.Push(context.Pop());
        }

        private void CallFunction(Context context, FunctionInstance functionInstance) {
            var arguments = new List<MelonObject>();

            while (context.Arguments.Count > 0) {
                arguments.Add(context.Arguments.Pop());
            }

            MelonObject self = null;

            if (context._stack.Count > 0) {
                self = context.Pop();
            }

            var returnVal = functionInstance.Run(self, arguments.ToArray());

            if (returnVal != null) {
                context.Push(returnVal);
            }
        }

        private void CALL(Context context) {
            var value = context.Pop();

            if (value is FunctionInstance functionInstance) {
                CallFunction(context, functionInstance);
            }
            else if (value is MelonType melonType && melonType.Properties.TryGetValue("Constructor", out Property member) && member.value is FunctionInstance constructor) {
                CallFunction(context, constructor);
            }
            else {
                throw new MelonException("Object is not a function or type.");
            }
        }

        private void BRTRUE(Context context) {
            context.Next();

            var value = context.Pop();

            if (value is BooleanInstance booleanInstance) {
                if (booleanInstance.value) {
                    context.Goto(context.Instruction);
                }
                else {
                    context.Next();
                }
            }
            else {
                throw new MelonException($"Value must be of type 'bool'");
            }
        }
    }
}
