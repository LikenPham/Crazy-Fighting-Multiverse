using UnityEngine;

public class PlayerFallState : PlayerState
{
    private int airHash;
    public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        airHash = stateMachine.animData.GetHash(AnimKey.Air);
    }

    public override void Enter()
    {
        stateMachine.InputReader.dashPressed += OnDash;
        stateMachine.animationModule.PlayAnimation(airHash);
    }

    public override void LogicUpdate()
    {
        if (stateMachine.TryJump()) return;

        if (stateMachine.movementModule.IsGrounded() && stateMachine.movementModule.VerticalVelocity <= 0.01f)
        {
            stateMachine.ChangeState<PlayerLandState>();
        } 
    }

    public override void PhysicsUpdate()
    {
        stateMachine.movementModule.Move(stateMachine.InputReader.moveDirection.x);
    }

    public override void Exit()
    {
        stateMachine.InputReader.dashPressed -= OnDash;
    }

    private void OnDash()
    {
        stateMachine.ChangeState<PlayerDashState>();
    }
}
