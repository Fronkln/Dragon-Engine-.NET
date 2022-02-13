using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class EntityBase : CTask
    {

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_RELEASE_ENTITY", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_EntityBase_ReleaseEntity(IntPtr entity, uint level = 0, bool no_sweeper = false);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_GETTER_SCENEROOT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_EntityBase_Getter_SceneRoot(IntPtr entity);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_GET_ORIENT", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DELibrary_EntityBase_Getter_Orient(IntPtr entity, ref Quaternion res);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_SET_ORIENT", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DELibrary_EntityBase_Setter_Orient(IntPtr entity, IntPtr res);

        //[DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_GET_ORIENT", CallingConvention = CallingConvention.Cdecl)]
        // public static extern Vector4 DELibrary_EntityBase_Getter_Orient(IntPtr entity);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_GET_POS_CENTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern Vector4 DELibrary_EntityBase_GetPosCenter(IntPtr entity);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_SET_POS_CENTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_EntityBase_SetPosCenter(IntPtr entity, Vector4 pos);


        /// <summary>
        /// Entity's center position.
        /// </summary>
        public Vector4 PosCenter
        {
            get
            {
                return GetPosCenter();
            }
            set
            {
                SetPosCenter(value);
            }
        }

        public Quaternion Orient
        {
            get
            {
                Quaternion quat = new Quaternion();
                DELibrary_EntityBase_Getter_Orient(Pointer, ref quat);

                return quat;
            }
            set
            {
                IntPtr ptr = value.ToIntPtr();
                DELibrary_EntityBase_Setter_Orient(Pointer, value.ToIntPtr());

                Marshal.FreeHGlobal(ptr);
            }
        }

        /// <summary> Forward direction of this entity.</summary>
        public Vector3 forwardDirection { get { return Orient * Vector3.forward; }}
        /// <summary> Up direction of this entity.</summary>
        public Vector3 upDirection { get { return Orient * Vector3.up; } }
        /// <summary> Forward direction of this entity.</summary>
        public Vector3 rightDirection { get { return Orient * Vector3.right; } }

        /// <summary>
        /// Get the scene entity this entity is part of.
        /// </summary>
        public EntityHandle<SceneBase> Scene
        {
            get
            {
                return DELibrary_EntityBase_Getter_SceneRoot(_objectAddress);
            }
        }

        /// <summary>
        /// Destroy the entity.
        /// </summary>
        public void DestroyEntity(uint level = 0, bool no_sweeper = false)
        {
            DELibrary_EntityBase_ReleaseEntity(_objectAddress, level, no_sweeper);
        }

        /// <summary>
        /// Get the entity's center position.
        /// </summary>
        public Vector4 GetPosCenter()
        {
            return DELibrary_EntityBase_GetPosCenter(_objectAddress);
        }

        /// <summary>
        /// Set entity position.
        /// </summary>
        public void SetPosCenter(Vector4 position)
        {
            DELibrary_EntityBase_SetPosCenter(_objectAddress, position);
        }

        /// <summary>
        /// Returns a scene entity
        /// </summary>
        public virtual EntityHandle<EntityBase> GetSceneEntity(SceneEntity sceneEnt)
        {
            return Scene.Get().GetSceneEntity(sceneEnt);
        }

        /// <summary>
        /// Only use this if you know the type of scene entity you are accessing. Improper use will cause crashes
        /// </summary>
        /// <returns></returns>
        public EntityHandle<T> GetSceneEntity<T>(SceneEntity sceneEnt) where T : CTask, new()
        {
            //Implicit operator automatically converts to handle
            return GetSceneEntity(sceneEnt).UID;
        }
    }
}
