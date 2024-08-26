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
        
        //english
        if (LanguageManager.instance.localeID == 0)
        {
            itemText.text = _item.itemName;
        }
        //chinese
        else if (LanguageManager.instance.localeID == 1)
        {
            itemText.text = _item.itemName_Chinese;
        }


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
