using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.InputReader.dashPressed += OnDash;
        stateMachine.movementModule.Stop();
    }

    public override void LogicUpdate()
    {
        if (stateMachine.TryJump()) return;

        if (stateMachine.InputReader.moveDirection.x != 0)
        {
            stateMachine.ChangeState<PlayerMoveState>();
            Debug.Log("Chuyen State Move " + stateMachine.InputReader.moveDirection.x + stateMachine.movementModule.IsGrounded());
        }
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
