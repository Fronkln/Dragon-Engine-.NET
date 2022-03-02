using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;
using DragonEngineLibrary;
using Steamworks;

namespace Y7MP
{
    public static class Extensions
    {
        public static byte[] ToBytes<T>(this T type)
        {

            int size = Marshal.SizeOf(type);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(type, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        //Has to exactly match or epic failure
        public static object[] ReadFunctionArguments(this BinaryReader reader, MethodInfo funcInf)
        {
            ParameterInfo[] parameters = funcInf.GetParameters();
            object[] readParams = new object[parameters.Length];

            for(int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo param = parameters[i];
                readParams[i] = reader.ReadObjectUnknown(param.ParameterType);
            }

            return readParams;
        }

        public static T ToObject<T>(this byte[] arr) where T : new()
        {
            T obj = new T();

            int size = Marshal.SizeOf(obj);
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(arr, 0, ptr, size);

            obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);

            return obj;
        }

        public static object ToObjectType(this byte[] arr, Type T)
        {
            object obj = Activator.CreateInstance(T);

            int size = Marshal.SizeOf(obj);
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(arr, 0, ptr, size);

            obj = Marshal.PtrToStructure(ptr, T);
            Marshal.FreeHGlobal(ptr);

            return obj;
        }

        public static int SizeOf(this object type)
        {
            return Marshal.SizeOf(type);
        }

        public static string Name(this CSteamID id)
        {
            string name = SteamFriends.GetFriendPersonaName(id);

            if (string.IsNullOrEmpty(name))
                return "(EMPTY NAME)";

            return name;
        }

        #region BinaryWriter

        public static void Write(this BinaryWriter writer, Vector3 pos)
        {
            writer.Write(pos.x);
            writer.Write(pos.y);
            writer.Write(pos.z);
        }

        public static void Write(this BinaryWriter writer, Vector4 pos)
        {
            writer.Write(pos.x);
            writer.Write(pos.y);
            writer.Write(pos.z);
            writer.Write(pos.w);
        }

        public static void Write(this BinaryWriter writer, Quaternion quat)
        {
            writer.Write(quat.x);
            writer.Write(quat.y);
            writer.Write(quat.z);
            writer.Write(quat.w);
        }

        public static void Write(this BinaryWriter writer, MotionBehaviorGroupActID id)
        {
            writer.Write((uint)id.group);
            writer.Write((uint)id.action);
        }

        public static void Write(this BinaryWriter writer, MotionPlayInfo inf)
        {
            writer.Write(inf.jaunt_goal_);
            writer.Write(inf.blend_space_param_);
            writer.Write(inf.behavior_id_);
            writer.Write(inf.behavior_id_next_);
            writer.Write((uint)inf.gmt_id_);
            writer.Write(inf.tick_gmt_now_);
            writer.Write(inf.tick_now_);
            writer.Write(inf.tick_old_);
            writer.Write(inf.tick_blend_);
            writer.Write(inf.tick_key_);
            writer.Write(inf.no_blend_);
            writer.Write(inf.ignore_trans_);
        }

        public static void WriteObject<T>(this BinaryWriter writer, T obj) where T : new()
        {
            writer.Write(obj.ToBytes());
        }

        #endregion



        #region BinaryReader

        public static T ReadObject<T>(this BinaryReader reader) where T : new()
        {
            byte[] buff = reader.ReadBytes(Marshal.SizeOf<T>());

            return buff.ToObject<T>();
        }

        public static object ReadObjectUnknown(this BinaryReader reader, Type type) 
        {
            byte[] buff = reader.ReadBytes(Marshal.SizeOf(type));

            return buff.ToObjectType(type);
        }

        public static Vector3 ReadVector3(this BinaryReader reader)
        {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public static Vector4 ReadVector4(this BinaryReader reader)
        {
            return new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public static Quaternion ReadQuaternion(this BinaryReader reader)
        {
            return new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public static MotionBehaviorGroupActID ReadMotionBehaviorGroupActID(this BinaryReader reader)
        {
            MotionBehaviorGroupActID read = new MotionBehaviorGroupActID();

            read.group = (BehaviorGroupID)reader.ReadUInt32();
            read.action = (BehaviorActionID)reader.ReadUInt32();

            return read;
        }

        public static MotionPlayInfo ReadMotionPlayInfo(this BinaryReader reader)
        {
            MotionPlayInfo read = new MotionPlayInfo();

            read.jaunt_goal_ = reader.ReadVector4();
            read.blend_space_param_ = reader.ReadVector4();
            read.behavior_id_ = reader.ReadMotionBehaviorGroupActID();
            read.behavior_id_next_ = reader.ReadMotionBehaviorGroupActID();
            read.gmt_id_ = (MotionID)reader.ReadUInt32();
            read.tick_gmt_now_ = reader.ReadUInt32();
            read.tick_now_ = reader.ReadUInt32();
            read.tick_old_ = reader.ReadUInt32();
            read.tick_blend_ = reader.ReadUInt32();
            read.tick_key_ = reader.ReadUInt32();
            read.no_blend_ = reader.ReadBoolean();
            read.ignore_trans_ = reader.ReadBoolean();

            return read;
        }

        #endregion
    }
}
