using UnityEngine;

public class PlayerJumpState : PlayerState
{
    private bool jumped;
    private int jumpHash;

    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        jumpHash = stateMachine.animData.GetHash(AnimKey.Jump);
    }

    public override void Enter()
    {
        jumped = false;
        stateMachine.animationModule.PlayAnimation(jumpHash);
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
            stateMachine.movementModule.Jump();
            jumped = true;
        }

        stateMachine.movementModule.Move(moveInput);
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
