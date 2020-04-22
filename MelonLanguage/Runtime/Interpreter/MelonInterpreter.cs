using MelonLanguage.Compiling;
using MelonLanguage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MelonLanguage.Runtime.Interpreter {
    public class MelonInterpreter {
        public Stack<MelonObject> Memory { get; private set; }  = new Stack<MelonObject>();
        public int InstructionCounter { get; private set; }
        public string[] Strings { get; private set; }

        public MelonInterpreter (string[] strings) {
            Strings = strings;
        }

        public MelonObject Execute (int[] instructions) {
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
            }

            return Memory.Peek();
        }
        private void LoadString(int[] instructions) {
            InstructionCounter++;

            // Retrieve string from string table
            Memory.Push(new StringInstance(Strings[instructions[InstructionCounter]]));

            InstructionCounter++;
        }

        private void LoadBoolean(int[] instructions) {
            InstructionCounter++;

            Memory.Push(new BooleanInstance(instructions[InstructionCounter] == 1));

            InstructionCounter++;
        }

        private void LoadInteger (int[] instructions) {
            InstructionCounter++;

            Memory.Push(new IntegerInstance(instructions[InstructionCounter]));

            InstructionCounter++;
        }

        private void LoadDecimal(int[] instructions) {
            InstructionCounter++;
            var left = instructions[InstructionCounter];
            InstructionCounter++;
            var right = instructions[InstructionCounter];

            // Converts 2 int32s into a single double
            long longVal = (long)left << 32 | (long)(uint)right;
            var decimalVal = BitConverter.Int64BitsToDouble(longVal);

            Memory.Push(new DecimalInstance(decimalVal));

            InstructionCounter++;
        }

        private void AddOperation (int[] instructions) {
            var right = Memory.Pop();
            var left = Memory.Pop();

            var result = default(MelonObject);

            if (left is IntegerInstance && right is IntegerInstance) {
                result = new IntegerInstance((left as IntegerInstance).value + (right as IntegerInstance).value);
            } else if (left is IntegerInstance && right is DecimalInstance) {
                result = new DecimalInstance((left as IntegerInstance).value + (right as DecimalInstance).value);
            } else if (left is DecimalInstance li && right is IntegerInstance) {
                result = new DecimalInstance((left as DecimalInstance).value + (right as IntegerInstance).value);
            }

            Memory.Push(result);

            InstructionCounter++;
        }

        private void MulOperation(int[] instructions) {
            var right = Memory.Pop();
            var left = Memory.Pop();

            var result = default(MelonObject);

            if (left is IntegerInstance && right is IntegerInstance) {
                result = new IntegerInstance((left as IntegerInstance).value * (right as IntegerInstance).value);
            }
            else if (left is IntegerInstance && right is DecimalInstance) {
                result = new DecimalInstance((left as IntegerInstance).value * (right as DecimalInstance).value);
            }
            else if (left is DecimalInstance li && right is IntegerInstance) {
                result = new DecimalInstance((left as DecimalInstance).value * (right as IntegerInstance).value);
            }

            Memory.Push(result);

            InstructionCounter++;
        }
    }
}
