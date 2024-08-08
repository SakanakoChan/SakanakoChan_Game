using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class GameData
{
    public int currecny;

    public SerializableDictionary<string, int> inventory;
    public List<string> equippedEquipmentIDs;

    public GameData()
    {
        this.currecny = 0;
        //inventory<itemID, stackSize>
        inventory = new SerializableDictionary<string, int>();
        equippedEquipmentIDs = new List<string>();
    }
}
