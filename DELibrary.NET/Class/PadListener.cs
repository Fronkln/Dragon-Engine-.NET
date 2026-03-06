using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class PadListener : UnmanagedObject
    {
        public IntPtr DeviceSlot
        {
            get
            {
                return _Getter_DeviceSlot();
            }
            set
            {
                _Setter_DeviceSlot(value);
            }
        }

        //janky but will do the job for now
        private IntPtr _Getter_DeviceSlot()
        {
            if(Pointer == IntPtr.Zero)
                return IntPtr.Zero;

            return (IntPtr)Marshal.ReadInt64(Pointer + 8);
        }

        private void _Setter_DeviceSlot(IntPtr slot)
        {
            if (Pointer == IntPtr.Zero)
                return;

            Marshal.WriteInt64(Pointer + 8, (long)slot);
        }
    }
}
