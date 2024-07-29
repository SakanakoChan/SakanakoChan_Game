using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item Effect")]
public class ItemEffect : ScriptableObject
{
    public virtual void ExecuteAttackEffect_HitNeeded(Transform _enemyTransform)
    {
        //Debug.Log("Effect Executed");
    }
    
    public virtual void ExecuteAttackEffect_NoHitNeeded()
    {

    }

}
