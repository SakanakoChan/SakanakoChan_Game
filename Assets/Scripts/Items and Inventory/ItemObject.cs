using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData item;

    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = item.icon;
        gameObject.name = $"Item Object - {item.name}";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            Inventory.instance.AddItem(item);
            Debug.Log($"Picked up item {item.itemName}");
            Destroy(gameObject);
        }
    }

}
