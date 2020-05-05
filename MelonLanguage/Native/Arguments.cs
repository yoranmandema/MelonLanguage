using MelonLanguage.Runtime;

namespace MelonLanguage.Native {
    public class Arguments {
        public MelonObject[] Values { get; }
        public int Length => Values.Length;

        public static Arguments Empty => new Arguments(new MelonObject[0]);

        public Arguments(params MelonObject[] values) {
            Values = values;
        }

        public MelonObject this[int key] {
            get {
                if (key >= 0 && key < Values.Length) {
                    return Values[key];
                }
                else {
                    return null;
                }
            }
        }

        public T GetAs<T>(int i) where T : MelonObject {
            var value = this[i];

            if (value != null && value is T) {
                return value as T;
            }
            else {
                throw new MelonException($"Expected argument of type '{typeof(T).Name}'");
            }
        }

        public override string ToString() {
            return $"[{string.Join(",", Values as object[])}]";
        }
    }
}
