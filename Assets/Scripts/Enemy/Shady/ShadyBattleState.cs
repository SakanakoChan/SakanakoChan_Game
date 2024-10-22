using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShadyBattleState : ShadyState
{
    private Transform player;

    private int moveDirection;

    public ShadyBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //entering battleState will set the default enemy aggressiveTime
        //To prevent the case where if player approaching enemy from behind
        //enemy will get stuck in switching between idleState and battleState
        stateTimer = enemy.aggressiveTime;

        player = PlayerManager.instance.player.transform;

        //if player is attacking enemy from behin,
        //enemy will turn to player's side immediately
        FacePlayer();

        if (player.GetComponent<PlayerStats>().isDead)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
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

        //enemy always faces player in battle state
        //to prevent enemy from getting stuck in edge of ground
        FacePlayer();

        if (enemy.IsPlayerDetected())
        {
            //If enemy can see player, then it's always in aggreesive mode
            stateTimer = enemy.aggressiveTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {

                if (CanAttack())
                {
                    anim.SetBool("Idle", false);
                    anim.SetBool("BattleMove", false);
                    stateMachine.ChangeState(enemy.explosionState);
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

        if (!enemy.IsGroundDetected())
        {
            enemy.SetVelocity(0, rb.velocity.y);
            ChangeToIdleAnimation();
            return;
        }

        enemy.SetVelocity(enemy.battleMoveSpeed * moveDirection, rb.velocity.y);
        ChangeToMoveAnimation();

    }

    private bool CanAttack()
    {
        //skeleton can only attack when it's on ground
        if (Time.time - enemy.lastTimeAttacked >= enemy.attackCooldown && !enemy.isKnockbacked && rb.velocity.y <= 0.1f && rb.velocity.y >= -0.1f)
        {
            //enemy's lastTimeAttacked is set in attackState
            //enemy's attack frequency will be random
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            return true;
        }

        return false;
    }

    private void ChangeToIdleAnimation()
    {
        anim.SetBool("BattleMove", false);
        anim.SetBool("Idle", true);
    }

    private void ChangeToMoveAnimation()
    {
        anim.SetBool("Idle", false);
        anim.SetBool("BattleMove", true);
    }

    private void FacePlayer()
    {
        if (player.transform.position.x < enemy.transform.position.x)
        {
            if (enemy.facingDirection != -1)
            {
                enemy.Flip();
            }
        }

        if (player.transform.position.x > enemy.transform.position.x)
        {
            if (enemy.facingDirection != 1)
            {
                enemy.Flip();
            }
        }
    }
}
