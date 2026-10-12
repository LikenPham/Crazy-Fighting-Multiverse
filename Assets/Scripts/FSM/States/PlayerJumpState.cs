using UnityEngine;

public class PlayerJumpState : PlayerState
{
    private bool jumped;

    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        jumped = false;
        stateMachine.InputReader.dashPressed += OnDash;
    }

    public override void LogicUpdate()
    {
        if (!jumped) return;

        float moveInput = stateMachine.InputReader.moveDirection.x;

        if (stateMachine.movementModule.VerticalVelocity <= 0f && stateMachine.movementModule.IsGrounded())
        {
            if (moveInput != 0f)
            {
                stateMachine.ChangeState<PlayerMoveState>();
            }
            else
            {
                stateMachine.ChangeState<PlayerIdleState>();
            }
        }
    }

    public override void PhysicsUpdate()
    {
        float moveInput = stateMachine.InputReader.moveDirection.x;

        if (!jumped)
        {
            Debug.Log("Jump() được gọi");
            stateMachine.movementModule.Jump();
            jumped = true;
        }

        stateMachine.movementModule.Move(moveInput);

        Debug.Log("Chuyen State Jump " + moveInput + stateMachine.movementModule.IsGrounded());
    }

    public override void Exit()
    {
        stateMachine.InputReader.dashPressed -= OnDash;
        Debug.Log("Exit Jump");
    }

    private void OnDash()
    {
        stateMachine.ChangeState<PlayerDashState>();
    }
}
