using MelonLanguage.Native;

namespace MelonLanguage.Native {
    public class MelonType : MelonObject {
        public virtual string Name { get; } = "type";
        public MelonPrototype Prototype { get; internal set; }

        public MelonType(MelonEngine engine) : base(engine) {
        }
    }
}
