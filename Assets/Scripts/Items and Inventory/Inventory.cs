using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventorySlot> inventorySlotList;
    public Dictionary<ItemData, InventorySlot> inventorySlotDictionary;


    [Header("Inventory UI")]
    [SerializeField] private Transform referenceInventory;
    private InventorySlot_UI[] inventorySlotUI;


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
        inventorySlotUI = referenceInventory.GetComponentsInChildren<InventorySlot_UI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ItemData _itemToRemove = inventorySlotList[inventorySlotList.Count - 1].item;
            RemoveItem(_itemToRemove);
        }
    }

    private void UpdateInventoryUI()
    {
        for (int i = 0; i < inventorySlotList.Count; i++)
        {
            inventorySlotUI[i].UpdateInventorySlotUI(inventorySlotList[i]);
        }
    }

    public void AddItem(ItemData _item)
    {
        //if this item is already in inventory, its stack size++
        if(inventorySlotDictionary.TryGetValue(_item, out InventorySlot value))
        {
            value.AddStack();
        }
        else  //if this item is not in inventory, add it to the inventoryItem list
        {
            InventorySlot newItem = new InventorySlot(_item);  //initialize the inventroyItem using contructor and make its stackSize++ (=1)
            inventorySlotList.Add(newItem);  //add this inventoryItem to the inventory item list
            inventorySlotDictionary.Add(_item, newItem);
        }

        UpdateInventoryUI();
    }

    public void RemoveItem(ItemData _item)
    {
        if(inventorySlotDictionary.TryGetValue(_item, out InventorySlot value))
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

        UpdateInventoryUI();
    }
}
