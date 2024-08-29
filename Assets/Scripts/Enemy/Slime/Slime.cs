using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slime : Enemy
{
    #region States
    public SlimeIdleState idleState {  get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeStunnedState stunnedState { get; private set; }
    public SlimeDeathState deathState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new SlimeIdleState(this, stateMachine, "Idle", this);
        moveState = new SlimeMoveState(this, stateMachine, "Move", this);
        battleState = new SlimeBattleState(this, stateMachine, "Move", this);
        attackState = new SlimeAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SlimeStunnedState(this, stateMachine, "Stunned", this);
        deathState = new SlimeDeathState(this, stateMachine, "Death", this);

        SetupDefaultFacingDirection(-1);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        //to prevent counter image from always showing when skeleton's attack got interrupted
        if (stateMachine.currentState != attackState)
        {
            CloseCounterAttackWindow();
        }
    }

    public override bool CanBeStunnedByCounterAttack()
    {
        if (base.CanBeStunnedByCounterAttack())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }

        return false;
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deathState);
    }

    public override void GetIntoBattleState()
    {
        //if (stateMachine.currentState == battleState || stateMachine.currentState == attackState)
        //{
        //    return;
        //}

        if (stateMachine.currentState != battleState && stateMachine.currentState != stunnedState && stateMachine.currentState != deathState)
        {
            stateMachine.ChangeState(battleState);
        }
    }
}
