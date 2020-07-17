using System;

namespace MelonLanguage.Native {
    public class MelonGenericType : MelonType {
        public Type[] Types { get; set; }

        public MelonGenericType(MelonEngine engine) : base(engine) {
        }
    }
}
