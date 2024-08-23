using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownablePlatform : MonoBehaviour
{
    Player player;
    Collider2D cd;

    public bool playerIsOnPlatform { get; private set; } = false;

    private void Start()
    {
        player = PlayerManager.instance.player;
        cd = GetComponent<Collider2D>();
    }

    private void Update()
    {
        //if (playerIsOnPlatform)
        //{
        //    if(Input.GetKeyDown(KeyCode.S))
        //    {
        //        StartCoroutine(TurnOffPlatformColliderForTime_Coroutine(0.5f));
        //    }
        //}
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            playerIsOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            playerIsOnPlatform = false;
        }
    }

    public void TurnOffPlatformColliderForTime(float _seconds)
    {
        StartCoroutine(TurnOffPlatformColliderForTime_Coroutine(_seconds));
    }

    private IEnumerator TurnOffPlatformColliderForTime_Coroutine(float _seconds)
    {
        cd.enabled = false;
        yield return new WaitForSeconds(_seconds);
        cd.enabled = true;
    }

}
