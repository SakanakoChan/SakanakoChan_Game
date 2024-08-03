using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeSlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string skillName;
    [SerializeField] private int skillPrice;
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
        GetComponent<Button>().onClick.AddListener(() => UnlockSkill());
    }

    private void Start()
    {
        skillImage.color = lockedSkillColor;
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

        if (PlayerManager.instance.BuyIfAvailable(skillPrice) == false)
        {
            return;
        }

        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector2 offset = ui.SetupToolTipPositionOffsetAccordingToUISlotPosition(transform, 0.15f, 0.15f, 0.15f, 0.15f);

        ui.skillToolTip.transform.position = new Vector2(transform.position.x + offset.x, transform.position.y + offset.y);
        ui.skillToolTip.ShowToolTip(skillName, skillDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }
}
