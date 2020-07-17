using MelonLanguage.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MelonLanguage.Native {
    public class ArrayInstance : MelonInstance, IGeneric {
        public List<MelonObject> values;

        public Type[] GenericTypes { get; set; }

        public ArrayInstance(MelonEngine engine, MelonObject[] vals) : base(engine) {
            Type = engine.arrayType;

            values = new List<MelonObject>(vals);
        }

        public override string ToString() {
            return $"[{string.Join(",", values)}]";
        }

        public void SetValue(int index, MelonObject value) {
            if (values.Count == index) {
                values.Add(value);
            }
            else if (values.Count < index) {
                values.AddRange(Enumerable.Repeat(default(MelonObject), values.Count - index));
            }
            else {
                values[index] = value;
            }
        }

        public MelonObject GetValue(int index) {
            if (index < values.Count && index >= 0) {
                return values[index];
            }
            else {
                throw new MelonException("Index out of bounds");
            }
        }
    }
}
