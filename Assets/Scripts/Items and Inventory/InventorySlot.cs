using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemData item;
    public int stackSize;

    public InventorySlot(ItemData _newItemData)
    {
        item = _newItemData;
        AddStack();
    }

    public void AddStack()
    {
        stackSize++;
    }

    public void RemoveStack()
    {
        stackSize--;
    }
}
