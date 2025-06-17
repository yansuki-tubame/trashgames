using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDeathState : EnemyState
{
    private Enemy enemy;
    private GameObject obj;
    private Player2 player;
    public EnemyDeathState(EnemyStateMachine _stateMachine, Enemy _enemyBase, Enemy _enemy, string _animBoolName) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.rb.gravityScale = 0;
        obj = enemy.gameObject;
        player = GameObject.Find("Player").GetComponent<Player2>();
        player.currentEnergy = (player.currentEnergy + 0.3f) <= player.maxEnergy ? player.currentEnergy + 0.3f : player.maxEnergy;
    }
    public override void Exit()
    {
        base.Exit();
        GameObject.Destroy(obj);
    }

    public override void Update()
    {
        base.Update();
    }
}
