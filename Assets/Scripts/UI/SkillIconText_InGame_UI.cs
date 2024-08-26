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
            GetComponent<TextMeshProUGUI>().text = KeyBindManager.instance.UniformKeybindName(keybind.ToString());

            //chinese needs to decrease font size a bit
            if (LanguageManager.instance.localeID == 1)
            {
                GetComponent<TextMeshProUGUI>().fontSize = 22;
            }
            //english
            else if (LanguageManager.instance.localeID == 0)
            {
                GetComponent<TextMeshProUGUI>().fontSize = 26;
            }
        }
    }
}
