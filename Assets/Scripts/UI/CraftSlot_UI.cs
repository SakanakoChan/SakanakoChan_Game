using UnityEngine.EventSystems;

public class CraftSlot_UI : InventorySlot_UI
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void SetupCraftSlot(ItemData_Equipment _item)
    {
        if (_item == null)
        {
            return;
        }

        inventorySlot.item = _item;
        itemImage.sprite = _item.icon;
        itemText.text = _item.itemName;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemData_Equipment equipmentToCraft = inventorySlot.item as ItemData_Equipment;

        Inventory.instance.CraftIfAvailable(equipmentToCraft, equipmentToCraft.requiredCraftMaterials);

    }
}
