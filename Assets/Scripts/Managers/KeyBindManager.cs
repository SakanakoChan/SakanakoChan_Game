using System;
using UnityEngine;

public class KeyBindManager : MonoBehaviour, ISaveManager
{
    public static KeyBindManager instance;

    public SerializableDictionary<string, KeyCode> keybindsDictionary;

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
    }

    private void Start()
    {

    }

    private void Update()
    {
    }

    public void TestInputEnter()
    {
        foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                Debug.Log("The entered key is: " + key.ToString());
            }
        }
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
        keybindsDictionary = _data.keybindsDictionary;
    }

    public void SaveData(ref GameData _data)
    {
        
    }
}
