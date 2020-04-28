using System.Collections.Generic;

namespace MelonLanguage.Native {
    public class MelonObject {
        public Dictionary<string, MelonMember> Members { get; set; } = new Dictionary<string, MelonMember>();
    }
}
