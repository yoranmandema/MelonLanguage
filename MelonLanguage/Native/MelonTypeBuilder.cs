//using MelonLanguage.Native.Function;
//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Text;
//using System.Linq;

//namespace MelonLanguage.Native {
//    public class MelonTypeBuilder {
//        public static NativeMethodGroup[] GetMethodGroups(MelonEngine engine, Type type) {
//            Console.WriteLine(type);

//            MethodInfo[] methods = type.GetMethods().OrderBy(x => x.Name).ToArray();

//            List<MethodInfo> group = new List<MethodInfo>();
//            List<NativeMethodGroup> groups = new List<NativeMethodGroup>();

//            for (int i = 0; i < methods.Length; i++) {
//                MethodInfo method = methods[i];

//                if (method.IsSpecialName && (method.Name.StartsWith("get_") || method.Name.StartsWith("set_"))) {
//                    continue;
//                }

//                Console.WriteLine($"Starting group {method.Name}");

//                while ((group.Count == 0 || method.Name == group.Last().Name) && i < methods.Length) {
//                    group.Add(method);
//                    Console.WriteLine($"Added {method}");

//                    i++;

//                    method = methods[i];
//                }

//                NativeMethodGroup methodGroupInstance = MelonFunctionBuilder.FromMethodGroup(engine, group.ToArray());

//                groups.Add(methodGroupInstance);
//                group.Clear();
//            }

//            return groups.ToArray();
//        }
//    }
//}
