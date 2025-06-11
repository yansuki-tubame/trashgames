using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBug : Enemy
{
    public ChargeBugMoveState moveState { get; private set; }
    public ChargeBugBattleState battleState { get; private set; }
    public ChargeBugAttackState attackState { get; private set; }
    public EnemyDeathState deathState { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        detectRange = 5.0f;
        moveState = new ChargeBugMoveState(stateMachine,this, this, "Move");
        battleState = new ChargeBugBattleState(stateMachine,this, this, "Battle");
        attackState = new ChargeBugAttackState(stateMachine, this, this, "Attack");
        deathState = new EnemyDeathState(stateMachine, this, this, "Death");
    }
    protected override void Start()
    {
        base.Start();
        health = 10.0f;
        this.moveSpeed = 1.5f;
        this.attackCoolDown = 5.0f;
        stateMachine.Initialize(moveState);
    }
    protected override void Update()
    {
        base.Update();
        if (stateMachine.currentState != deathState&&health<=0)
        {
            stateMachine.ChangeState(deathState);
        }
    }
}
