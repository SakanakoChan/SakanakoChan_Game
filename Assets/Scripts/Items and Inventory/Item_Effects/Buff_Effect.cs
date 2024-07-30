using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    maxHP,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightningDamage
}

[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item Effect/Buff Effect")]
public class Buff_Effect : ItemEffect
{
    [SerializeField] private StatType buffStatType;
    [SerializeField] private int buffValue;
    [SerializeField] private int buffDuration;

    private PlayerStats playerStats;

    //public override void ExecuteEffect_NoHitNeeded()
    //{
    //    playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

    //    playerStats.IncreaseStatByTime(GetBuffStat(buffStatType), buffValue, buffDuration);
    //}

    public override void ExecuteEffect(Transform _enemyTransform)
    {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.IncreaseStatByTime(GetBuffStat(buffStatType), buffValue, buffDuration);
    }


    private Stat GetBuffStat(StatType buffStatType)
    {
        Stat buffStat = null;

        switch (buffStatType)
        {
            case StatType.strength: buffStat = playerStats.strength; break;
            case StatType.agility: buffStat = playerStats.agility; break;
            case StatType.intelligence: buffStat = playerStats.intelligence; break;
            case StatType.vitality: buffStat = playerStats.vitality; break;
            case StatType.damage: buffStat = playerStats.damage; break;
            case StatType.critChance: buffStat = playerStats.critChance; break;
            case StatType.critPower: buffStat = playerStats.critPower; break;
            case StatType.maxHP: buffStat = playerStats.maxHP; break;
            case StatType.armor: buffStat = playerStats.armor; break;
            case StatType.evasion: buffStat = playerStats.evasion; break;
            case StatType.magicResistance: buffStat = playerStats.magicResistance; break;
            case StatType.fireDamage: buffStat = playerStats.fireDamage; break;
            case StatType.iceDamage: buffStat = playerStats.iceDamage; break;
            case StatType.lightningDamage: buffStat = playerStats.lightningDamage; break;
        }

        return buffStat;
    }
}
