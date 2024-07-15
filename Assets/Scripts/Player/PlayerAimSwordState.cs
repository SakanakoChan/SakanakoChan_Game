using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //show aim dots while entering AimSword State
        player.skill.sword.ShowDots(true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            //Not showing dots after exiting AimSwordState
            player.skill.sword.ShowDots(false);
            stateMachine.ChangeState(player.idleState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //in ThrowSword animation will call ThrowSword()
            //which will call CreateSword()
            //which will call ShowDots(false)
            stateMachine.ChangeState(player.throwSwordState);
        }
    }
}
