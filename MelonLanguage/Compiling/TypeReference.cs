using MelonLanguage.Native;
using System.Linq;

namespace MelonLanguage.Compiling {
    public class TypeReference {
        public MelonType Type { get; set; }
        public TypeReference[] GenericTypes { get; set; }

        public TypeReference(MelonType type) {
            Type = type;
        }

        public override string ToString() {
            var str = Type.Name;

            if (GenericTypes != null) {
                str += $"<{string.Join(",", GenericTypes.Select(x => x.Type.Name))}>";
            }

            return str;
        }
    }
}
