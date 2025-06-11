using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDeathState : EnemyState
{
    private Enemy enemy;
    private GameObject obj;
    public EnemyDeathState(EnemyStateMachine _stateMachine, Enemy _enemyBase, Enemy _enemy, string _animBoolName) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = 1.0f;
        obj = enemy.gameObject;
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0.0f)
        {
            GameObject.Destroy(obj);
            Exit();
        }
    }
}
