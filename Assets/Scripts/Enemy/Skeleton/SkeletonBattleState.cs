using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Enemy_Skeleton enemy;
    private Transform player;

    private int moveDirection;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        //entering battleState will set the default enemy aggressiveTime
        //To prevent the case where if player approaching enemy from behind
        //enemy will get stuck in switching between idleState and battleState
        stateTimer = enemy.aggressiveTime;

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.instance.StopSFX(24);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.stateMachine.currentState != this)
        {
            return;
        }

        AudioManager.instance.PlaySFX(24, enemy.transform);

        //AudioManager.instance.PlaySFX(14, enemy.transform);

        if (enemy.IsPlayerDetected())
        {
            //If enemy can see player, then it's always in aggreesive mode
            stateTimer = enemy.aggressiveTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {

                if (CanAttack())
                {
                    anim.SetBool("Idle", false);
                    anim.SetBool("Move", false);
                    stateMachine.ChangeState(enemy.attackState);
                    return;
                }
            }
        }
        else  //If enemy can't see player, 
        {
            //If enemy can't see player or player is out of enemy's scan range, enemy will switch back to patrol mode
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > enemy.playerScanDistance)
            {
                stateMachine.ChangeState(enemy.idleState);
                return;
            }
        }

        //this will make enemy move towards player only when player is far from enemy's attack distance
        //or player is behind enemy
        //when enemy is close to player it'll be stopped
        if (enemy.IsPlayerDetected() && Vector2.Distance(enemy.transform.position, player.transform.position) < enemy.attackDistance)
        {
            ChangeToIdleAnimation();
            return;
        }

        if (player.position.x > enemy.transform.position.x)
        {
            moveDirection = 1;
        }
        else if (player.position.x < enemy.transform.position.x)
        {
            moveDirection = -1;
        }

        enemy.SetVelocity(enemy.battleMoveSpeed * moveDirection, rb.velocity.y);
        ChangeToMoveAnimation();

    }

    private bool CanAttack()
    {
        if (Time.time - enemy.lastTimeAttacked >= enemy.attackCooldown && !enemy.isKnockbacked)
        {
            //enemy's attack frequency will be random
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            return true;
        }

        return false;
    }

    private void ChangeToIdleAnimation()
    {
        anim.SetBool("Move", false);
        anim.SetBool("Idle", true);
    }

    private void ChangeToMoveAnimation()
    {
        anim.SetBool("Idle", false);
        anim.SetBool("Move", true);
    }
}
