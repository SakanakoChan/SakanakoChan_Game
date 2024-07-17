using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.patrolStayTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //if skeleton is already not in idleState, it's not gonna execute the remaining code
        if (stateMachine.currentState != enemy.idleState)
        {
            return;
        }

        enemy.SetVelocity(0, rb.velocity.y);

        if (stateTimer < 0)
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.moveState);
        }
    }
}
