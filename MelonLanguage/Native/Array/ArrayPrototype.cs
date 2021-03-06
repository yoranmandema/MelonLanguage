﻿using MelonLanguage.Native.Function;

namespace MelonLanguage.Native {
    public class ArrayPrototype : MelonPrototype {

        public ArrayPrototype(MelonEngine engine, MelonType type) : base(engine, type) {
            var properties = new PropertyDictionary {
                ["Length"] = new Property(new NativeFunctionInstance("Length", type, engine, Length)),
                ["Push"] = new Property(new NativeFunctionInstance("Push", type, engine, Push))
            };

            SetProperties(properties);
        }

        [ReturnType(typeof(IntegerType))]
        public MelonObject Length(MelonObject self, Arguments arguments) {
            return Engine.CreateInteger((self as ArrayInstance).values.Count);
        }

        [ReturnType(typeof(ArrayType))]
        [Parameter("item",0,true)]
        public MelonObject Push(MelonObject self, Arguments arguments) {
            var array = (self as ArrayInstance);

            array.values.AddRange(arguments.Values);

            return array;
        }
    }
}
