using System;
using System.Collections.Generic;
using System.Text;

namespace MelonLanguage {

    public class TypeMapper {
        public Type InputType { get; }
        public Type OutputType { get; }

        public Func<MelonEngine, object, object> Mapper { get; }

        public TypeMapper (Type it, Type ot, Func<MelonEngine, object, object> mapper) {
            InputType = it;
            OutputType = ot;

            Mapper = mapper;
        }
    }
}
