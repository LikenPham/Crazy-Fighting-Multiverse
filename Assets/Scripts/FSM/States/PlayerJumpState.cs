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

        int jumpHash = stateMachine.animData.GetHash(AnimKey.Jump);
        bool animDone = stateMachine.animationModule.IsAnimationFinished(jumpHash);
        bool falling = stateMachine.movementModule.VerticalVelocity < 0f;

        if (animDone || falling)
            stateMachine.ChangeState<PlayerFallState>();
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
