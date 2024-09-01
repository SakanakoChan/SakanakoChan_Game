using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerTeleportState : DeathBringerState
{
    public DeathBringerTeleportState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("Teleport!");
        enemy.FindTeleportPosition();
        stateTimer = 1;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.currentState != this)
        {
            return;
        }

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
