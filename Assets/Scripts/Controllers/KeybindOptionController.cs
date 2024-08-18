using TMPro;
using UnityEngine;

public class KeybindOptionController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI behaveName;
    [SerializeField] protected TextMeshProUGUI behaveKeybind;

    public void SetupKeybindOption(string _behaveName, string _behaveKeybind)
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

        behaveName.text = _behaveName;
        behaveKeybind.text = _behaveKeybind;
    }
}
