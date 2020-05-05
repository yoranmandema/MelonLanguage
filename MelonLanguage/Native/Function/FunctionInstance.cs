using System;

namespace MelonLanguage.Native {
    public abstract class FunctionInstance : MelonInstance {
        public string Name { get; }
        public MelonObject Self { get; set; }
        public Type ReturnType { get; set; }
        public Type[] ParameterTypes { get; set; }

        public FunctionInstance(string name, MelonEngine engine) : base(engine) {
            Type = engine.functionType;
            Name = name;
        }

        public abstract MelonObject Run(MelonObject self, params MelonObject[] args);
    }
}
