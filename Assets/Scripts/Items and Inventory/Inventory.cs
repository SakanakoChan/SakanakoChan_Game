using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventorySlot> inventorySlotList;
    public Dictionary<ItemData, InventorySlot> inventorySlotDictionary;

    public List<InventorySlot> stashSlotList;
    public Dictionary<ItemData, InventorySlot> stashSlotDictionary;


    [Header("Inventory UI")]
    [SerializeField] private Transform referenceInventory;
    [SerializeField] private Transform referenceStash;

    private InventorySlot_UI[] inventorySlotUI;
    private InventorySlot_UI[] stashSlotUI;


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
        inventorySlotList = new List<InventorySlot>();
        inventorySlotDictionary = new Dictionary<ItemData, InventorySlot>();

        stashSlotList = new List<InventorySlot>();
        stashSlotDictionary = new Dictionary<ItemData, InventorySlot>();

        inventorySlotUI = referenceInventory.GetComponentsInChildren<InventorySlot_UI>();
        stashSlotUI = referenceStash.GetComponentsInChildren<InventorySlot_UI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ItemData _itemToRemove = inventorySlotList[inventorySlotList.Count - 1].item;
            RemoveItem(_itemToRemove);
        }
    }

    private void UpdateInventoryAndStashUI()
    {
        for (int i = 0; i < inventorySlotList.Count; i++)
        {
            inventorySlotUI[i].UpdateInventorySlotUI(inventorySlotList[i]);
        }

        for (int i = 0; i < stashSlotList.Count; i++)
        {
            stashSlotUI[i].UpdateInventorySlotUI(stashSlotList[i]);
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            AddEquipmentToInventory(_item);
        }
        else if (_item.itemType == ItemType.Material)
        {
            AddMaterialToStash(_item);
        }

        UpdateInventoryAndStashUI();
    }

    public void AddMaterialToStash(ItemData _item)
    {
        if (stashSlotDictionary.TryGetValue(_item, out InventorySlot value))
        {
            value.AddStack();
        }
        else
        {
            InventorySlot newItem = new InventorySlot(_item);
            stashSlotList.Add(newItem);
            stashSlotDictionary.Add(_item, newItem);
        }
    }

    public void AddEquipmentToInventory(ItemData _item)
    {
        //if this item is already in inventory, its stack size++
        if (inventorySlotDictionary.TryGetValue(_item, out InventorySlot value))
        {
            value.AddStack();
        }
        else  //if this item is not in inventory, add it to the inventoryItem list
        {
            InventorySlot newItem = new InventorySlot(_item);  //initialize the inventroyItem using contructor and make its stackSize++ (=1)
            inventorySlotList.Add(newItem);  //add this inventoryItem to the inventory item list
            inventorySlotDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventorySlotDictionary.TryGetValue(_item, out InventorySlot value))
        {
            //if there's only 1 or less amount of this item in inventory,
            //remove it from the inventory item list
            if (value.stackSize <= 1)
            {
                inventorySlotList.Remove(value);
                inventorySlotDictionary.Remove(_item);
            }
            else  //if there're multiple this items in inventory, the stack size--
            {
                value.RemoveStack();
            }
        }

        //For removing materials from stash
        if (stashSlotDictionary.TryGetValue(_item, out InventorySlot stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stashSlotList.Remove(stashValue);
                stashSlotDictionary.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }

        }


        UpdateInventoryAndStashUI();
    }
}
