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
