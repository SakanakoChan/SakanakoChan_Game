using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoundZone : MonoBehaviour
{
    private bool hasDamagedPlayer = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasDamagedPlayer)
        {
            if (collision.GetComponent<Player>() != null)
            {
                collision.GetComponent<PlayerStats>()?.TakeDamage(30, transform, collision.transform, false);
                hasDamagedPlayer = true;
            }
        }
    }
}
