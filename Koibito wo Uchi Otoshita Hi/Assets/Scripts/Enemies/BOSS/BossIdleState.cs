using Google.Protobuf.WellKnownTypes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BossIdleState : EnemyState
{
    private Boss enemy;
    private Transform player;
    public BossIdleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, Boss _enemy, string _animBoolName) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.Find("Player").transform;
        stateTimer = 0.2f;
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(0.0f, enemy.rb.velocity.y);
        if (stateTimer < 0)
        {
            if (Vector2.Distance(enemy.transform.position, player.transform.position) < enemy.detectRange && enemy.IsPlayerSeen())
            {
                int rand = Random.Range(0, 21);
                if (0 <= rand && rand <= 4)
                {
                    stateMachine.ChangeState(enemy.chargeState);
                }
                else if (5 <= rand && rand <= 6)
                {
                    stateMachine.ChangeState(enemy.shootState);
                }
                else if (7 <= rand && rand <= 14)
                {
                    stateMachine.ChangeState(enemy.missleState);
                }
                else
                {
                    stateMachine.ChangeState(enemy.tornadoState);
                }
            }
            else
            {
                stateMachine.ChangeState(enemy.moveState);
            }
        }

    }
}
