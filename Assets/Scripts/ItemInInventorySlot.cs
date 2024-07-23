using System;
using UnityEngine;

[Serializable]
public class ItemInInventorySlot
{
    public ItemData item;
    public int stackSize;

    public ItemInInventorySlot(ItemData _newItemData)
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
