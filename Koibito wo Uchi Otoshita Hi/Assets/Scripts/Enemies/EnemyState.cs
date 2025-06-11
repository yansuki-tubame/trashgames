//EnemyState:敌人状态基类
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemy;
    protected Rigidbody2D rb;

    private string animBoolName;
    public float duration;
    protected float stateTimer;
    protected bool triggerCalled;

    //构造函数
    public EnemyState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.enemy = _enemyBase;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        enemy.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        enemy.anim.SetBool(animBoolName, false);
    }
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}

