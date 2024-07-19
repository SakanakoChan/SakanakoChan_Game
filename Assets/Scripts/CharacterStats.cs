using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat maxHP;
    public Stat damage;
    public Stat evasion;
    public Stat strength;

    [SerializeField] private int currentHP;

    protected virtual void Start()
    {
        currentHP = maxHP.GetValue();
    }

    public virtual void DoDamge(CharacterStats _targetStats)
    {
        int _totalDamage = damage.GetValue() + strength.GetValue();

        _targetStats.TakeDamage(_totalDamage, transform, _targetStats.transform);
    }

    public virtual void TakeDamage(int _damage, Transform _attacker, Transform _attackee)
    {
        currentHP -= _damage;

        Debug.Log($"{gameObject.name} received {_damage} damage");

        _attackee.GetComponent<Entity>()?.DamageEffect(_attacker, _attackee);

        if(currentHP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} is Dead");
    }


}
