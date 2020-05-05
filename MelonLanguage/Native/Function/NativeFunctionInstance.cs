using System.Reflection;

namespace MelonLanguage.Native {
    public class NativeFunctionInstance : FunctionInstance {
        public delegate MelonObject NativeFunctionDelegate(MelonObject self, Arguments arguments);

        public NativeFunctionDelegate Delegate { get; }

        public NativeFunctionInstance(string name, MelonEngine engine, NativeFunctionDelegate del) : base(name, engine) {
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

        public static bool TryCreateFunction(MelonEngine engine, MethodInfo method, out NativeFunctionInstance function) {
            if (CheckSignature(method)) {
                var del = (NativeFunctionDelegate)method.CreateDelegate(typeof(NativeFunctionDelegate));

                function = new NativeFunctionInstance(del.Method.Name, engine, del);

                return true;
            }

            function = null;

            return false;
        }

        public static bool CheckSignature(MethodInfo method) {
            if (!method.IsStatic) {
                return false;
            }

            if (!method.IsPublic) {
                return false;
            }

            if (method.ReturnType != typeof(MelonObject)) {
                return false;
            }

            ParameterInfo[] parameters = method.GetParameters();

            if (parameters.Length != 3) {
                return false;
            }

            if (parameters[0].ParameterType != typeof(MelonEngine)) {
                return false;
            }

            if (parameters[1].ParameterType != typeof(MelonObject)) {
                return false;
            }

            if (parameters[2].ParameterType != typeof(Arguments)) {
                return false;
            }

            return true;
        }
    }
}
