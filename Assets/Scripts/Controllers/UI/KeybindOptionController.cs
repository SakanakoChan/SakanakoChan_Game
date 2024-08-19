using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KeybindOptionController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI behaveName;
    [SerializeField] protected TextMeshProUGUI behaveKeybind;
    [SerializeField] private Button keybindButton;
    [Space]
    [SerializeField] private GameObject keybindConflictPromptWindowPrefab;

    private void Start()
    {
        keybindButton.onClick.AddListener(ChangeKeybind);
    }

    public void SetupKeybindOption(string _behaveName, string _behaveKeybind)
    {
        _behaveKeybind = UniformKeybindName(_behaveKeybind);

        behaveName.text = _behaveName;
        behaveKeybind.text = _behaveKeybind;
    }

    private string UniformKeybindName(string _behaveKeybind)
    {
        if (_behaveKeybind.StartsWith("Alpha"))
        {
            _behaveKeybind = _behaveKeybind.Remove(0, 5);
        }

        if (_behaveKeybind.Equals("Mouse0"))
        {
            _behaveKeybind = "Mouse Left";
        }

        if (_behaveKeybind.Equals("Mouse1"))
        {
            _behaveKeybind = "Mouse Right";
        }

        if (_behaveKeybind.StartsWith("Left"))
        {
            _behaveKeybind = _behaveKeybind.Insert(4, " ");
        }

        return _behaveKeybind;
    }

    public void ChangeKeybind()
    {
        StartCoroutine(ChangeKeybindInput());

        //show awaiting input prompt window UI
        //Debug.Log($"awaiting input for {behaveName.text}");
        //behaveKeybind.text = "Awaiting input: ";

        //get user keycode input and change the keybind text in UI
        //StartCoroutine(CheckInput_Coroutine());

        //update the keybind dictionary
    }

    private IEnumerator ChangeKeybindInput()
    {
        //show awaiting input prompt window UI
        behaveKeybind.text = "Awaiting input";

        yield return new WaitUntil(CheckInput);

        keybindButton.onClick.RemoveAllListeners();
        SkillPanel_InGame_UI.instance.UpdateAllSkillIconTexts();

        yield return new WaitWhile(HasAnyKey);

        keybindButton.interactable = true;
        keybindButton.onClick.AddListener(ChangeKeybind);
    }

    private bool CheckInput()
    {
        keybindButton.interactable = false;

        foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keycode))
            {
                behaveKeybind.text = UniformKeybindName(keycode.ToString());
                Debug.Log($"{behaveName.text} keybind has changed to {keycode.ToString()}");

                KeyBindManager.instance.keybindsDictionary[behaveName.text] = keycode;

                if (HasKeybindConflict(keycode))
                {
                    //parent is Keybind_Options
                    //instantiate keybind conflict prompt window
                    GameObject newKeybindConflictPromptWindow = Instantiate(keybindConflictPromptWindowPrefab, transform.parent.parent.parent.parent);

                    //setup keybind conflict prompt window
                    newKeybindConflictPromptWindow.GetComponent<KeybindConflictPromptWindowController>()?.SetupKeybindConflictPromptWindow(keycode);
                }

                return true;
            }

        }

        //Debug.Log("keybind change failed");
        return false;
    }


    private bool HasAnyKey()
    {
        if (Input.anyKey || Input.anyKeyDown)
        {
            return true;
        }

        return false;
    }

    private bool HasKeybindConflict(KeyCode _keycode)
    {
        foreach (var search in KeyBindManager.instance.keybindsDictionary)
        {
            //will not conflict with itself
            if (search.Key == behaveName.text)
            {
                continue;
            }

            if (search.Value == _keycode)
            {
                return true;
            }
        }

        return false;
    }

}
