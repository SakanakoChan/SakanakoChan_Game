using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPanel_InGame_UI : MonoBehaviour
{
    public static SkillPanel_InGame_UI instance;

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
        UpdateAllSkillIconTexts();
    }

    public void UpdateAllSkillIconTexts()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponentInChildren<SkillIconText_InGame_UI>()?.UpdateSkillIconText();
        }
    }
}
