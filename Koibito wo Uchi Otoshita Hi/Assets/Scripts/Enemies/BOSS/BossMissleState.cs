using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMissleState : EnemyState
{
    protected Boss enemy;
    private Transform player;
    public BossMissleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, Boss _enemy, string _animBoolName) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 1.0f;
        player = GameObject.Find("Player").transform;
        if (enemy.IsPlayerSeen() && (player.position.x - enemy.transform.position.x) * enemy.facingDir < 0)
        {
            enemy.Flip();
        }
        for (int i = 0; i <= 180; i += 36)
        {
            Quaternion quaternion = Quaternion.Euler(0, 0, enemy.facingDir > 0 ? i : 180 - i);
            Vector3 vec = quaternion * new Vector3(0.2f, 0.0f, 0.0f);
            enemy.bulletsummoner.summonBullet(enemy.transform.position + vec, quaternion, "Prefabs/ChaseBullet");
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(0.0f, enemy.rb.velocity.y);
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
