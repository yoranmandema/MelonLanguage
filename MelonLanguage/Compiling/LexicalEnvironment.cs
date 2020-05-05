using MelonLanguage.Native;
using System.Collections.Generic;
using System.Linq;

namespace MelonLanguage.Compiling {
    public class LexicalEnvironment {
        public LexicalEnvironment Parent { get; private set; }
        public List<LexicalEnvironment> Children { get; }
        public Dictionary<string, Variable> Variables { get; } = new Dictionary<string, Variable>();
        public bool IsRoot { get; }

        public LexicalEnvironment Root {
            get {
                LexicalEnvironment parent = Parent;

                if (parent == null || IsRoot) {
                    return this;
                }

                while (parent.Parent?.IsRoot == false) {
                    parent = parent.Parent;
                }

                return parent;
            }
        }

        private readonly MelonEngine _engine;

        public LexicalEnvironment(MelonEngine engine, bool isRoot) {
            IsRoot = isRoot;

            _engine = engine;

            Children = new List<LexicalEnvironment>();
        }

        public LexicalEnvironment(LexicalEnvironment parent, bool isRoot) {
            IsRoot = isRoot;
            Parent = parent;

            Parent.AddChild(this);
            _engine = Parent._engine;

            Variables = new Dictionary<string, Variable>(parent.Variables);
            Children = new List<LexicalEnvironment>();
        }

        public int[] CreateLocals(out string[] names, out MelonObject[] initialValues) {
            var allVars = GetAllVariables();
            var localTypes = new int[allVars.Count];
            var localNames = new string[allVars.Count];
            var localValues = new MelonObject[allVars.Count];

            for (int i = 0; i < allVars.Count; i++) {
                var variable = allVars[i];
                var typeKv = _engine.Types.FirstOrDefault(x => x.Value == variable.type);

                localTypes[i] = typeKv.Key;
                localNames[i] = variable.name;
                localValues[i] = variable.value;
            }

            names = localNames;
            initialValues = localValues;

            return localTypes;
        }

        private List<Variable> GetAllVariables() {
            var variables = new List<Variable>(Variables.Values);

            foreach (var child in Children) {
                variables.AddRange(child.GetAllVariables());
            }

            variables.Sort((a, b) => a.name.CompareTo(b.name));

            return variables;
        }

        public void AddChild(LexicalEnvironment env) {
            env.Parent = this;
            Children.Add(env);
        }

        public Variable GetVariable(string name) {
            if (Variables.ContainsKey(name)) {
                return Variables[name];
            }
            else {
                return Parent?.GetVariable(name);
            }
        }

        public Variable AddVariable(string name, MelonObject value, MelonType type) {
            if (value is MelonInstance melonInstance) {
                type = melonInstance.Type;
            }
            else if (value is MelonType melonType) {
                type = melonType;
            }

            var variable = new Variable { name = name, type = type, value = value };

            Variables[name] = variable;

            return variable;
        }
    }
}
