using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.movementModule.Stop();
        stateMachine.InputReader.dashPressed += OnDash;
        stateMachine.InputReader.jumpPressed += OnJump;
    }

    public override void LogicUpdate()
    {
        float moveInput = stateMachine.InputReader.moveDirection.x;

        if (moveInput == 0)
        {
            stateMachine.ChangeState<PlayerIdleState>();
            Debug.Log("Chuyen State Idle " + moveInput + stateMachine.movementModule.IsGrounded());
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        float moveInput = stateMachine.InputReader.moveDirection.x;
        stateMachine.movementModule.Move(moveInput);
    }

    public override void Exit()
    {
        stateMachine.InputReader.dashPressed -= OnDash;
        stateMachine.InputReader.jumpPressed -= OnJump;
    }

    private void OnDash()
    {
        stateMachine.ChangeState<PlayerDashState>();
    }

    private void OnJump()
    {
        if (stateMachine.movementModule.IsGrounded())
            stateMachine.ChangeState<PlayerJumpState>();
    }
}
