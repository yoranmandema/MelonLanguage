using System;
using System.Collections.Generic;
using System.Text;

namespace MelonLanguage.Native {
    public class IntegerType : MelonType {
        public override string Name { get; } = "int";

        public IntegerType(MelonEngine engine) : base(engine) {
        }

        [MelonFunction]
        public IntegerInstance Construct (int value) {
            return new IntegerInstance(Engine,this,value);
        }
    }
}
