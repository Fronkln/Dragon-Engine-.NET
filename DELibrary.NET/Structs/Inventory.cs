using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class Inventory
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_SDCHUNK_PLAYER_GET_CARRY_ITEM_COUNT", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetCarryItemCount();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_SDCHUNK_PLAYER_GET_BOX_ITEM_COUNT", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetBoxItemCount();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_SDCHUNK_PLAYER_GET_CARRY_ITEM_DATA", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Internal_GetCarryItemData();
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_SDCHUNK_PLAYER_GET_BOX_ITEM_DATA", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Internal_GetBoxItemData();


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_SDCHUNK_PLAYER_SET_CARRY_ITEMS", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Internal_SetCarryItems(IntPtr buf, int length);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_SDCHUNK_PLAYER_SET_BOX_ITEMS", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Internal_SetBoxItems(IntPtr buf, int length);

        public enum StockKind
        {
            Carry,
            Box,
           // All
        }

        public static InventoryItem[] CarryItems
        {
            get
            {
                return Internal_GetCarryItemData().ToTypeArray<InventoryItem>(GetCarryItemCount());
            }
            set
            {
                unsafe
                {
                    fixed (InventoryItem* p = value)
                    {
                        IntPtr ptr = (IntPtr)p;
                        Internal_SetCarryItems(ptr, value.Length);
                    }
                }
            }
        }

        public static InventoryItem[] BoxItems
        {
            get
            {
                return Internal_GetBoxItemData().ToTypeArray<InventoryItem>(GetBoxItemCount());
            }
            set
            {
                unsafe
                {
                    fixed (InventoryItem* p = value)
                    {
                        IntPtr ptr = (IntPtr)p;
                        Internal_SetBoxItems(ptr, value.Length);
                    }
                }
            }
        }


        public static void ClearItems(StockKind kind = StockKind.Carry)
        {
            if (kind == StockKind.Carry)
                CarryItems = new InventoryItem[] { };
            else
                BoxItems = new InventoryItem[] { };
        }
        public static void AddItem(ItemID item, int count, StockKind kind = StockKind.Carry)
        {
            bool carry = kind == StockKind.Carry;

            List<InventoryItem> items = (carry ? CarryItems : BoxItems).ToList();


            int existingIndex = -1;
            InventoryItem existing = new InventoryItem();

            for(int i = 0; i < items.Count; i++)
                if(items[i].ItemID == item)
                {
                    existingIndex = i;
                    existing = items[i];
                    break;
                }

            if (existing.ItemID != 0)
            {
                existing.Count += count;
                items[existingIndex] = existing;
            }
            else
            {
                InventoryItem newItem = new InventoryItem(item, count);
                items.Add(newItem);
            }

            if (carry)
                CarryItems = items.ToArray();
            else
                BoxItems = items.ToArray();
        }

    }
}
