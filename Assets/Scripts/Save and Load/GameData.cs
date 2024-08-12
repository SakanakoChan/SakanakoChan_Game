using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class GameData
{
    public int currecny;

    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> equippedEquipmentIDs;

    public SerializableDictionary<string, bool> checkpointsDictionary;
    public string closestActivatedCheckpointID;


    [Header("Dropped Currency")]
    public int droppedCurrencyAmount;
    public Vector2 deathPosition;

    [Header("Audio settings")]
    public SerializableDictionary<string, float> volumeSettingsDictionary;

    public GameData()
    {
        this.currecny = 0;
        this.droppedCurrencyAmount = 0;
        this.deathPosition = Vector2.zero;

        //skillTree<skillName, unlocked>
        skillTree = new SerializableDictionary<string, bool>();
        //inventory<itemID, stackSize>
        inventory = new SerializableDictionary<string, int>();

        equippedEquipmentIDs = new List<string>();

        //checkpoints<checkpointID, activated>
        checkpointsDictionary = new SerializableDictionary<string, bool>();

        closestActivatedCheckpointID = string.Empty;

        //volumeSettingsDictionary<exposedParameter, value>
        volumeSettingsDictionary = new SerializableDictionary<string, float>();
    }
}
