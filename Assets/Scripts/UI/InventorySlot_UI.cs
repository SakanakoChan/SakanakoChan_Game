using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class InventorySlot_UI : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemAmount_text;

    public InventorySlot inventorySlot;

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
}
