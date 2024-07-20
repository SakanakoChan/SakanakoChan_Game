using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major Stats")]
    public Stat strength;  //damage + 1; crit_power + 1%
    public Stat agility;  //evasion + 1%; crit_chance + 1%
    public Stat intelligence; //magic_damage + 1; magic_resistance + 3
    public Stat vitaliy; //maxHP + 5

    [Header("Defensive Stats")]
    public Stat maxHP;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;  //critPower = 150% by default

    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    [Header("Ailments Info")]
    public bool isIgnited; //does damage over time
    public bool isChilled; //armor - 20%
    public bool isShocked; //accuracy - 20% (enemy evasion + 20%)

    [Space]
    [SerializeField] private int currentHP;

    //see ApplyAilment()
    private float ignitedAilmentTimer;
    private float chilledAilmentTimer;
    private float shockedAilmentTimer;

    private float ignitedDamageCooldown = 0.3f;
    private float ignitedDamageTimer;
    private int igniteDamage; //is set up in DoMagicDamage()

    //Stats calculation:
    //total_evasion = evasion + agility + [attacker shock effect];
    //total_damage = (damage + strength) * [total_crit_power] - target.armor * [target chill effect];
    //total_crit_chance = critChance + agility;
    //total_crit_power = critPower + strength;
    //total_magic_damage = (fireDamage + iceDamage + lightningDamage + intelligence) - (target.magicResistance + 3 * target.intelligence);



    protected virtual void Start()
    {
        currentHP = maxHP.GetValue();
        critPower.SetDefaultValue(150);
    }

    protected virtual void Update()
    {
        ignitedAilmentTimer -= Time.deltaTime;
        chilledAilmentTimer -= Time.deltaTime;
        shockedAilmentTimer -= Time.deltaTime;

        ignitedDamageTimer -= Time.deltaTime;

        if (ignitedAilmentTimer < 0)
        {
            isIgnited = false;
        }

        if (chilledAilmentTimer < 0)
        {
            isChilled = false;
        }

        if (shockedAilmentTimer < 0)
        {
            isShocked = false;
        }


        if (ignitedDamageTimer < 0 && isIgnited)
        {
            Debug.Log($"Take burn damage {igniteDamage}");
            currentHP -= igniteDamage;

            if (currentHP <= 0)
            {
                Die();
            }

            ignitedDamageTimer = ignitedDamageCooldown;
        }

    }

    public virtual void DoDamge(CharacterStats _targetStats)
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

        _totalDamage = CheckTargetArmor(_targetStats, _totalDamage);
        //_targetStats.TakeDamage(_totalDamage, transform, _targetStats.transform);

        DoMagicDamage(_targetStats);
    }

    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int _totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        _totalMagicDamage = CheckTargetMagicResistance(_targetStats, _totalMagicDamage);

        _targetStats.TakeDamage(_totalMagicDamage, transform, _targetStats.transform); ;

        //only if at least 1 of the magic damage is > 0, ailment can be applied
        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
        {
            return;
        }

        //choose the highest magic damage to apply the related ailment
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        //if all the magic damage are the same, randomly pick 1 ailment to apply
        if (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < 0.3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < 0.5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < 0.5f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f));
        }

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);

        //if (_fireDamage == _iceDamage && _fireDamage == _lightningDamage && _iceDamage == _lightningDamage)
        //{
        //    canApplyIgnite = true;
        //}
        //else
        //{
        //    if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) == _fireDamage)
        //    {
        //        canApplyIgnite = true;
        //    }

        //    if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) == _iceDamage)
        //    {
        //        canApplyChill = true;
        //    }

        //    if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) == _lightningDamage)
        //    {
        //        canApplyShock = true;
        //    }
        //}


    }

    private static int CheckTargetMagicResistance(CharacterStats _targetStats, int _totalMagicDamage)
    {
        _totalMagicDamage -= _targetStats.magicResistance.GetValue() + (3 * _targetStats.intelligence.GetValue());

        _totalMagicDamage = Mathf.Clamp(_totalMagicDamage, 0, int.MaxValue);
        return _totalMagicDamage;
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if (isIgnited || isChilled || isShocked)
        {
            return;
        }

        if (_ignite)
        {
            isIgnited = _ignite;
            ignitedAilmentTimer = 2;
        }

        if (_chill)
        {
            isChilled = _chill;
            chilledAilmentTimer = 10;
        }

        if (_shock)
        {
            isShocked = _shock;
            shockedAilmentTimer = 4;
        }

        if (isIgnited)
        {
            Debug.Log($"{gameObject.name} is Ignited");
        }
        else if (isChilled)
        {
            Debug.Log($"{gameObject.name} is Chilled");
        }
        else if (isShocked)
        {
            Debug.Log($"{gameObject.name} is Shocked");
        }
    }

    public void SetupIgniteDamage(int _igniteDamage)
    {
        igniteDamage = _igniteDamage;
    }


    public virtual void TakeDamage(int _damage, Transform _attacker, Transform _attackee)
    {
        currentHP -= _damage;

        Debug.Log($"{gameObject.name} received {_damage} damage");

        _attackee.GetComponent<Entity>()?.DamageEffect(_attacker, _attackee);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} is Dead");
    }


    #region Damage Calculation - Armor and Crit
    private int CheckTargetArmor(CharacterStats _targetStats, int _totalDamage)
    {
        //chill effect: reduce armor by 20%
        if (_targetStats.isChilled)
        {
            _totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);
        }
        else
        {
            _totalDamage -= _targetStats.armor.GetValue();
        }


        //make totalDamge >= 0
        _totalDamage = Mathf.Clamp(_totalDamage, 0, int.MaxValue);
        return _totalDamage;
    }

    private bool TargetCanEvadeThisAttack(CharacterStats _targetStats)
    {
        int _totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
        {
            _totalEvasion += 20;
        }

        if (Random.Range(0, 100) < _totalEvasion)
        {
            Debug.Log("Attack Evaded");
            return true;
        }

        return false;
    }

    private bool CanCrit()
    {
        int _totalCritChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= _totalCritChance)
        {
            return true;
        }

        return false;
    }

    private int CalculatCritDamage(int _damage)
    {
        float _totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;

        float critDamage = _damage * _totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }
    #endregion
}
