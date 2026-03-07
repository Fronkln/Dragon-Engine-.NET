using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public class Posture : UnmanagedObject
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_POSTURE_GET_NODE_ID", CallingConvention = CallingConvention.Cdecl)]
        private static extern short DELib_Posture_GetNodeID(IntPtr posture, ref PXDHash boneName);

        /// <summary>
        /// Find node (also known as a bone) ID
        /// </summary>
        public short GetNodeID(string boneName)
        {
            PXDHash boneHash = new PXDHash(boneName);
            
            return DELib_Posture_GetNodeID(Pointer, ref boneHash);
        }
    }
}
