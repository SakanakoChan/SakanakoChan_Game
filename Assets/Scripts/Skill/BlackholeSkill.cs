using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkill : Skill
{
    [SerializeField] private GameObject blackholePrefab;
    [Space]
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [Space]
    [SerializeField] private int cloneAttackAmount;
    [SerializeField] private float cloneAttackCooldown;
    [Space]
    [SerializeField] private float QTEInputWindow;

    private GameObject currentBlackhole;
    private BlackholeSkillController currentBlackholeScript;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        currentBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity); ;

        currentBlackholeScript = currentBlackhole.GetComponent<BlackholeSkillController>();

        currentBlackholeScript.SetupBlackholeSkill(maxSize, growSpeed, shrinkSpeed, cloneAttackAmount, cloneAttackCooldown, QTEInputWindow);
    }

    public override bool UseSkillIfAvailable()
    {
        return base.UseSkillIfAvailable();
    }

    public bool CanExitBlackholeSkill()
    {
        if (currentBlackholeScript == null)
        {
            return false;
        }

        if (currentBlackholeScript.CloneAttackHasFinished())
        {
            currentBlackholeScript = null;
            return true;
        }

        return false;
    }

}