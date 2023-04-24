using System;
using System.Reflection;
using System.Collections.Generic;
using DragonEngineLibrary;

namespace Y7MP
{
    public static class ReflectionCache
    {
        //SimpleNetworkedEntity only
        public static Dictionary<string, Type> ClassNameCache = new Dictionary<string, Type>();
        public static Dictionary<Type, MethodInfo> CreationMethodCache = new Dictionary<Type, MethodInfo>();

        public static void Cache()
        {
            ClassNameCache.Clear();
            CreationMethodCache.Clear();

            Assembly assmb = Assembly.GetExecutingAssembly();

            Type[] mpTypes = assmb.GetTypes();
            Type targetType = typeof(SimpleNetworkedEntity);

            foreach (Type type in mpTypes)
            {
                if (type.BaseType != targetType)
                    continue;

                ClassNameCache.Add(type.Name, type);

                MethodInfo[] typeFunctions = type.GetMethods();

                foreach (MethodInfo inf in typeFunctions)
                {
                    MpRPCAttribute attrib = inf.GetCustomAttribute<MpRPCAttribute>();

                    if (attrib != null)
                    {
                        RPCMethods.RegisteredRPCs.Add(attrib.eventID, inf);
#if DEBUG
                        DragonEngine.Log("Registered function " + inf.Name + " of class " + type.Name + " for RPC: " + attrib.eventID);
#endif
                    }

                    if (inf.Name == "Create")
                        CreationMethodCache.Add(type, inf);
                }
            }
        }
    }
}
