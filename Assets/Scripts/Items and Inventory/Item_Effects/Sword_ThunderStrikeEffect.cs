using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item Effect/Thunder Strike")]
public class Sword_ThunderStrikeEffect : ItemEffect
{

    [SerializeField] private GameObject thunderStrikePrefab;

    public override void ExecuteEffect(Transform _enemyTransform)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _enemyTransform.position, Quaternion.identity);

        Destroy(newThunderStrike, 1f);
    }
}
