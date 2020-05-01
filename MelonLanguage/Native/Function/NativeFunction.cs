using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MelonLanguage.Native {
    public class NativeFunction : FunctionInstance {
        public delegate MelonObject NativeFunctionDelegate(MelonObject self, Arguments arguments);

        public NativeFunctionDelegate Delegate { get; }

        public NativeFunction(MelonEngine engine, NativeFunctionDelegate del) : base(engine) {
            MelonFunctionAttribute functionAttribute = del.Method.GetCustomAttribute<MelonFunctionAttribute>();

            if (functionAttribute != null) {
                ReturnType = functionAttribute.ReturnType;
                ParameterTypes = functionAttribute.ParameterTypes;
            }

            Delegate = del;
        }

        public override MelonObject Run(MelonObject self, params MelonObject[] args) {
            return Delegate.Invoke(self, new Arguments(args));
        }

        public static bool TryCreateFunction (MelonEngine engine, MethodInfo method, out NativeFunction function) {
            if (CheckSignature(method)) {
                var del = (NativeFunctionDelegate)method.CreateDelegate(typeof(NativeFunctionDelegate));

                function = new NativeFunction(engine, del);

                return true;
            }

            function = null;

            return false;
        }

        public static bool CheckSignature(MethodInfo method) {
            if (!method.IsStatic) return false;
            if (!method.IsPublic) return false;
            if (method.ReturnType != typeof(MelonObject)) return false;

            ParameterInfo[] parameters = method.GetParameters();

            if (parameters.Length != 3) return false;
            if (parameters[0].ParameterType != typeof(MelonEngine)) return false;
            if (parameters[1].ParameterType != typeof(MelonObject)) return false;
            if (parameters[2].ParameterType != typeof(Arguments)) return false;

            return true;
        }
    }
}
