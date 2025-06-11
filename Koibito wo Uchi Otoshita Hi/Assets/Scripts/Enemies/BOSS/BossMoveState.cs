using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossMoveState : EnemyState
{
    private Boss enemy;
    private Transform player;
    private int moveDir;
    public BossMoveState(EnemyStateMachine _stateMachine, Enemy _enemyBase, Boss _enemy, string _animBoolName) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.Find("Player").transform;
        stateTimer = 1.0f;
        if ((player.position.x - enemy.transform.position.x) * enemy.facingDir < 0)
        {
            enemy.Flip();
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(enemy.facingDir * enemy.moveSpeed, enemy.rb.velocity.y);
        if (enemy.isWallDetected())
        {
            enemy.Flip();
        }
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
