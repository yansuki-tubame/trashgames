using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShootState : EnemyState
{
    protected Boss enemy;
    private Transform player;
    private float angle;
    public BossShootState(EnemyStateMachine _stateMachine, Enemy _enemyBase, Boss _enemy, string _animBoolName) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 4.0f;
        angle = 360.0f;
        enemy.rb.gravityScale = 0;
        player = GameObject.Find("Player").transform;
        enemy.transform.position = new Vector3(player.position.x, player.position.y + 8, 0.0f);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
        enemy.rb.gravityScale = 1;
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(0.0f, 0.0f);
        if (stateTimer * 100 < angle)
        {
            for (int i = 0; i <= 240; i += 120)
            {
                Quaternion quaternion = Quaternion.Euler(0, 0, angle+i);
                Vector3 vec = quaternion * new Vector3(1.0f, 0.0f, 0.0f);
                enemy.bulletsummoner.summonBullet(enemy.transform.position + vec, quaternion, "Prefabs/OrdinaryBullet");
            }
            angle -= 20;
        }
        
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
