using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop itemDropSystem;

    [Header("Enemy Level Info")]
    [SerializeField] private int enemyLevel = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = 0.4f;

    protected override void Start()
    {
        ModifyAllStatsAccordingToEnemyLevel();

        base.Start();

        enemy = GetComponent<Enemy>();
        itemDropSystem = GetComponent<ItemDrop>();

    }


    public override void TakeDamage(int _damage, Transform _attacker, Transform _attackee)
    {
        base.TakeDamage(_damage, _attacker, _attackee);
    }

    protected override void Die()
    {
        base.Die();

        enemy.Die();

        //enemy will drop items when dying
        itemDropSystem.GenrateDrop();
    }

    private void ModifyAllStatsAccordingToEnemyLevel()
    {
        ModifyStatAccordingToEnemyLevel(strength);
        ModifyStatAccordingToEnemyLevel(agility);
        ModifyStatAccordingToEnemyLevel(intelligence);
        ModifyStatAccordingToEnemyLevel(vitality);

        ModifyStatAccordingToEnemyLevel(damage);
        ModifyStatAccordingToEnemyLevel(critChance);
        ModifyStatAccordingToEnemyLevel(critPower);

        ModifyStatAccordingToEnemyLevel(maxHP);
        ModifyStatAccordingToEnemyLevel(armor);
        ModifyStatAccordingToEnemyLevel(evasion);
        ModifyStatAccordingToEnemyLevel(magicResistance);

        ModifyStatAccordingToEnemyLevel(fireDamage);
        ModifyStatAccordingToEnemyLevel(iceDamage);
        ModifyStatAccordingToEnemyLevel(lightningDamage);
    }

    private void ModifyStatAccordingToEnemyLevel(Stat _stat)
    {
        //when enemyLevel > 1, increase enemy's stats
        for (int i = 1; i < enemyLevel; i++)
        {
            float _modifier = _stat.GetValue() * percentageModifier;
            _stat.AddModifier(Mathf.RoundToInt(_modifier));
        }
    }
}
