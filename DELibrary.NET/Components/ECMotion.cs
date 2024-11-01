using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using DragonEngineLibrary.Service;

namespace DragonEngineLibrary
{
    public class ECMotion : ECCharaBaseComponent
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_REQUESTBEHAVIOR", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr DELib_ECMotion_RequestBehavior(IntPtr motion, MotionBehaviorType type, BehaviorActionID action);

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

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_GET_FRAME", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_ECMotion_GetFrame(IntPtr motion);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_GET_FRAMES", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_ECMotion_GetFrames(IntPtr motion);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_IS_LAST_FRAME", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        private static extern bool DELib_ECMotion_IsLastFrame(IntPtr motion);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_GET_BEP_CURRENT_TICK", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_ECMotion_Getter_CurrentBepTick(IntPtr motion);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_GET_BEP_LAST_TICK", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_ECMotion_Getter_LastBepTick(IntPtr motion);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_GET_BEP_ID", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_ECMotion_Getter_BepID(IntPtr motion);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_GET_BEHAVIOR_STATE", CallingConvention = CallingConvention.Cdecl)]
        private static extern BehaviorActionID DELib_ECMotion_Get_Behavior_State(IntPtr motion);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_GET_BEHAVIOR_SET", CallingConvention = CallingConvention.Cdecl)]
        private static extern BehaviorSetID DELib_ECMotion_Get_Behavior_Set(IntPtr motion, MotionBehaviorType type);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_GET_GMT_ID", CallingConvention = CallingConvention.Cdecl)]
        private static extern MotionID DELib_ECMotion_Getter_GMTID(IntPtr motion);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_SET_TEMP_MOTION_SPEED", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_ECMotion_SetTempMotionSpeed(IntPtr motion, float speed);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_SET_FRAME", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_ECMotion_SetFrame(IntPtr motion, uint tick);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECMOTION_GETTER_SYNC_SERIAL", CallingConvention = CallingConvention.Cdecl)]
        private static extern int DELib_ECMotion_Getter_SyncSerial(IntPtr motion);


        ///<summary>Motion play info.</summary>
        public MotionPlayInfo PlayInfo
        {
            get
            {
                unsafe
                {
                    IntPtr result = DELib_ECMotion_Getter_PlayInfo(Pointer);

                    if (result == IntPtr.Zero)
                        return new MotionPlayInfo();
                    else
                        return *((MotionPlayInfo*)result);
                }
          //      IntPtr result = DELib_ECMotion_Getter_PlayInfo(Pointer);
             //   MotionPlayInfo inf = (result == IntPtr.Zero ? new MotionPlayInfo() : Marshal.PtrToStructure<MotionPlayInfo>(result));

              //  return inf;
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

        public MotionID GmtID
        {
            get
            {
                return DELib_ECMotion_Getter_GMTID(Pointer);
            }
        }

        public uint BepID
        {
            get
            {
                return DELib_ECMotion_Getter_BepID(Pointer);
            }
        }


        public uint Frame
        {
            get
            {
                return DELib_ECMotion_GetFrame(Pointer) / 100;
            }
        }

        public uint Frames
        {
            get
            {
                return DELib_ECMotion_GetFrames(Pointer) / 100; 
            }
        }

        public GameTick BepCurrentTick
        {
            get
            {
                return new GameTick(DELib_ECMotion_Getter_CurrentBepTick(Pointer));
            }
        }

        public GameTick BepLastTick
        {
            get
            {
                return new GameTick(DELib_ECMotion_Getter_LastBepTick(Pointer));
            }
        }

        /// <summary>
        /// Progress of animation shown from 0.0f to 1.0f
        /// </summary>
        public float NormalizedTime
        {
            get
            {
                uint ticks = Frames;
                uint now = Frame;

              //  DragonEngine.Log(ticks + "   " + now);

                return (float)now / (float)ticks;
            }
        }

        public int SyncSerial
        {
            get
            {
                return DELib_ECMotion_Getter_SyncSerial(Pointer);
            }
        }


        ///<summary>Request a behavior to be played.</summary>
        [DECompatibility(DEGames.YLAD)]
        public void RequestBehavior(MotionBehaviorType type, BehaviorActionID action)
        {
            DELib_ECMotion_RequestBehavior(_objectAddress, type, action);
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

        public bool RequestedAnimPlaying()
        {
            return GmtID != MotionID.invalid;
        }

        public BehaviorActionID GetBehaviorState()
        {
            return DELib_ECMotion_Get_Behavior_State(Pointer);
        }

        public BehaviorSetID GetBehaviorSet(MotionBehaviorType type)
        {
            return DELib_ECMotion_Get_Behavior_Set(Pointer, type);
        }

        public void SetFrame(uint tick)
        {
            MotionPlayInfo inf = PlayInfo;
            inf.tick_now_ = tick;
            inf.tick_gmt_now_ = tick;

            PlayInfo = inf;
            //DELib_ECMotion_SetFrame(Pointer, tick);
        }

        public bool IsLastFrame()
        {
            return DELib_ECMotion_IsLastFrame(Pointer);
        }

        public bool InTimingRange(uint nodeID)
        {
            MotionPlayInfo inf = PlayInfo;
            MotionService.TimingResult timing = MotionService.SearchTimingDetail(inf.tick_now_, BepID, nodeID);

            if (!timing.IsValid())
                return false;

            return Frame >= timing.Start / 100 && Frame <= timing.End / 100;
        }

        public bool InTimingRange(uint nodeID, uint tick)
        {
            MotionPlayInfo inf = PlayInfo;
            MotionService.TimingResult timing = MotionService.SearchTimingDetail(tick, BepID, nodeID);

            if (!timing.IsValid())
                return false;

            return inf.tick_now_ >= timing.Start && inf.tick_now_ <= timing.End;
        }

        public MotionService.TimingResult GetTiming(uint nodeID, uint tick = 0)
        {
            MotionPlayInfo inf = PlayInfo;
            MotionService.TimingResult timing = MotionService.SearchTimingDetail(tick, BepID, nodeID);

            return timing;
        }


        public void SetTempSpeed(float speed)
        {
            DELib_ECMotion_SetTempMotionSpeed(Pointer, speed);
        }
    }
}
