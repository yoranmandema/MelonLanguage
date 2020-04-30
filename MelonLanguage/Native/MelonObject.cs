using MelonLanguage.Runtime;
using System.Collections.Generic;

namespace MelonLanguage.Native {
    public abstract class MelonObject {
        public MelonEngine Engine { get; private set; }

        public PropertyDictionary Properties { get; set; } = new PropertyDictionary();

        public MelonObject (MelonEngine engine) {
            Engine = engine;
        }

        public virtual void GenerateFunctions () {
            var methods = GetType().GetMethods();

            for (int i = 0; i < methods.Length; i++) {
                var m = methods[i];

                if (NativeFunction.TryCreateFunction(Engine, m, out NativeFunction function)) {
                    function.Self = this;

                    Properties.Add(m.Name, new Property(function));
                }
            }
        }

        public virtual Property GetProperty (string name) {
            if (Properties.ContainsKey(name)) {
                return Properties[name];
            } else {
                throw new MelonException($"Object does not contain member '{name}'");
            }
        }

        public void SetProperties (PropertyDictionary properties) {
            Properties = properties;
        }
    }
}
