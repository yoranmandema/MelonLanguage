using MelonLanguage.Native;

namespace MelonLanguage.Native {
    public class MelonType : MelonObject {
        public virtual string Name { get; } = "type";
        public MelonEngine Engine { get; private set; }

        public MelonType(MelonEngine engine) {
            Engine = engine;

            var methods = GetType().GetMethods();

            for (int i = 0; i < methods.Length; i++) {
                var m = methods[i];

                if (NativeFunction.TryCreateFunction(Engine, m, out NativeFunction function)) {
                    function.Self = this;

                    Members.Add(m.Name, new MelonMember(m.Name, function));
                }
            }
        }
    }
}
