using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Sequential, Pack = 8, Size = 16)]
    public struct InventoryItem
    {
        public ItemID ItemID;
        public int Count;
        public int CountInItemBox;

        public InventoryItem(ItemID id, int itemCount, int itemCountInBox)
        {
            ItemID = id;
            Count = itemCount;
            CountInItemBox = itemCountInBox;
        }

        public InventoryItem(ItemID id, int itemCount)
        {
            ItemID = id;
            Count = itemCount;
            CountInItemBox = 0;
        }

        public InventoryItem(ItemID id)
        {
            ItemID = id;
            Count = 1;
            CountInItemBox = 0;
        }
    }
}
