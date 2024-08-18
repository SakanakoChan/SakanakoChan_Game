using System.Collections.Generic;
using UnityEngine;

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

    [Header("Keybind Settings")]
    public SerializableDictionary<string, KeyCode> keybindsDictionary;

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

        keybindsDictionary = new SerializableDictionary<string, KeyCode>();
        SetupDefaultKeybinds();
    }

    private void SetupDefaultKeybinds()
    {
        keybindsDictionary.Add("Attack", KeyCode.Mouse0);
        keybindsDictionary.Add("Aim", KeyCode.Mouse1);
        keybindsDictionary.Add("Flask", KeyCode.Alpha1);
        keybindsDictionary.Add("Dash", KeyCode.LeftShift);
        keybindsDictionary.Add("Parry", KeyCode.Q);
        keybindsDictionary.Add("Crystal", KeyCode.F);
        keybindsDictionary.Add("Blackhole", KeyCode.R);
    }
}
