using MelonLanguage.Native;
using System.Linq;

namespace MelonLanguage.Compiling {
    public class TypeReference {
        public int TypeId { get; }
        public MelonType Type => Engine.GetType(TypeId);
        public TypeReference[] GenericTypes { get; internal set; }

        public  MelonEngine Engine { get;  }

        private TypeReference(MelonEngine engine) {
            Engine = engine;
        }

        public TypeReference(MelonEngine engine, MelonType type) : this(engine) {
            TypeId = engine.GetTypeID(type);
        }

        public TypeReference(MelonEngine engine, int type) : this(engine) {
            TypeId = type;
        }


        public bool IsEqualTo (TypeReference type) {
            if (Type != type.Type) {
                return false;
            }

            if (GenericTypes?.Any() == true && type.GenericTypes?.Any() == true) {
                if (GenericTypes.Length != GenericTypes.Length) {
                    return false;
                }

                for (int i = 0; i < GenericTypes.Length; i++) {
                    if (!GenericTypes[i].IsEqualTo(type.GenericTypes[i])) {
                        return false;
                    }
                }
            }

            return true;
        }

        public override string ToString() {
            var str = Type.Name;

            if (GenericTypes != null) {
                str += $"<{string.Join(",", GenericTypes.Select(x => x.ToString()))}>";
            }

            return str;
        }
    }
}
