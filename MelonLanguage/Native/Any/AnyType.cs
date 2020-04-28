using System;
using System.Collections.Generic;
using System.Text;

namespace MelonLanguage.Native {
    public class AnyType : MelonType {
        public override string Name => "any";

        public AnyType(MelonEngine engine) : base(engine) {
        }
    }
}
