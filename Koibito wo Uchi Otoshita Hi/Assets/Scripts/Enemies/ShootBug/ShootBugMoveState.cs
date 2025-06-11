using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootBugMoveState : EnemyState
{
    private ShootBug enemy;
    private Transform player;
    private int moveDir;
    public ShootBugMoveState(EnemyStateMachine _stateMachine, Enemy _enemyBase, ShootBug _enemy, string _animBoolName) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.Find("Player").transform;
        stateTimer = 1.0f;
        if (enemy.IsPlayerSeen() && (player.position.x - enemy.transform.position.x) * enemy.facingDir < 0)
        {
            enemy.Flip();
        }
        if (enemy.isWallDetected())
        {
            enemy.Flip();
        }
        enemy.SetVelocity(enemy.facingDir * enemy.moveSpeed * 3, 25.0f);
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer <0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
