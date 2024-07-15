using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Move Info")]
    public float patrolMoveSpeed;
    public float patrolStayTime;

    [Header("Recon Info")]
    public float playerScanDistance;
    public float playerHearDistance;
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Battle/Aggressive Info")]
    public float battleMoveSpeed;
    public float aggressiveTime;

    [Header("Attack Info")]
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;

    [Header("Stunned Info")]
    public float stunDuration;
    public Vector2 stunMovement;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterPromptImage;

    public EnemyStateMachine stateMachine {  get; private set; }
    protected Player player { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
        player = PlayerManager.instance.player;
    }

    protected override void Start()
    {
        base.Start();
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

    //protected override IEnumerator HitKnockback()
    //{
    //    knockbackDirection = player.facingDirection;
        
    //    return base.HitKnockback();
    //}
}
