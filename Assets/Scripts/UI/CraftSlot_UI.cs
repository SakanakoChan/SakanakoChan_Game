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

        if (itemText.text.Length > 12)
        {
            itemText.fontSize = itemText.fontSize * 0.8f;
        }
        else
        {
            itemText.fontSize = 24;
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.craftWindow.SetupCraftWindow(inventorySlot.item as ItemData_Equipment);

    }
}
