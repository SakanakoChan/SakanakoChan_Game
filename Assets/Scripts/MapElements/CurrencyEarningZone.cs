using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyEarningZone : MonoBehaviour
{
    private bool hasGivenPlayerCurrency = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasGivenPlayerCurrency)
        {
            if(collision.GetComponent<Player>() != null)
            {
                PlayerManager.instance.currency += 1000;
                hasGivenPlayerCurrency = true;
            }
        }
    }
}
