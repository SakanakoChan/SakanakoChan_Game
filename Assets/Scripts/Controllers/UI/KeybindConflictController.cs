using TMPro;
using UnityEngine;

public class KeybindConflictController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI behaveName;
    [SerializeField] protected TextMeshProUGUI behaveKeybind;

    public void SetupKeybindConflict(string _behaveName, string _behaveKeybind)
    {
        behaveName.text = _behaveName;
        behaveKeybind.text = UniformKeybindName(_behaveKeybind);
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

}
