using System;
using System.Collections.Generic;
using System.Reflection;
using DragonEngineLibrary;

namespace Y7MP
{
    public static class RPCMethods
    {
        public static Dictionary<RPCEvent, MethodInfo> RegisteredRPCs = new Dictionary<RPCEvent, MethodInfo>();


    }

    public enum RPCEvent : ushort
    {

    }
}
