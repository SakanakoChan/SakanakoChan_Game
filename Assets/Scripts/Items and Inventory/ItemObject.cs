using System;
using UnityEngine;


//Remember to use Fill up item data base function in inventorymanager script in unity editor every time making a new item!
public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData item;

    [Header("Item-in-map Info")]
    public bool isItemInMap;
    public int itemInMapID;

    private void Start()
    {
        DestroyPickedUpItemInMap();
    }

    private void DestroyPickedUpItemInMap()
    {
        if (isItemInMap)
        {
            var pickedUpItemList = GameManager.instance.pickedUpItemInMapList;

            foreach (var item in pickedUpItemList)
            {
                if (item.GetComponent<ItemObject>()?.itemInMapID == itemInMapID)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

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
            PlayerManager.instance.player.fx.CreatePopUpText("No more space in inventory!");
            return;
        }

        Inventory.instance.AddItem(item);
        AudioManager.instance.PlaySFX(18, transform);

        GameManager.instance.pickedUpItemInMapList.Add(gameObject);

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
