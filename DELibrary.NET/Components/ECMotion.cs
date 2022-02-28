using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ECMotion : ECCharaBaseComponent
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_REQUESTGMT", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr DELib_ECMotion_RequestGMT(IntPtr motion, uint gmt_id);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_GETTER_PLAYINFO", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr DELib_ECMotion_Getter_PlayInfo(IntPtr motion);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_SETTER_PLAYINFO", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_ECMotion_Setter_PlayInfo(IntPtr motion, IntPtr inf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_GETTER_BHV_PARTS_INFO", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr DELib_ECMotion_Getter_BhvPartsInfo(IntPtr motion);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_SETTER_BHV_PARTS_INFO", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_ECMotion_Setter_BhvPartsInfo(IntPtr motion, IntPtr inf);

        public MotionPlayInfo PlayInfo
        {
            get
            {
                IntPtr result = DELib_ECMotion_Getter_PlayInfo(Pointer);
                MotionPlayInfo inf = (result == IntPtr.Zero ? new MotionPlayInfo() : Marshal.PtrToStructure<MotionPlayInfo>(result));

                return inf;
            }
            set
            {
                IntPtr obj = value.ToIntPtr();
                DELib_ECMotion_Setter_PlayInfo(Pointer, obj);

                Marshal.FreeHGlobal(obj);
            }
        }
        public MotionPlayInfo BhvPartsInfo
        {
            get
            {
                IntPtr result = DELib_ECMotion_Getter_BhvPartsInfo(Pointer);
                MotionPlayInfo inf = (result == IntPtr.Zero ? new MotionPlayInfo() : Marshal.PtrToStructure<MotionPlayInfo>(result));

                return inf;
            }
            set
            {
                IntPtr obj = value.ToIntPtr();
                DELib_ECMotion_Setter_BhvPartsInfo(Pointer, obj);

                Marshal.FreeHGlobal(obj);
            }
        }

        public void RequestGMT(MotionID gmt)
        {
            RequestGMT((uint)gmt);
        }
        public void RequestGMT(uint gmt)
        {
            DELib_ECMotion_RequestGMT(_objectAddress, gmt);
        }
    }
}
