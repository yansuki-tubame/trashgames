using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public float chargeSpeed;
    public BulletSpawner bulletsummoner;
    public BossIdleState idleState { get; private set; }
    public BossMoveState moveState { get; private set; }
    public BossChargeState chargeState { get; private set; }
    public BossShootState shootState { get; private set; }
    public BossMissleState missleState { get; private set; }
    public BossTornadoState tornadoState { get; private set; }
    public EnemyDeathState deathState { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        bulletsummoner = gameObject.AddComponent<BulletSpawner>();
        hearRange = 25.0f;
        detectRange = 25.0f;
        idleState = new BossIdleState(stateMachine, this, this, "Idle");
        moveState = new BossMoveState(stateMachine, this, this, "Move");
        chargeState = new BossChargeState(stateMachine, this, this, "Charge");
        shootState = new BossShootState(stateMachine, this, this, "Shoot");
        missleState = new BossMissleState(stateMachine, this, this, "Missle");
        tornadoState = new BossTornadoState(stateMachine, this, this, "Tornado");
        deathState = new EnemyDeathState(stateMachine, this, this, "Death");
    }

    protected override void Start()
    {
        base.Start();
        moveSpeed = 5.5f;
        attackCoolDown = 5.0f;
        chargeSpeed = 40.0f;
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
