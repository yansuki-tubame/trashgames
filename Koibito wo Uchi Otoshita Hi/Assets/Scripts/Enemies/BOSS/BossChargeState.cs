using Google.Protobuf.WellKnownTypes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossChargeState : EnemyState
{
    private Boss enemy;
    private Transform player;
    private Vector3 target;
    private bool arrived;
    public BossChargeState(EnemyStateMachine _stateMachine, Enemy _enemyBase, Boss _enemy, string _animBoolName) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.Find("Player").transform;
        stateTimer = 1.5f;
        enemy.rb.gravityScale = 0;
        arrived = false;
        if (enemy.IsPlayerSeen() && (player.position.x - enemy.transform.position.x) * enemy.facingDir < 0)
        {
            enemy.Flip();
        }
        enemy.SetVelocity(0.0f, 40.0f);
    }
    public override void Exit()
    {
        enemy.rb.gravityScale = 1;
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 1.40f&&stateTimer>1.20f)
        {
            target = player.position;
            enemy.SetVelocity(0.0f, 0.0f);
        }
        if(stateTimer<1.2f&&!arrived)
        {
            if (Vector2.Distance(enemy.transform.position, target) < 0.1f)
            {
                arrived = true;
            }
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, target, enemy.chargeSpeed * Time.deltaTime);
        }
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }

    }
}
