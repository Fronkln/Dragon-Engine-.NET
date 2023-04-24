using System;

namespace Y7MP
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class MpRPCAttribute : Attribute
    {
        public RPCEvent eventID;

        public MpRPCAttribute(RPCEvent eventID)
        {
            this.eventID = eventID;
        }
    }
}
