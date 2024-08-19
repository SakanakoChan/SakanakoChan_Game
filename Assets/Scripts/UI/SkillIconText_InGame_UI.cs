using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillIconText_InGame_UI : MonoBehaviour
{
    [SerializeField] private string skillName;

    public void UpdateSkillIconText()
    {
        if (KeyBindManager.instance.keybindsDictionary.TryGetValue(skillName, out KeyCode keybind))
        {
            GetComponent<TextMeshProUGUI>().text = UniformKeybindName(keybind.ToString());
        }
    }

    private string UniformKeybindName(string _behaveKeybind)
    {
        if (_behaveKeybind.StartsWith("Alpha"))
        {
            _behaveKeybind = _behaveKeybind.Remove(0, 5);
        }

        if (_behaveKeybind.Equals("Mouse0"))
        {
            _behaveKeybind = "LMB";
        }

        if (_behaveKeybind.Equals("Mouse1"))
        {
            _behaveKeybind = "RMB";
        }

        if (_behaveKeybind.Equals("Mouse2"))
        {
            _behaveKeybind = "MID";
        }

        if (_behaveKeybind.StartsWith("Left"))
        {
            _behaveKeybind = _behaveKeybind.Remove(1, 3);
        }

        return _behaveKeybind;
    }
}
