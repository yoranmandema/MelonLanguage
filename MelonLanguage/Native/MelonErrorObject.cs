namespace MelonLanguage.Native {
    public class MelonErrorObject : MelonObject {
        public string message;

        public MelonErrorObject(string msg) {
            message = msg;
        }
    }
}
