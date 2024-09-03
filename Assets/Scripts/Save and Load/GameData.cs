using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

[System.Serializable]
public class GameData
{
    public int currecny;

    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> equippedEquipmentIDs;

    public SerializableDictionary<string, bool> checkpointsDictionary;
    public string closestActivatedCheckpointID;
    public string lastActivatedCheckpointID;

    [Header("Dropped Currency")]
    public int droppedCurrencyAmount;
    public Vector2 deathPosition;

    [Header("Item in map")]
    public List<GameObject> pickedUpItemInMapList;

    [Header("Audio settings")]
    public SerializableDictionary<string, float> volumeSettingsDictionary;

    [Header("Keybind Settings")]
    public SerializableDictionary<string, KeyCode> keybindsDictionary;
    //public SerializableDictionary<string, KeyCode> keybindsDictionary_Chinese; 

    [Header("Gameplay Settings")]
    public SerializableDictionary<string, bool> gameplayToggleSettingsDictionary;

    [Header("Language Settings")]
    public int localeID; //0 for english, 1 for chinese

    public GameData()
    {
        this.currecny = 0;
        this.droppedCurrencyAmount = 0;
        this.deathPosition = Vector2.zero;

        //setup picked up item in map list, by default it should be empty
        pickedUpItemInMapList = new List<GameObject>();

        //default language is english
        localeID = 0;

        //skillTree<skillName, unlocked>
        skillTree = new SerializableDictionary<string, bool>();
        //inventory<itemID, stackSize>
        inventory = new SerializableDictionary<string, int>();

        equippedEquipmentIDs = new List<string>();

        //checkpoints<checkpointID, activated>
        checkpointsDictionary = new SerializableDictionary<string, bool>();

        closestActivatedCheckpointID = string.Empty;
        lastActivatedCheckpointID = string.Empty;

        //volumeSettingsDictionary<exposedParameter, value>
        volumeSettingsDictionary = new SerializableDictionary<string, float>();

        keybindsDictionary = new SerializableDictionary<string, KeyCode>();
        SetupDefaultKeybinds();

        //keybindsDictionary_Chinese = new SerializableDictionary<string, KeyCode>();
        //SetupDefaultKeybinds_Chinese();

        //gameplay toggle settings
        gameplayToggleSettingsDictionary = new SerializableDictionary<string, bool>();
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
        keybindsDictionary.Add("Character", KeyCode.C);
        keybindsDictionary.Add("Craft", KeyCode.B);
        keybindsDictionary.Add("Skill", KeyCode.K);
    }

    private void SetupDefaultKeybinds_Chinese()
    {
        keybindsDictionary.Add("����", KeyCode.Mouse0);
        keybindsDictionary.Add("��׼", KeyCode.Mouse1);
        keybindsDictionary.Add("Ԫ��ƿ", KeyCode.Alpha1);
        keybindsDictionary.Add("���", KeyCode.LeftShift);
        keybindsDictionary.Add("����", KeyCode.Q);
        keybindsDictionary.Add("ˮ��", KeyCode.F);
        keybindsDictionary.Add("�ڶ�", KeyCode.R);
        keybindsDictionary.Add("��ɫ���", KeyCode.C);
        keybindsDictionary.Add("�������", KeyCode.B);
        keybindsDictionary.Add("�������", KeyCode.K);
    }
}
