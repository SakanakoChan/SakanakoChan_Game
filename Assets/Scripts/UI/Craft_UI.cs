using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Craft_UI : InventorySlot_UI
{
    private void OnEnable()
    {
        UpdateInventorySlotUI(inventorySlot);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemData_Equipment equipmentToCraft = inventorySlot.item as ItemData_Equipment;

        Inventory.instance.CraftIfAvailable(equipmentToCraft, equipmentToCraft.requiredCraftMaterials);
        
    }
}
