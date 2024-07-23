using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ItemObject : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private ItemData itemData;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        sr.sprite = itemData.icon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            Debug.Log($"Picked up item {itemData.itemName}");
            Destroy(gameObject);
        }
    }
}
