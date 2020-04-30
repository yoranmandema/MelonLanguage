using System;
using System.Collections.Generic;
using System.Text;

namespace MelonLanguage.Native {
    public abstract class FunctionInstance : MelonInstance {
        public MelonObject Self { get; set; }

        public FunctionInstance(MelonEngine engine) : base(engine) {
            Type = engine.functionType;
        }

        public abstract MelonObject Run(MelonObject self, params MelonObject[] args);
    }
}
