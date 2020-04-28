using System;
using System.Collections.Generic;
using System.Text;

namespace MelonLanguage.Native {
    public abstract class FunctionInstance : MelonInstance {
        public MelonObject Self { get; }

        public FunctionInstance(MelonEngine engine, MelonType type) : base(engine, type) {

        }

        public abstract MelonObject Run(MelonObject self, params MelonObject[] args);
    }
}
