using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(PlayerStateMachine _stateMachine, Player2 _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
        player = _player;
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = 0.15f;
        player.SetVelocity(player.facingDir * player.moveSpeed * 4, 0.0f);
        player.rb.gravityScale = 0;
        player.InvincibleTime = 0.3f;
    }
    public override void Exit()
    {
        base.Exit();
        player.rb.gravityScale = 8;
        player.dashCooldown = 0.6f;
        player.SetVelocity(player.facingDir * player.moveSpeed, player.rb.velocity.y);
    }
    public override void Update()
    {
        base.Update();
        if (stateTimer < 0.0f)
        {
            if (player.isGroundDetected())
            {
                stateMachine.ChangeState(player.moveState);
            }
            else
            {
                stateMachine.ChangeState(player.airState);
            }
        }
    }
}
