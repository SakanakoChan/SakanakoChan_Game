using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private float colorLosingSpeed;
    [Space]
    [SerializeField] private bool canAttack;


    public void CreateClone()
    {
        GameObject newClone = Instantiate(clonePrefab, PlayerManager.instance.player.transform.position, Quaternion.identity); ;
        CloneSkillController newCloneScript = newClone.GetComponent<CloneSkillController>();

        newCloneScript.SetupClone(cloneDuration, colorLosingSpeed, canAttack);
    }
}
