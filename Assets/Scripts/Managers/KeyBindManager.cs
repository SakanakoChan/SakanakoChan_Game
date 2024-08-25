using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindManager : MonoBehaviour, ISaveManager
{
    public static KeyBindManager instance;

    public Dictionary<string, KeyCode> keybindsDictionary;

    [SerializeField] private KeybindList_UI keybindList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        keybindsDictionary = new Dictionary<string, KeyCode>();
    }

    private void Update()
    {

    }
    
    public void UpdateKeybindListLanguage()
    {
        keybindList.UpdateAllKeybindOptionsLanguage();
    }

    //public string GetKeyCodeByBehaveName(string _behaveName)
    //{
    //    if (keybindsDictionary.TryGetValue(_behaveName, out KeyCode key))
    //    {
    //        return $"The keybind for this behave is: {key.ToString()}";
    //    }
    //    return "Didn't find keybind for this behave";
    //}

    //public void GetAllKeyBinds()
    //{
    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        KeyBind keybind = transform.GetChild(i).GetComponent<KeyBind>();

    //        if (keybind != null)
    //        {
    //            keyBindDictionary.Add(keybind.behaveName, keybind.behaveKeyBind);
    //        }
    //    }
    //}

    public void LoadData(GameData _data)
    {
        foreach (var search in _data.keybindsDictionary)
        {
            keybindsDictionary.Add(search.Key, search.Value);
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.keybindsDictionary.Clear();

        foreach (var search in keybindsDictionary)
        {
            _data.keybindsDictionary.Add(search.Key, search.Value);
        }
    }
}
