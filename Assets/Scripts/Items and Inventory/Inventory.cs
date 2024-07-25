using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    //inventory stores equipments
    public List<InventorySlot> inventorySlotList;
    public Dictionary<ItemData, InventorySlot> inventorySlotDictionary;

    //stash stores materials
    public List<InventorySlot> stashSlotList;
    public Dictionary<ItemData, InventorySlot> stashSlotDictionary;

    //equipped equipments
    public List<InventorySlot> equippedEquipmentSlotList;
    public Dictionary<ItemData_Equipment, InventorySlot> equippedEquipmentSlotDictionary;


    [Header("Inventory UI")]
    [SerializeField] private Transform referenceInventory;
    [SerializeField] private Transform referenceStash;
    [SerializeField] private Transform referenceEquippedEquipments;

    private InventorySlot_UI[] inventorySlotUI;
    private InventorySlot_UI[] stashSlotUI;
    private EquippedEquipmentSlot_UI[] equippedEquipmentSlotUI;


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

        equippedEquipmentSlotList = new List<InventorySlot>();
        equippedEquipmentSlotDictionary = new Dictionary<ItemData_Equipment, InventorySlot>();

        inventorySlotUI = referenceInventory.GetComponentsInChildren<InventorySlot_UI>();
        stashSlotUI = referenceStash.GetComponentsInChildren<InventorySlot_UI>();
        equippedEquipmentSlotUI = referenceEquippedEquipments.GetComponentsInChildren<EquippedEquipmentSlot_UI>();
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
        //show equipped equipments UI
        for (int i = 0; i < equippedEquipmentSlotUI.Length; i++)
        {
            //if in the equipped equipment list there's an equipment
            //whose type is same as this UI slot equipment type (e.g. the equipped equipment is a weapon, and this is a weapon slot UI)
            //update this UI slot according to that equipment
            foreach (var search in equippedEquipmentSlotDictionary)
            {
                if (search.Key.equipmentType == equippedEquipmentSlotUI[i].equipmentType)
                {
                    equippedEquipmentSlotUI[i].UpdateInventorySlotUI(search.Value);
                }
            }
        }

        //Clean up all the slot UIs before update
        //to ensure no extra slot UI exists after update
        for (int i = 0; i < inventorySlotUI.Length; i++)
        {
            inventorySlotUI[i].CleanUpInventorySlotUI();
        }

        for (int i = 0; i < stashSlotUI.Length; i++)
        {
            stashSlotUI[i].CleanUpInventorySlotUI();
        }


        for (int i = 0; i < inventorySlotList.Count; i++)
        {
            inventorySlotUI[i].UpdateInventorySlotUI(inventorySlotList[i]);
        }

        for (int i = 0; i < stashSlotList.Count; i++)
        {
            stashSlotUI[i].UpdateInventorySlotUI(stashSlotList[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        //convert ItemData type to ItemData_Equipment type (father class -> child class)
        ItemData_Equipment _itemToEquip = _item as ItemData_Equipment;

        InventorySlot newEquipmentSlot = new InventorySlot(_itemToEquip);

        //if this type of equipment is already equipped,
        //remove the equipped one to equip the new one
        ItemData_Equipment _oldEquippedEquipment = null;

        foreach (var search in equippedEquipmentSlotDictionary)
        {
            if (search.Key.equipmentType == _itemToEquip.equipmentType)
            {
                _oldEquippedEquipment = search.Key;
            }
        }

        if (_oldEquippedEquipment != null)
        {
            UnequipSameTypeEquipment(_oldEquippedEquipment);
            //the unequipped old equipment will get back to inventory
            AddItem(_oldEquippedEquipment);
        }

        equippedEquipmentSlotList.Add(newEquipmentSlot);
        equippedEquipmentSlotDictionary.Add(_itemToEquip, newEquipmentSlot);
        //equipped equipment will be removed from inventory
        RemoveItem(_itemToEquip);
        //UpdateInventoryAndStashUI();
    }

    private void UnequipSameTypeEquipment(ItemData_Equipment _equipmentToRemove)
    {
        if (equippedEquipmentSlotDictionary.TryGetValue(_equipmentToRemove, out InventorySlot value))
        {
            equippedEquipmentSlotList.Remove(value);
            equippedEquipmentSlotDictionary.Remove(_equipmentToRemove);
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
