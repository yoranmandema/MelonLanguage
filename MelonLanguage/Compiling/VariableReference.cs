namespace MelonLanguage.Compiling {
    public class VariableReference {
        public Variable Variable { get; set; }
        public VariableReferenceType Type { get; set; }

        public VariableReference(Variable variable, VariableReferenceType type) {
            Variable = variable;
            Type = type;
        }
    }
}
