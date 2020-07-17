using MelonLanguage.Compiling;
using MelonLanguage.Native;
using System.Collections.Generic;

namespace MelonLanguage.Runtime {
    public class Context {
        public LexicalEnvironment LexicalEnvironment { get; set; }
        public Dictionary<int, VariableReference> Variables { get; private set; } = new Dictionary<int, VariableReference>();

        public Stack<MelonObject> Arguments { get; } = new Stack<MelonObject>();

        public MelonObject ReturnValue { get; set; }

        public int InstrCounter { get; private set; }

        public int Instruction => Instructions[InstrCounter];

        public int[] Instructions { get; private set; }
        public readonly Stack<MelonObject> _stack = new Stack<MelonObject>();

        public Context(ParseContext parseContext) {
            Instructions = parseContext.instructions.ToArray();
            LexicalEnvironment = parseContext.LexicalEnvironment;
            Variables = new Dictionary<int, VariableReference>(parseContext.Variables);
        }

        internal void SetArguments(MelonObject[] args) {
            int i = 0;

            foreach (var kv in Variables) {
                if (kv.Value.Type == VariableReferenceType.Argument) {
                    Variables[kv.Key].Variable.value = args[i];
                    i++;
                }
            }
        }

        private Context() {
        }

        public Context Clone() {
            var variableReferences = new Dictionary<int, VariableReference>(this.Variables);

            foreach (var kv in variableReferences) {
                if (kv.Value.Type == VariableReferenceType.Local || kv.Value.Type == VariableReferenceType.Argument) {
                    variableReferences[kv.Key].Variable = new Variable() {
                        name = kv.Value.Variable.name,
                        value = kv.Value.Variable.value,
                        type = kv.Value.Variable.type,
                    };
                }
            }

            return new Context() {
                Instructions = this.Instructions,
                LexicalEnvironment = this.LexicalEnvironment,
                Variables = new Dictionary<int, VariableReference>(this.Variables)
            };
        }

        public void Reset() {
            InstrCounter = 0;
            _stack.Clear();
        }

        public void Finish() {
            InstrCounter = Instructions.Length;
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
