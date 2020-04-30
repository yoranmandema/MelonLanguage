namespace MelonLanguage.Native {
    public class MelonErrorObject : MelonObject {
        public string message;

        public MelonErrorObject(MelonEngine engine, string msg) : base(engine) {
            message = msg;
        }
    }
}
