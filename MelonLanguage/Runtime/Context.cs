using MelonLanguage.Compiling;
using MelonLanguage.Native;
using System.Collections.Generic;

namespace MelonLanguage.Runtime {
    public class Context {
        public LexicalEnvironment LexicalEnvironment { get; set; }
        public Dictionary<int, VariableReference> Variables { get; } = new Dictionary<int, VariableReference>();

        public Stack<MelonObject> Arguments { get; } = new Stack<MelonObject>();

        public int InstrCounter { get; private set; }

        public int Instruction => Instructions[InstrCounter];

        public int[] Instructions { get; }
        public readonly Stack<MelonObject> _stack = new Stack<MelonObject>();

        public Context(ParseContext parseContext) {
            Instructions = parseContext.instructions.ToArray();
            LexicalEnvironment = parseContext.LexicalEnvironment;
            Variables = new Dictionary<int, VariableReference>(parseContext.Variables);
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
            if (value == null) {
                throw new MelonException($"Invalid value 'null' was pushed to the stack!");
            }

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
