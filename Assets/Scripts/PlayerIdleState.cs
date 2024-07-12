using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(0, rb.velocity.y);

        if (!(xInput == 0 || (player.IsWallDetected() && xInput == player.facingDirection)))
        {
            stateMachine.ChangeState(player.moveState);
        }

        //if (xInput != 0)
        //{
        //    stateMachine.ChangeState(player.moveState);
        //}

    }
}
