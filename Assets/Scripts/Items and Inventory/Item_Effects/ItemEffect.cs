using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffect(Transform _enemyTransform)
    {
        //Debug.Log("Effect Executed");
    }
    
    //public virtual void ExecuteEffect_NoHitNeeded()
    //{

    //}

    public virtual void ReleaseSwordArcane()
    {

    }

}
