namespace MelonLanguage.Native {
    public abstract class MelonInstance : MelonObject {
        public MelonType Type { get; set; }
        public MelonPrototype Prototype => Type.Prototype;

        public MelonInstance(MelonEngine engine) : base(engine) {
        }

        public override Property GetProperty(string name) {
            if (Prototype.Properties.ContainsKey(name)) {
                return Prototype.GetProperty(name);
            }
            else {
                return base.GetProperty(name);
            }
        }
    }
}
