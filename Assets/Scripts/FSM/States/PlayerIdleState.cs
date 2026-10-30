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
        stateMachine.InputReader.normalAttack += OnNormalAttack;
        stateMachine.movementModule.Stop();
    }

    public override void LogicUpdate()
    {
        stateMachine.animationModule.PlayAnimation(idleHash);

        if (stateMachine.TryJump()) return;

        if (!stateMachine.movementModule.IsGrounded() && stateMachine.movementModule.VerticalVelocity < -0.1f)
        {
            stateMachine.ChangeState<PlayerFallState>();
            return;
        }

        if (stateMachine.InputReader.moveDirection.x != 0)
        {
            stateMachine.ChangeState<PlayerMoveState>();
        }

        if (stateMachine.InputReader.blockHeld == true)
        {
            stateMachine.ChangeState<PlayerBlockState>();
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.InputReader.dashPressed -= OnDash;
        stateMachine.InputReader.normalAttack -= OnNormalAttack;
    }

    private void OnDash()
    {
        stateMachine.ChangeState<PlayerDashState>();
    }

    private void OnNormalAttack()
    {
        stateMachine.StartAttack(AnimKey.NormalAttack);
    }

}
