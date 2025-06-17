using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    private bool jumped = false;
    public PlayerAirState(PlayerStateMachine _stateMachine, Player2 _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
        player = _player;
    }
    public override void Enter()
    {
        base.Enter();
        jumped = false;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            player.SetVelocity(player.moveSpeed * player.facingDir, player.rb.velocity.y);
        }
        if (player.isGroundDetected())
        {
            stateMachine.ChangeState(player.moveState);
        }
        if (Input.GetKeyDown(KeyCode.Space)&&!jumped)
        {
            jumped = true;
            player.SetVelocity(player.rb.velocity.x, player.jumpSpeed);
        }
        if (Input.GetKeyDown(KeyCode.F) && player.dashCooldown <= 0)
        {
            stateMachine.ChangeState(player.dashState);
        }
    }
}
