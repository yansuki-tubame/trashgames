using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChargeBugBattleState : EnemyState
{
    private Transform player;
    private ChargeBug enemy;
    public ChargeBugBattleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, ChargeBug _enemy, string _animBoolName) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        player = GameObject.Find("Player").transform;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        stateTimer = enemy.battleTime;
        if (Vector2.Distance(enemy.transform.position, player.transform.position) < 1.0f)
        {
            stateMachine.ChangeState(enemy.attackState);
        }
        if (enemy.isWallDetected() || Vector2.Distance(enemy.transform.position, player.transform.position) > 20)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
        /*if (player.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }
        else if (player.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }*/
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir * 8.0f, enemy.rb.velocity.y);
    }
}
