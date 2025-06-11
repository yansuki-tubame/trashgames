using Google.Protobuf.WellKnownTypes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootBugIdleState : EnemyState
{
    private ShootBug enemy;
    private Transform player;
    public ShootBugIdleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, ShootBug _enemy, string _animBoolName) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.Find("Player").transform;
        stateTimer = 0.35f;
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(0.0f, enemy.rb.velocity.y);
        if (stateTimer<0)
        {
            if (Vector2.Distance(enemy.transform.position, player.transform.position) < 7.0f && enemy.IsPlayerSeen())
            {
                stateMachine.ChangeState(enemy.attackState);
            }
            else
            {
                stateMachine.ChangeState(enemy.moveState);
            }
        }
        
    }
}
