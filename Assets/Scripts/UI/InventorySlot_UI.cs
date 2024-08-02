using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot_UI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    public InventorySlot inventorySlot; //value is assigned in UpdateInventroySlotUI

    protected UI ui;

    protected virtual void Awake()
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
                itemText.text = inventorySlot.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void CleanUpInventorySlotUI()
    {
        inventorySlot = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;

        itemText.text = null;
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

        ui.itemToolTip.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inventorySlot == null || inventorySlot.item == null)
        {
            return;
        }

        //Vector2 mousePosition = Input.mousePosition;
        //ui.itemToolTip.transform.position = new Vector2(mousePosition.x - 175, mousePosition.y + 175);

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
