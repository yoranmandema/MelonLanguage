using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MelonLanguage.Compiling {
    public class LexicalEnvironment {

        public LexicalEnvironment Parent { get; private set; }
        public List<LexicalEnvironment> Children { get; }
        public Dictionary<string, Variable> Variables { get; }

        public LexicalEnvironment Root { get {
                LexicalEnvironment parent = Parent;

                if (parent == null) return this;

                while (parent.Parent != null) {
                    parent = parent.Parent;
                }

                return parent;
            }
        }

        private readonly MelonEngine _engine;

        public LexicalEnvironment (MelonEngine engine) {
            _engine = engine;
            Variables = new Dictionary<string, Variable>();
            Children = new List<LexicalEnvironment>();
        }

        public LexicalEnvironment (LexicalEnvironment parent) {
            Parent = parent;

            Parent.AddChild(this);
            _engine = Parent._engine;

            Variables = new Dictionary<string, Variable>();
            Children = new List<LexicalEnvironment>();
        }

        public int[] CreateLocals (out string[] names) {
            var allVars = GetAllVariables();
            var localTypes = new int[allVars.Count];
            var localNames = new string[allVars.Count];

            for (int i = 0; i < allVars.Count; i++) {
                var variable = allVars[i];
                var typeKv = _engine.Types.FirstOrDefault(x => x.Value == variable.type);

                localTypes[i] = typeKv.Key;
                localNames[i] = variable.name;
            }

            names = localNames;

            return localTypes;
        }

        private List<Variable> GetAllVariables () {
            var variables = new List<Variable>(Variables.Values);

            foreach (var child in Children) {
                variables.AddRange(child.GetAllVariables());
            }

            variables.Sort((a, b) => a.id - b.id);

            return variables;
        }

        public void AddChild (LexicalEnvironment env) { 
            env.Parent = this;
            Children.Add(env);
        }

        public Variable GetVariable (string name) {
            if (Variables.ContainsKey(name)) {
                return Variables[name];
            } else {
                return Parent?.GetVariable(name);
            }
        }
    }
}
