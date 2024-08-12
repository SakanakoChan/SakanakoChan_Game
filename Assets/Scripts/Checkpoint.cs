using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;

    public string checkpointID;

    public bool activated;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            ActivateCheckpoint();
            SaveManager.instance.SaveGame();
        }
    }

    public void ActivateCheckpoint()
    {
        if (activated)
        {
            return;
        }

        anim.SetBool("Active", true);
        activated = true;
        AudioManager.instance.PlaySFX(5, transform);
    }

    [ContextMenu("Generate checkpoint ID")]
    private void GenerateCheckpointID()
    {
        checkpointID = System.Guid.NewGuid().ToString();
    }
}
