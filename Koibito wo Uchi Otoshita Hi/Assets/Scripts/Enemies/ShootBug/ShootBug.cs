using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBug : Enemy
{
    public BulletSpawner bulletsummoner;
    public ShootBugIdleState idleState { get; private set; }
    public ShootBugMoveState moveState { get; private set; }
    public ShootBugAttackState attackState { get; private set; }
    public EnemyDeathState deathState { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        bulletsummoner = gameObject.AddComponent<BulletSpawner>();
        hearRange = 7.0f;
        detectRange = 12.0f;
        idleState = new ShootBugIdleState(stateMachine, this, this, "Idle");
        moveState = new ShootBugMoveState(stateMachine, this, this, "Move");
        attackState = new ShootBugAttackState(stateMachine, this, this, "Attack");
        deathState = new EnemyDeathState(stateMachine, this, this, "Death");
    }

    protected override void Start()
    {
        base.Start();
        health = 10.0f;
        this.moveSpeed = 1.2f;
        this.attackCoolDown = 5.0f;
        stateMachine.Initialize(idleState);
    }
    protected override void Update()
    {
        base.Update();
        if (stateMachine.currentState != deathState && health <= 0)
        {
            stateMachine.ChangeState(deathState);
        }
    }
}
