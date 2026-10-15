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
    }

    public override void LogicUpdate()
    {
        if (stateMachine.TryJump()) return;

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
    }

    private void OnDash()
    {
        stateMachine.ChangeState<PlayerDashState>();
    }
}
