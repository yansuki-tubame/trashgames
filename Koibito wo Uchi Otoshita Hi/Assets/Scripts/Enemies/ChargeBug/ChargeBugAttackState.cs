using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBugAttackState : EnemyState
{
    protected ChargeBug enemy;
    private GameObject player;
   
    public ChargeBugAttackState(EnemyStateMachine _stateMachine, Enemy _enemyBase, ChargeBug _enemy, string _animBoolName) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.Find("Player");
        if (Vector2.Distance(player.transform.position, enemy.transform.position) < 1.0f)
        {
            player.GetComponent<Player2>().TakeDamage(20.0f);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(0f, 0f);
        if (triggerCalled)
        {
            triggerCalled = false;
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
