using System.Collections;
using System.Collections.Generic;
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
        base.TakeDamage(_damage, _attacker, _attackee);
    }

    protected override void Die()
    {
        base.Die();

        player.Die();

        GetComponent<PlayerItemDrop>()?.GenrateDrop();
    }

    public override void DecreaseHPBy(int _takenDamage)
    {
        base.DecreaseHPBy(_takenDamage);

        ItemData_Equipment currentArmor = Inventory.instance.GetEquippedEquipmentByType(EquipmentType.Armor);

        if(currentArmor != null)
        {
            //currentArmor.ExecuteItemEffect(player.transform);
            Inventory.instance.UseArmorEffect_ConsiderCooldown(player.transform);
        }
    }
}
