using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillToolTip_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;

    public void ShowToolTip(string _skillName, string _skillDescription)
    {
        skillName.text = _skillName;
        skillDescription.text = _skillDescription;
        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}
