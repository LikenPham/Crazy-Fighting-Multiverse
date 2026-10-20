using UnityEngine;

public class PlayerIdleState : PlayerState
{
    private int idleHash;

    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        idleHash = stateMachine.animData.GetHash(AnimKey.Idle);
    }

    public override void Enter()
    {
        stateMachine.InputReader.dashPressed += OnDash;
        stateMachine.movementModule.Stop();
    }

    public override void LogicUpdate()
    {
        stateMachine.animationModule.PlayAnimation(idleHash);

        if (stateMachine.TryJump()) return;

        if (stateMachine.InputReader.moveDirection.x != 0)
        {
            stateMachine.ChangeState<PlayerMoveState>();
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
