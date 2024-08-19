using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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


        if (Input.GetKeyDown(/*KeyCode.Mouse0*/ KeyBindManager.instance.keybindsDictionary["Attack"]))
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }

        if (Input.GetKeyDown(/*KeyCode.Q*/ KeyBindManager.instance.keybindsDictionary["Parry"]) && player.skill.parry.parryUnlocked && player.skill.parry.SkillIsReadyToUse())
        {
            //stateMachine.ChangeState(player.counterAttackState);
            SkillManager.instance.parry.UseSkillIfAvailable();
        }

        if (Input.GetKeyDown(/*KeyCode.R*/ KeyBindManager.instance.keybindsDictionary["Blackhole"]) && player.skill.blackholeSkill.blackholeUnlocked && player.skill.blackholeSkill.SkillIsReadyToUse())
        {
            stateMachine.ChangeState(player.blackholeSkillState);
        }

        //!player.sword   is same as   player.sword == null
        //HasNoSword() will return true if there's no sword in player
        //or return falase and return the sword if player has sword
        if (Input.GetKeyDown(/*KeyCode.Mouse1*/ KeyBindManager.instance.keybindsDictionary["Aim"]) && player.HasNoSword() && player.skill.sword.throwSwordSkillUnlocked)
        {
            stateMachine.ChangeState(player.aimSwordState);
        }

        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }
}
