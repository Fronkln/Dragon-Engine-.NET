using System;
using System.Runtime.InteropServices;


namespace DragonEngineLibrary
{
    public class AuthNode : EntityBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "CAUTH_NODE_GET_SELF", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_AuthNode_GetSelf(IntPtr node);

        [DllImport("Y7Internal.dll", EntryPoint = "CAUTH_NODE_GET_CHARACTER_BASE", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_AuthNode_GetCharacterBase(IntPtr node);

        [DllImport("Y7Internal.dll", EntryPoint = "CAUTH_NODE_GET_AUTH_NODE", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_AuthNode_GetAuthNode(IntPtr node, int nodeType);

        public EntityHandle<AuthNode> GetSelf()
        {
            return DELib_AuthNode_GetSelf(Pointer);
        }

        public EntityHandle<Character> GetCharacter()
        {
            return DELib_AuthNode_GetCharacterBase(Pointer);
        }

        public AuthNode GetAuthNode(int nodeType)
        {
            return new EntityHandle<AuthNode>(DELib_AuthNode_GetAuthNode(Pointer, nodeType)).Get();
        }
    }
}
