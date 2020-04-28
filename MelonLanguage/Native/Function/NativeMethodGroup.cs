//using MelonLanguage.Runtime;
//using System;
//using System.Reflection;
//using System.Reflection.Emit;

//namespace MelonLanguage.Native.Function {
//    public class NativeMethodGroup : FunctionInstance {
//        public MethodInfo[] Methods { get; }

//        public override string Name { get; }

//        public NativeMethodGroup(MelonEngine engine, MelonType type, MethodInfo[] methods) : base(engine, type) {
//            Methods = methods;

//            Name = methods[0].Name;
//        }

//        private static bool CheckType(MelonEngine engine, Type inputType, Type outputType) {
//            TypeMapper tm = engine.TypeMappers.Find(tm => tm.InputType == inputType && tm.OutputType == outputType);

//            return (typeof(MelonObject).IsAssignableFrom(inputType) && typeof(MelonObject).IsAssignableFrom(outputType)) || (tm != null);
//        }

//        public static bool FindMatchingMethod (MelonEngine engine, MethodInfo[] methods, Type returnType, MelonObject[] args, out MethodInfo method) {
//            method = null;

//            for (int i = 0; i < methods.Length; i++) {
//                var m = methods[i];

//                if (!CheckType(engine, m.ReturnType, returnType)) continue; // Return type invalid, skip to next method

//                var parameters = m.GetParameters();

//                for (int j = 0; j < parameters.Length; j++) {
//                    var p = parameters[j];

//                    if (!CheckType(engine, args[i].GetType(), p.ParameterType)) continue; // Parameter type invalid, skip to next method
//                }

//                // Method has fitting parameters
//                method = m;
//                return true;
//            }

//            return false;
//        }

//        public override MelonObject Run(Type returnType, params MelonObject[] args) {
//            Console.WriteLine(string.Join(",", (object[])args));

//            if (FindMatchingMethod(Engine, Methods, returnType, args, out MethodInfo method)) {
//                object self = null;
                
//                method.Invoke(self, args);
//            }

//            throw new MelonException($"No suitable method found for {returnType} ({string.Join(",", (object[])args)})");
//        }
//    }
//}
