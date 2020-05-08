using MelonLanguage.Extensions;
using System.Collections.Generic;

namespace MelonLanguage.Compiling {
    public class ParseContext {
        public List<int> instructions = new List<int>();
        public Dictionary<int, VariableReference> Variables { get; } = new Dictionary<int, VariableReference>();
        public Dictionary<int, int> BranchLines { get; } = new Dictionary<int, int>();
        public LexicalEnvironment LexicalEnvironment { get; set; }

        public ParseContext(MelonEngine engine, LexicalEnvironment environment) {
            LexicalEnvironment = new LexicalEnvironment(environment, !environment.IsRoot);
        }

        public int AddVariableReference(Variable variable, VariableReferenceType type) {
            var id = Variables.Count;

            Variables[Variables.Count] = new VariableReference(variable, type);

            return id;
        }

        public int GetVariableReference(VariableReference variableReference) {
            return Variables.KeyByValue(variableReference);
        }
    }
}
