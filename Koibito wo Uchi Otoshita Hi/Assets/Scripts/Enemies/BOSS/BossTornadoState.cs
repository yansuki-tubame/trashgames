using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTornadoState : EnemyState
{
    protected Boss enemy;
    private Transform player;
    
    public BossTornadoState(EnemyStateMachine _stateMachine, Enemy _enemyBase, Boss _enemy, string _animBoolName) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 0.5f;
        enemy.rb.gravityScale = 0;
        player = GameObject.Find("Player").transform;
        enemy.transform.position = new Vector3(player.position.x, player.position.y + 8, 0.0f);
        
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
        enemy.rb.gravityScale = 1;
        Quaternion quaternion = Quaternion.Euler(0, 0, 0);
        enemy.bulletsummoner.summonBullet(enemy.transform.position, quaternion, "Prefabs/TornadoCenter");
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(0.0f, 0.0f);
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
