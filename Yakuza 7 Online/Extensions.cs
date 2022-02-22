using System;
using System.Runtime.InteropServices;
using System.IO;
using DragonEngineLibrary;
using Steamworks;

namespace Y7MP
{
    public static class Extensions
    {
        public static int SizeOf(this object type)
        {
            return Marshal.SizeOf(type);
        }

        public static string Name(this CSteamID id)
        {
            return SteamFriends.GetFriendPersonaName(id);
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

        #endregion



        #region BinaryReader

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

        #endregion
    }
}
