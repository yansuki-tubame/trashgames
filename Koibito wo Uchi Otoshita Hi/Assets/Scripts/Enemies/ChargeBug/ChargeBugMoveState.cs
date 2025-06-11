using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChargeBugMoveState : EnemyState
{
    private ChargeBug enemy;
    private Transform player;
    public ChargeBugMoveState(EnemyStateMachine _stateMachine, Enemy _enemyBase, ChargeBug _enemy, string _animBoolName) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        enemy = _enemy;
    }
    
    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, enemy.rb.velocity.y);
        if (enemy.IsPlayerSeen())
        {
            stateMachine.ChangeState(enemy.battleState);
        }
        if (enemy.isWallDetected())
        {
            enemy.Flip();
        }
    }
}
