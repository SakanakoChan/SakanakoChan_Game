using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;

    private Player player;

    [SerializeField] private Checkpoint[] checkpoints;

    [Header("Dropped Currency")]
    [SerializeField] private GameObject deathBodyPrefab;
    public int droppedCurrencyAmount;
    [SerializeField] private Vector2 deathPosition;

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
        player = PlayerManager.instance.player;
    }

    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(scene.name);
    }

    public void PauseGame(bool _pause)
    {
        if (_pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private Checkpoint FindClosestActivatedCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestActivatedCheckpoint = null;

        foreach (var checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(player.transform.position, checkpoint.transform.position);

            if (distanceToCheckpoint < closestDistance && checkpoint.activated == true)
            {
                closestDistance = distanceToCheckpoint;
                closestActivatedCheckpoint = checkpoint;
            }
        }

        return closestActivatedCheckpoint;
    }

    private void LoadDroppedCurrency(GameData _data)
    {
        droppedCurrencyAmount = _data.droppedCurrencyAmount;
        deathPosition = _data.deathPosition;

        if (droppedCurrencyAmount > 0)
        {
            GameObject deathBody = Instantiate(deathBodyPrefab, deathPosition, Quaternion.identity);
            deathBody.GetComponent<DroppedCurrencyController>().droppedCurrency = droppedCurrencyAmount;
        }

        //to prevent from generating a death body on ground
        //when player is not dead in the last life with some currency left
        //and chose to save and continue game;
        droppedCurrencyAmount = 0;
    }

    private void LoadCheckpoints(GameData _data)
    {
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
    }

    private void SpawnPlayerAtClosestActivatedCheckpoint(GameData _data)
    {
        if (_data.closestActivatedCheckpointID == null)
        {
            return;
        }

        foreach (var checkpoint in checkpoints)
        {
            if (_data.closestActivatedCheckpointID == checkpoint.checkpointID)
            {
                player.transform.position = checkpoint.transform.position;
            }
        }
    }

    public void LoadData(GameData _data)
    {
        LoadDroppedCurrency(_data);

        //activate all the checkpoints which are saved as activated
        LoadCheckpoints(_data);

        //player will be spawned at the closest activated checkpoint
        SpawnPlayerAtClosestActivatedCheckpoint(_data);
    }

    public void SaveData(ref GameData _data)
    {
        //saving death position and dropped currency
        _data.droppedCurrencyAmount = droppedCurrencyAmount;
        _data.deathPosition = player.transform.position;


        //prevent from saving duplicated redundant checkpoint data
        _data.checkpointsDictionary.Clear();

        _data.closestActivatedCheckpointID = FindClosestActivatedCheckpoint()?.checkpointID;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpointsDictionary.Add(checkpoint.checkpointID, checkpoint.activated);
        }
    }
}
