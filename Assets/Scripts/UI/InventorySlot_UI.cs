using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot_UI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemAmount_text;

    public InventorySlot inventorySlot; //value is assigned in UpdateInventroySlotUI

    public void UpdateInventorySlotUI(InventorySlot _inventorySlot)
    {
        inventorySlot = _inventorySlot;

        itemImage.color = Color.white;

        if (inventorySlot != null)
        {
            itemImage.sprite = inventorySlot.item.icon;

            if (inventorySlot.stackSize > 1)
            {
                itemAmount_text.text = inventorySlot.stackSize.ToString();
            }
            else
            {
                itemAmount_text.text = "";
            }
        }
    }

    public void CleanUpInventorySlotUI()
    {
        inventorySlot = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;

        itemAmount_text.text = null;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        //when clikcing the invisible slot UI,
        //directly return to prevent bugs
        if (inventorySlot == null)
        {
            return;
        }

        if (inventorySlot.item.itemType == ItemType.Equipment)
        {
            Inventory.instance.EquipItem(inventorySlot.item);
        }
    }
}
