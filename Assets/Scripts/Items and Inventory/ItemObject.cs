using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.XR;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData item;

    public void SetupItemDrop(ItemData _item, Vector2 _dropVelocity)
    {
        item = _item;
        rb.velocity = _dropVelocity;

        SetupItemIconAndName();
    }

    //pickup item is called in ItemObject_Trigger
    public void PickupItem()
    {
        if (!Inventory.instance.CanAddEquipmentToInventory() && item.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0, 5);
            return;
        }

        Inventory.instance.AddItem(item);
        Debug.Log($"Picked up item {item.itemName}");
        Destroy(gameObject);
    }
    private void SetupItemIconAndName()
    {
        if (item == null)
        {
            return;
        }

        GetComponent<SpriteRenderer>().sprite = item.icon;
        gameObject.name = $"Item Object - {item.name}";
    }
}
