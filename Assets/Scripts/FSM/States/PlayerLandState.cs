using UnityEngine;

public class PlayerLandState : PlayerState
{
    private int landHash;
    public PlayerLandState (PlayerStateMachine stateMachine) : base(stateMachine)
    {
        landHash = stateMachine.animData.GetHash(AnimKey.Land);
    }

    public override void Enter()
    {
        stateMachine.movementModule.Stop();
        stateMachine.animationModule.PlayAnimation(landHash);
        stateMachine.InputReader.dashPressed += OnDash;
    }

    public override void LogicUpdate()
    {
        if (stateMachine.TryJump()) return;

        bool finished = stateMachine.animationModule.IsAnimationFinished(landHash);

        if (finished || stateMachine.InputReader.moveDirection.x != 0f)
        {
            if (stateMachine.InputReader.moveDirection.x != 0f)
                stateMachine.ChangeState<PlayerMoveState>();
            else
                stateMachine.ChangeState<PlayerIdleState>();
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
