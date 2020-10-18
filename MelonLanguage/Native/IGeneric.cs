using MelonLanguage.Compiling;
using System;

namespace MelonLanguage.Native {
    internal interface IGeneric {
        public TypeReference[] GenericTypes { get; set; }
    }
}
