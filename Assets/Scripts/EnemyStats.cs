using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    protected override void Start()
    {
        base.Start();

        enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(int _damage, Transform _attacker, Transform _attackee)
    {
        base.TakeDamage(_damage, _attacker, _attackee);
    }

    protected override void Die()
    {
        base.Die();

        enemy.Die();
    }
}
