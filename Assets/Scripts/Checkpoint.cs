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
        }
    }

    public void ActivateCheckpoint()
    {
        anim.SetBool("Active", true);
        activated = true;
        //SaveManager.instance.SaveGame();
    }

    [ContextMenu("Generate checkpoint ID")]
    private void GenerateCheckpointID()
    {
        checkpointID = System.Guid.NewGuid().ToString();
    }
}
