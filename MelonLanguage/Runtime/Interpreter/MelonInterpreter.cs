using MelonLanguage.Compiling;
using MelonLanguage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MelonLanguage.Runtime.Interpreter {
    public class MelonInterpreter {
        private readonly ExpressionSolver _expressionSolver;
        private readonly MelonEngine _engine;
        private MelonObject accessed;

        public MelonInterpreter(MelonEngine engine) {
            _engine = engine;

            _expressionSolver = new ExpressionSolver(engine);
        }

        public MelonObject Execute(Context context) {

            while (context.InstrCounter < context.Instructions.Length) {
                bool goNext = true;

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
                    case (int)OpCode.MUL:
                    case (int)OpCode.CEQ:
                    case (int)OpCode.CLT:
                    case (int)OpCode.CGT:
                        SolveOperation(context, (OpCode)context.Instruction);
                        break;
                    case (int)OpCode.LDLOC:
                        LDLOC(context);
                        break;
                    case (int)OpCode.STLOC:
                        STLOC(context);
                        break;
                    case (int)OpCode.LDTYP:
                        LDTYP(context);
                        break;
                    case (int)OpCode.BR:
                        BR(context);
                        goNext = false;
                        break;
                    case (int)OpCode.BRTRUE:
                        BRTRUE(context);
                        goNext = false;
                        break;
                    case (int)OpCode.GTMEM:
                        GTMEM(context);
                        break;
                    case (int)OpCode.CALL:
                        CALL(context);
                        break;
                    default:
                        throw new MelonException($"Unknown instruction '{context.Instruction:X4}'");
                }

                if (goNext) {
                    context.Next();
                }
            }

            return context.Last();
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

        private void AddOperation(Context context) {
            context.Push(_expressionSolver.Add(context.Pop(), context.Pop()));
        }

        private void MulOperation(Context context) {
            context.Push(_expressionSolver.Mul(context.Pop(), context.Pop()));
        }

        private void STLOC(Context context) {
            context.Next();

            context.LocalValues[context.Instruction] = context.Last();
        }

        private void LDLOC(Context context) {
            context.Next();

            context.Push(context.LocalValues[context.Instruction]);
        }

        private void LDTYP(Context context) {
            context.Next();

            context.Push(_engine.Types[context.Instruction]);
        }
        private void BR(Context context) {
            context.Next();

            context.Goto(context.Instruction);
        }

        private void GTMEM(Context context) {
            context.Next();

            var value = context.Pop();

            context.Push(value.Members[_engine.Strings[context.Instruction]].value);
        }

        private void CallFunction (Context context, FunctionInstance functionInstance) {
            var arguments = new List<MelonObject>();

            while (context._stack.Count > 0) {
                arguments.Add(context.Pop());
            }

            var returnVal = functionInstance.Run(arguments.ToArray());

            if (returnVal != null)
                context.Push(returnVal);

            context.Next();
        }

        private void CALL(Context context) {
            context.Next();

            var value = context.Pop();

            if (value is FunctionInstance functionInstance) {
                CallFunction(context, functionInstance);
            }
            else if (value is MelonType melonType && melonType.Members.TryGetValue("Constructor", out MelonMember member)) {
                if (member.value is FunctionInstance constructor) {
                    CallFunction(context, constructor);
                } else {
                    throw new MelonException("Object is not a function or type");
                }
            }
            else {
                throw new MelonException("Object is not a function or type");
            }
        }

        private void BRTRUE(Context context) {
            context.Next();

            var value = context.Pop();

            if (value is BooleanInstance booleanInstance) {
                if (booleanInstance.value) {
                    context.Goto(context.Instruction);
                }
            }
            else {
                throw new MelonException($"Value must be of type 'bool'");
            }
        }
    }
}
