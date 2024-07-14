using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;

    protected Rigidbody2D rb;
    protected Animator anim;

    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;


    public EnemyState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        enemyBase = _enemy;
        stateMachine = _stateMachine;
        animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;

        rb = enemyBase.rb;
        anim = enemyBase.anim;

        anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
}
