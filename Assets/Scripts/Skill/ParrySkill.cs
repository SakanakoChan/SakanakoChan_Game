using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
    [Header("Parry Unlock Info")]
    [SerializeField] private SkillTreeSlot_UI parryUnlockButton;
    public bool parryUnlocked;

    [Header("Parry Recover HP/FP Unlock Info")]
    [SerializeField] private SkillTreeSlot_UI parryRecoverUnlockButton;
    public bool parryRecoverUnlocked;
    [Range(0f, 1f)]
    [SerializeField] private float recoverPercentage;

    [Header("Parry With Mirage Unlock Info")]
    [SerializeField] private SkillTreeSlot_UI parryWithMirageUnlockButton;
    public bool parryWithMirageUnlocked;

    protected override void Start()
    {
        base.Start();

        parryUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockParry);
        parryRecoverUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockParryRecover);
        parryWithMirageUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockParryWithMirage);
    }

    public override bool UseSkillIfAvailable()
    {
        return base.UseSkillIfAvailable();
    }

    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked == true)
        {
            parryUnlocked = true;
        }
    }

    private void UnlockParryRecover()
    {
        if (parryRecoverUnlockButton.unlocked == true)
        {
            parryRecoverUnlocked = true;
        }
    }

    private void UnlockParryWithMirage()
    {
        if (parryWithMirageUnlockButton.unlocked == true)
        {
            parryWithMirageUnlocked = true;
        }
    }

    public void RecoverHPFPInSuccessfulParry()
    {
        //only did HP cuz FP is not implemented yet
        if (parryRecoverUnlocked == true)
        {
            int recoverAmount = Mathf.RoundToInt(player.stats.getMaxHP() * recoverPercentage);
            player.stats.IncreaseHPBy(recoverAmount);
        }
    }

    public void MakeMirageInSuccessfulParry(Vector3 _cloneSpawnPosition)
    {
        if (parryWithMirageUnlocked == true)
        {
            SkillManager.instance.clone.CreateCloneWithDelay(_cloneSpawnPosition, 0.1f);
        }
    }
}
