using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedEquipmentSlot_UI : InventorySlot_UI
{
    public EquipmentType equipmentType;

    private void OnValidate()
    {
        gameObject.name = equipmentType.ToString();
    }
}
