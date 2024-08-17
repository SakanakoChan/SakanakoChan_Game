using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Move Info")]
    public float patrolMoveSpeed;
    public float patrolStayTime;

    private float defaultPatrolMoveSpeed;

    [Header("Recon Info")]
    public float playerScanDistance;
    public float playerHearDistance;
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Battle/Aggressive Info")]
    public float battleMoveSpeed;
    public float aggressiveTime;

    private float defaultBattleMoveSpeed;

    [Header("Attack Info")]
    public float attackDistance;
    public float attackCooldown;
    public float minAttackCooldown;
    public float maxAttackCooldown;
    [HideInInspector] public float lastTimeAttacked;

    [Header("Stunned Info")]
    public float stunDuration;
    public Vector2 stunMovement;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterPromptImage;

    public EnemyStateMachine stateMachine { get; private set; }
    protected Player player { get; private set; }
    public EntityFX fx { get; private set; }

    public string lastAnimBoolName { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
        fx = GetComponent<EntityFX>();

        defaultBattleMoveSpeed = battleMoveSpeed;
        defaultPatrolMoveSpeed = patrolMoveSpeed;
    }

    protected override void Start()
    {
        base.Start();

        player = PlayerManager.instance.player;
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDirection, transform.position.y));
    }

    public virtual RaycastHit2D IsPlayerDetected()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, playerScanDistance, whatIsPlayer);
    }

    public void AnimationTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }

    public virtual void FreezeEnemy(bool _freeze)
    {
        if (_freeze)
        {
            battleMoveSpeed = 0;
            patrolMoveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            battleMoveSpeed = defaultBattleMoveSpeed;
            patrolMoveSpeed = defaultPatrolMoveSpeed;
            anim.speed = 1;
        }
    }


    protected virtual IEnumerator FreezeEnemyForTime_Coroutine(float _seconds)
    {
        FreezeEnemy(true);

        yield return new WaitForSeconds(_seconds);

        FreezeEnemy(false);
    }

    public virtual void FreezeEnemyForTime(float _seconds)
    {
        StartCoroutine(FreezeEnemyForTime_Coroutine(_seconds));
    }

    #region Counter Attack
    public void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterPromptImage.SetActive(true);
    }

    public void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterPromptImage.SetActive(false);
    }

    public virtual bool CanBeStunnedByCounterAttack()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }
    #endregion


    public virtual void AssignLastAnimBoolName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }

    public override void SlowSpeedBy(float _percentage, float _duration)
    {
        patrolMoveSpeed = patrolMoveSpeed * (1 - _percentage);
        battleMoveSpeed = battleMoveSpeed * (1 - _percentage);
        anim.speed = anim.speed * (1 - _percentage);

        Invoke("ReturnDefaultSpeed", _duration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        patrolMoveSpeed = defaultPatrolMoveSpeed;
        battleMoveSpeed = defaultBattleMoveSpeed;
    }

    public virtual void GetIntoBattleState()
    {

    }

    public override void DamageFlashEffect()
    {
        fx.StartCoroutine("FlashFX");
    }

}
