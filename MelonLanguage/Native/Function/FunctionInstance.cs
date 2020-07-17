using System;
using System.Text;

namespace MelonLanguage.Native {
    public abstract class FunctionInstance : MelonInstance {
        public string Name { get; }
        public MelonObject Self { get; set; }
        public Type ReturnType { get; set; }
        public FunctionParameter[] ParameterTypes { get; set; }

        public FunctionInstance(string name, MelonEngine engine) : base(engine) {
            Type = engine.functionType;
            Name = name;
        }

        public abstract MelonObject Run(MelonObject self, params MelonObject[] args);

        public override string ToString() {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("fn ");
            if (ReturnType != null) {
                stringBuilder.Append(ReturnType.Name).Append(' ');
            }

            stringBuilder.Append(Name);
            stringBuilder.Append(" (");

            if (ParameterTypes != null) {
                int i = 0;

                foreach (var p in ParameterTypes) {
                    if (p.IsVarargs) {
                        stringBuilder.Append("varargs ");
                    }

                    if (p.Type != null) {
                        stringBuilder.Append(p.Type).Append(' ');
                    }

                    stringBuilder.Append(p.Name);

                    if (i < ParameterTypes.Length - 1) {
                        stringBuilder.Append(",");
                    }

                    i++;
                }
            }

            stringBuilder.Append(")");

            return stringBuilder.ToString();
        }
    }
}
