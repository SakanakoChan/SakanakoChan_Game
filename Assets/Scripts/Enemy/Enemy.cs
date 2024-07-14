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

    public EnemyStateMachine stateMachine {  get; private set; }
    protected Player player { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
        player = GameObject.Find("Player").GetComponent<Player>();
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

    //protected override IEnumerator HitKnockback()
    //{
    //    knockbackDirection = player.facingDirection;
        
    //    return base.HitKnockback();
    //}
}
