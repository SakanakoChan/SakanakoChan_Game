using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMoveState : SlimeGroundedState
{
    public SlimeMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Slime _slime) : base(_enemyBase, _stateMachine, _animBoolName, _slime)
    {
    }

    public override void Exit()
    {
        base.Exit();
        //AudioManager.instance.StopSFX(24);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.stateMachine.currentState != this)
        {
            return;
        }

        //AudioManager.instance.PlaySFX(24, enemy.transform);
        //AudioManager.instance.PlaySFX(14, enemy.transform);

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        enemy.SetVelocity(enemy.patrolMoveSpeed * enemy.facingDirection, rb.velocity.y);
    }

}
