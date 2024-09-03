using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitCheck : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = PlayerManager.instance.player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Pit>())
        {
            player.isFacingPit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<Pit>())
        {
            player.isFacingPit = false;
        }
    }
}
