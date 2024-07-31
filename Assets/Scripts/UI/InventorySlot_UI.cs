using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot_UI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemAmount_text;

    public InventorySlot inventorySlot; //value is assigned in UpdateInventroySlotUI

    private UI ui;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
    }

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

        //LCtrl + mouse_left to delete items from inventory
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(inventorySlot.item);
            return;
        }

        if (inventorySlot.item.itemType == ItemType.Equipment)
        {
            Inventory.instance.EquipItem(inventorySlot.item);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inventorySlot == null || inventorySlot.item == null)
        {
            return;
        }

        ui.itemToolTip.ShowToolTip(inventorySlot.item as ItemData_Equipment);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (inventorySlot == null || inventorySlot.item == null)
        {
            return;
        }

        ui.itemToolTip.HideToolTip();
    }
}
