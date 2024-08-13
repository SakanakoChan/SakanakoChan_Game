using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage, Transform _attacker, Transform _attackee)
    {
        int takenDamage = DecreaseHPBy(_damage);

        //Debug.Log($"{gameObject.name} received {_damage} damage");

        _attackee.GetComponent<Entity>()?.DamageFlashEffect();

        //player will get knockbacked when the taken damage is bigger than 30% of maxHP
        if (takenDamage >= player.stats.getMaxHP() * 0.3f)
        {
            _attackee.GetComponent<Entity>()?.DamageKnockbackEffect(_attacker, _attackee);
        }

        if (currentHP <= 0 && !isDead)
        {
            Die();
        }
    }

    protected override void Die()
    {
        base.Die();

        player.Die();

        GameManager.instance.droppedCurrencyAmount = PlayerManager.instance.GetCurrentCurrency();
        PlayerManager.instance.currency = 0;

        GetComponent<PlayerItemDrop>()?.GenrateDrop();
    }

    public override int DecreaseHPBy(int _takenDamage)
    {
        base.DecreaseHPBy(_takenDamage);

        int randomIndex = Random.Range(34, 36);
        AudioManager.instance.PlaySFX(randomIndex, player.transform);

        ItemData_Equipment currentArmor = Inventory.instance.GetEquippedEquipmentByType(EquipmentType.Armor);

        if (currentArmor != null)
        {
            //currentArmor.ExecuteItemEffect(player.transform);
            Inventory.instance.UseArmorEffect_ConsiderCooldown(player.transform);
        }

        return _takenDamage;
    }

    public override void OnEvasion()
    {
        Debug.Log("Player evaded attack!");
        player.skill.dodge.CreateMirageOnDodge();
    }

    public void CloneDoDamage(CharacterStats _targetStats, float _cloneAttackDamageMultipler, Transform _cloneTransform)
    {
        if (TargetCanEvadeThisAttack(_targetStats))
        {
            return;
        }

        int _totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            Debug.Log("Critical Attack!");
            _totalDamage = CalculatCritDamage(_totalDamage);
        }

        //clone attack damage should be less than player's damage
        if (_cloneAttackDamageMultipler > 0)
        {
            _totalDamage = Mathf.RoundToInt(_totalDamage * _cloneAttackDamageMultipler);
        }

        _totalDamage = CheckTargetArmor(_targetStats, _totalDamage);
        _targetStats.TakeDamage(_totalDamage, _cloneTransform, _targetStats.transform);
    }
}
