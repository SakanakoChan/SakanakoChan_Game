using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    [Header("Move Info")]
    public float patrolMoveSpeed;
    public float patrolStayTime;
    public float battleMoveSpeed;

    #region States
    public SkeletonIdleState idleState {  get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }
}
