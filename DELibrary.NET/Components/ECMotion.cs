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

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_REQUESTRAGDOLL", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_ECMotion_RequestRagdoll(IntPtr motion, IntPtr opts);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_GETTER_PLAYINFO", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr DELib_ECMotion_Getter_PlayInfo(IntPtr motion);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_SETTER_PLAYINFO", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_ECMotion_Setter_PlayInfo(IntPtr motion, IntPtr inf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_GETTER_BHV_PARTS_INFO", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr DELib_ECMotion_Getter_BhvPartsInfo(IntPtr motion);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_SETTER_BHV_PARTS_INFO", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_ECMotion_Setter_BhvPartsInfo(IntPtr motion, IntPtr inf);

        ///<summary>Motion play info.</summary>
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
        ///<summary>Motion behavior parts info.</summary>
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

        ///<summary>Request a GMT to be played.</summary>
        [DECompatibility(DEGames.YK2 | DEGames.YLAD)]
        public void RequestGMT(MotionID gmt)
        {
            RequestGMT((uint)gmt);
        }
        ///<summary>Request a GMT to be played.</summary>
        [DECompatibility(DEGames.YK2 | DEGames.YLAD)]
        public void RequestGMT(uint gmt)
        {
            DELib_ECMotion_RequestGMT(_objectAddress, gmt);
        }

        ///<summary>Request to be ragdolled.</summary>
        [DECompatibility(DEGames.YK2 | DEGames.YLAD)]
        public void RequestRagdoll(RequestRagdollOptions opts)
        {
            IntPtr optsPtr = opts.ToIntPtr();
            DELib_ECMotion_RequestRagdoll(Pointer, optsPtr);

            Marshal.FreeHGlobal(optsPtr);
        }
    }
}
