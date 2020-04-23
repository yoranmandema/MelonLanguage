using MelonLanguage.Compiling;
using MelonLanguage.Native;
using System;
using System.Collections.Generic;

namespace MelonLanguage.Runtime.Interpreter {
    public class MelonInterpreter {
        public Stack<MelonObject> Memory { get; } = new Stack<MelonObject>();
        public int InstructionCounter { get; private set; }
        public Dictionary<int, string> Strings { get; }

        private readonly ExpressionSolver _expressionSolver = new ExpressionSolver();

        public MelonInterpreter(Dictionary<int, string> strings) {
            Strings = strings;
        }

        public MelonObject Execute(int[] instructions) {
            InstructionCounter = 0;

            while (InstructionCounter < instructions.Length) {
                switch (instructions[InstructionCounter]) {
                    case (int)OpCode.LDBOOL:
                        LoadBoolean(instructions);
                        break;
                    case (int)OpCode.LDINT:
                        LoadInteger(instructions);
                        break;
                    case (int)OpCode.LDDEC:
                        LoadDecimal(instructions);
                        break;
                    case (int)OpCode.LDSTR:
                        LoadString(instructions);
                        break;
                    case (int)OpCode.ADD:
                        AddOperation(instructions);
                        break;
                    case (int)OpCode.MUL:
                        MulOperation(instructions);
                        break;
                }

                InstructionCounter++;
            }

            return Memory.Peek();
        }
        private void LoadString(int[] instructions) {
            InstructionCounter++;

            // Retrieve string from string table
            Memory.Push(new StringInstance(Strings[instructions[InstructionCounter]]));
        }

        private void LoadBoolean(int[] instructions) {
            InstructionCounter++;

            Memory.Push(new BooleanInstance(instructions[InstructionCounter] == 1));
        }

        private void LoadInteger(int[] instructions) {
            InstructionCounter++;

            Memory.Push(new IntegerInstance(instructions[InstructionCounter]));
        }

        private void LoadDecimal(int[] instructions) {
            InstructionCounter++;
            var left = instructions[InstructionCounter];
            InstructionCounter++;
            var right = instructions[InstructionCounter];

            // Converts 2 int32s into a single double
            long longVal = (long)left << 32 | (uint)right;
            var decimalVal = BitConverter.Int64BitsToDouble(longVal);

            Memory.Push(new DecimalInstance(decimalVal));
        }

        private void AddOperation(int[] instructions) {
            var right = Memory.Pop();
            var left = Memory.Pop();

            Memory.Push(_expressionSolver.Add(left, right));
        }

        private void MulOperation(int[] instructions) {
            var right = Memory.Pop();
            var left = Memory.Pop();

            Memory.Push(_expressionSolver.Mul(left, right));
        }
    }
}
