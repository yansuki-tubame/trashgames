using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSneakState : PlayerState
{
    public PlayerSneakState(PlayerStateMachine _stateMachine, Player2 _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
        player = _player;
    }
    public override void Enter()
    {
        base.Enter();
        player.detectRatio = 0.5f;

    }
    public override void Exit()
    {
        base.Exit();
        player.detectRatio = 1.0f;
    }
    public override void Update()
    {
        base.Update();
        if (!player.isGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            player.SetVelocity(0.0f, 0.0f);
        }
        else
        {
            player.SetVelocity(player.moveSpeed * player.facingDir * 0.35f, player.rb.velocity.y);

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.SetVelocity(player.rb.velocity.x, player.jumpSpeed);
        }
        if (Input.GetKeyDown(KeyCode.F) && player.dashCooldown <= 0)
        {
            stateMachine.ChangeState(player.dashState);
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
