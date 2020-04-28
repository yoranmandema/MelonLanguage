using MelonLanguage.Native;
using System.Collections.Generic;

namespace MelonLanguage.Runtime {
    public class Context {

        public int[] LocalTypes { get; }
        public string[] LocalNames { get; }
        public MelonObject[] LocalValues { get; }

        public int InstrCounter { get; private set; }

        public int Instruction => _instructions[InstrCounter];

        public int[] Instructions => _instructions;

        private readonly int[] _instructions;
        public readonly Stack<MelonObject> _stack = new Stack<MelonObject>();

        public Context(string[] localNames, int[] localTypes, int[] instructions) {
            _instructions = instructions;
            LocalNames = localNames;
            LocalTypes = localTypes;

            LocalValues = new MelonObject[localTypes.Length];
        }

        public void Reset() {
            InstrCounter = 0;
            _stack.Clear();
        }

        public void Goto(int instruction) {
            InstrCounter = instruction;
        }

        public void Next() {
            InstrCounter++;
        }

        public void Push(MelonObject value) {
            _stack.Push(value);
        }

        public MelonObject Last() {
            return _stack.Peek();
        }

        public MelonObject Pop() {
            return _stack.Pop();
        }
    }
}
