using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Replace Clone By Crystal")]
    public bool replaceCloneByCrystal;

    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private float colorLosingSpeed;
    [Space]
    [SerializeField] private bool canAttack;
    [Space]
    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashEnd;
    [SerializeField] private bool createCloneOnCounterAttack;
    [Space]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float duplicatePossibility;
    //prevent creating endless duplicate clones
    public int maxDuplicateCloneAmount;
    [HideInInspector] public int currentDuplicateCloneAmount;

    //prevent creating endless duplicate clones
    public void RefreshCurrentDuplicateCloneAmount()
    {
        currentDuplicateCloneAmount = 0;
    }

    public void CreateClone(Vector3 _position)
    {
        //if replace clone by crstal is enabled,
        //will not create clones anymore
        //**************************************************************************
        //***Cannot enable Replace Clone By Crystal when Clone Mirage is enabled***
        //**************************************************************************
        if (replaceCloneByCrystal)
        {
            if (SkillManager.instance.crystal.spawnClone)
            {
                SkillManager.instance.crystal.spawnClone = false;
                Debug.Log("Clone_Mirage in Crystal Skill is DISABLED" +
                    "\nBecase Replace_Clone_By_Crystal in Clone Skill is ENABLED");
            }

            //prevent creating multiple crystals
            SkillManager.instance.crystal.DestroyCurrentCrystal();

            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        //prevent creating endless duplicate clones
        //or not being able to create duplicate clones
        RefreshCurrentDuplicateCloneAmount();

        GameObject newClone = Instantiate(clonePrefab, _position, Quaternion.identity);
        CloneSkillController newCloneScript = newClone.GetComponent<CloneSkillController>();

        newCloneScript.SetupClone(cloneDuration, colorLosingSpeed, canAttack, FindClosestEnemy(newClone.transform), canDuplicateClone, duplicatePossibility);
    }

    public void CreateDuplicateClone(Vector3 _position)
    {
        GameObject newClone = Instantiate(clonePrefab, _position, Quaternion.identity);
        CloneSkillController newCloneScript = newClone.GetComponent<CloneSkillController>();

        newCloneScript.SetupClone(cloneDuration, colorLosingSpeed, canAttack, FindClosestEnemy(newClone.transform), canDuplicateClone, duplicatePossibility);

        //prevent creating endless duplicate clones
        currentDuplicateCloneAmount++;
    }

    public void CreateCloneOnDashStart(Vector3 _position)
    {
        if (createCloneOnDashStart)
        {
            CreateClone(_position);
        }
    }

    public void CreateCloneOnDashEnd(Vector3 _position)
    {
        if (createCloneOnDashEnd)
        {
            CreateClone(_position);
        }
    }

    public void CreateCloneOnCounterAttack(Vector3 _position, float _delay)
    {
        if (createCloneOnCounterAttack)
        {
            StartCoroutine(CreateCloneWithDelay(_position, _delay));
        }
    }

    private IEnumerator CreateCloneWithDelay(Vector3 _position, float _seconds)
    {
        yield return new WaitForSeconds(_seconds);

        CreateClone(_position);
    }
}
