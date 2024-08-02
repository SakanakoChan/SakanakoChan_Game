using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeSlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [Space]
    [SerializeField] private Color lockedSkillColor;

    public bool unlocked;

    [SerializeField] private SkillTreeSlot_UI[] shouldBeUnlocked;
    [SerializeField] private SkillTreeSlot_UI[] shouldBeLocked;

    private Image skillImage;
    private UI ui;

    private void OnValidate()
    {
        gameObject.name = $"SkillTreeSlot_UI - {skillName}";
    }

    private void Awake()
    {
        skillImage = GetComponent<Image>();
        ui = GetComponentInParent<UI>();
    }

    private void Start()
    {
        skillImage.color = lockedSkillColor;

        GetComponent<Button>().onClick.AddListener(() => UnlockSkill());
    }

    public void UnlockSkill()
    {
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                Debug.Log("Prerequisite skill hasn't been unlocked!");
                return;
            }
        }

        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                Debug.Log("Alternative Skill has been unlocked so this skill can't be unlocked");
                return;
            }
        }

        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector2 mousePosition = Input.mousePosition;
        float xOffset = 0;
        float yOffset = 0;

        //if mouse is on the right side of the screen
        if (mousePosition.x > 600)
        {
            xOffset = -150;
        }
        else //if mouse is on the left side of the screen
        {
            xOffset = 150;
        }

        //if mouse is on the upper side of the screen
        if (mousePosition.y > 350)
        {
            yOffset = -125;
        }
        else //if mouse is on the lower side of the screen
        {
            yOffset = 125;
        }

        ui.skillToolTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
        ui.skillToolTip.ShowToolTip(skillName, skillDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }
}
