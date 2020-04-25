using System;
using System.Collections.Generic;
using System.Text;

namespace MelonLanguage.Native {
    public abstract class MelonType {
        public abstract string Name { get; }
        public MelonEngine Engine { get; private set; }

        public MelonType (MelonEngine engine) {
            Engine = engine;
        }
    }
}
