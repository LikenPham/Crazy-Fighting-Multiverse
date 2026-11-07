using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private AnimKey currentKey;
    private int attackHash;
    private bool wasAirborne;

    public PlayerAttackState (PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        attackHash = stateMachine.animData.GetHash(currentKey);
        AttackData data = stateMachine.attackData.GetAttack(currentKey);

        wasAirborne = !stateMachine.movementModule.IsGrounded();

        if (wasAirborne)
            stateMachine.movementModule.LockInAir();

        if (!wasAirborne)
            stateMachine.movementModule.Stop();

        if (data != null && data.type == AttackType.Ranged)
            stateMachine.launcher.SelectAttack(currentKey);
        else
            stateMachine.hitbox.SelectAttack(currentKey);

        stateMachine.animationModule.PlayAnimation(attackHash,0f, true);
    }

    public override void LogicUpdate()
    {
        bool grounded = stateMachine.movementModule.IsGrounded() && stateMachine.movementModule.VerticalVelocity <= 0.01f;

        if (wasAirborne && grounded)
        {
            stateMachine.ChangeState<PlayerLandState>();
            return;
        }

        if (stateMachine.animationModule.IsAnimationFinished(attackHash))
        {
            if (wasAirborne) stateMachine.ChangeState<PlayerFallState>();
            else stateMachine.ChangeState<PlayerIdleState>();
        }
    }

    public override void Exit()
    {
        stateMachine.movementModule.UnlockGravity();
        stateMachine.hitbox.Deactivate();   // phòng khi bị ngắt giữa chừng
    }

    public void SetAttack(AnimKey key) => currentKey = key;
}
