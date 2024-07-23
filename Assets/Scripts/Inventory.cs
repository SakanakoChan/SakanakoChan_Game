using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<ItemInInventorySlot> itemSlotList;
    public Dictionary<ItemData, ItemInInventorySlot> itemSlotDictionary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        itemSlotList = new List<ItemInInventorySlot>();
        itemSlotDictionary = new Dictionary<ItemData, ItemInInventorySlot>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ItemData _itemToRemove = itemSlotList[itemSlotList.Count - 1].item;
            RemoveItem(_itemToRemove);
        }
    }

    public void AddItem(ItemData _item)
    {
        //if this item is already in inventory, its stack size++
        if(itemSlotDictionary.TryGetValue(_item, out ItemInInventorySlot value))
        {
            value.AddStack();
        }
        else  //if this item is not in inventory, add it to the inventoryItem list
        {
            ItemInInventorySlot newItem = new ItemInInventorySlot(_item);  //initialize the inventroyItem using contructor and make its stackSize++ (=1)
            itemSlotList.Add(newItem);  //add this inventoryItem to the inventory item list
            itemSlotDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if(itemSlotDictionary.TryGetValue(_item, out ItemInInventorySlot value))
        {
            //if there's only 1 or less amount of this item in inventory,
            //remove it from the inventory item list
            if (value.stackSize <= 1)
            {
                itemSlotList.Remove(value);
                itemSlotDictionary.Remove(_item);
            }
            else  //if there're multiple this items in inventory, the stack size--
            {
                value.RemoveStack();
            }
        }
    }
}
