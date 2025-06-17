using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player2 player;
    protected Rigidbody2D rb;

    private string animBoolName;
    public float duration;
    protected float stateTimer;
    protected bool triggerCalled;

    //¹¹Ôìº¯Êý
    public PlayerState(PlayerStateMachine _stateMachine, Player2 _enemyBase, string _animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.player = _enemyBase;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        player.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }
}
