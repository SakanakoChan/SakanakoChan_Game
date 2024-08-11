using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;

    [SerializeField] private Checkpoint[] checkpoints;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }

        checkpoints = FindObjectsOfType<Checkpoint>();
    }

    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(scene.name);
    }

    private Checkpoint FindClosestActivatedCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestActivatedCheckpoint = null;

        foreach (var checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(PlayerManager.instance.player.transform.position, checkpoint.transform.position);

            if (distanceToCheckpoint < closestDistance && checkpoint.activated == true)
            {
                closestDistance = distanceToCheckpoint;
                closestActivatedCheckpoint = checkpoint;
            }
        }

        return closestActivatedCheckpoint;
    }

    public void LoadData(GameData _data)
    {
        //activate all the checkpoints which are saved as activated
        foreach (var search in _data.checkpointsDictionary)
        {
            foreach (var checkpoint in checkpoints)
            {
                if (checkpoint.checkpointID == search.Key && search.Value == true)
                {
                    checkpoint.ActivateCheckpoint();
                }
            }
        }

        //player will be spawned at the closest activated checkpoint
        foreach (var checkpoint in checkpoints)
        {
            if (_data.closestActivatedCheckpointID == checkpoint.checkpointID)
            {
                PlayerManager.instance.player.transform.position = checkpoint.transform.position;
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        //prevent from saving duplicated redundant data
        _data.checkpointsDictionary.Clear();

        _data.closestActivatedCheckpointID = FindClosestActivatedCheckpoint()?.checkpointID;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpointsDictionary.Add(checkpoint.checkpointID, checkpoint.activated);
        }
    }
}
