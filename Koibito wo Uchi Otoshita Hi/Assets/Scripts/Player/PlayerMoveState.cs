using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    private float speedX;
    public PlayerMoveState(PlayerStateMachine _stateMachine, Player2 _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
        player = _player;
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        player.SetVelocity(player.moveSpeed * player.facingDir, player.rb.velocity.y);
        if (!player.isGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            stateMachine.ChangeState(player.idleState);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.SetVelocity(player.rb.velocity.x, player.jumpSpeed);
        }
        if (Input.GetKeyDown(KeyCode.F) && player.dashCooldown <= 0)
        {
            stateMachine.ChangeState(player.dashState);
        }
    }
}
