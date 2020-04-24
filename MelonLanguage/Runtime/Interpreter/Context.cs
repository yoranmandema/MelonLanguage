using MelonLanguage.Native;
using System;
using System.Collections.Generic;
using System.Text;

namespace MelonLanguage.Runtime {
    public class Context {
        public int InstrCounter { get; private set; }

        public int Instruction { get {
                return _instructions[InstrCounter];
            }
        }

        private readonly int[] _instructions;
        private readonly Stack<MelonObject> _stack = new Stack<MelonObject>();

        public Context (int[] instructions) {
            _instructions = instructions;
        }

        public void Next () {
            InstrCounter++;
        }

        public void Push (MelonObject value) {
            _stack.Push(value);
        }

        public MelonObject Last () {
            return _stack.Peek();
        }

        public MelonObject Pop() {
            return _stack.Pop();
        }
    }
}
