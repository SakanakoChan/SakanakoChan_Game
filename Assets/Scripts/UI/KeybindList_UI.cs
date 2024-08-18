using UnityEngine;

public class KeybindList_UI : MonoBehaviour
{
    [SerializeField] private GameObject keybindOptionPrefab;

    private void Start()
    {
        GenerateKeybindList();
    }

    private void GenerateKeybindList()
    {
        foreach (var search in KeyBindManager.instance.keybindsDictionary)
        {
            if (search.Key != null)
            {
                GameObject keybindOption = Instantiate(keybindOptionPrefab, transform);
                keybindOption.GetComponent<KeybindOptionController>().SetupKeybindOption(search.Key, search.Value.ToString());
            }
        }
    }
}
