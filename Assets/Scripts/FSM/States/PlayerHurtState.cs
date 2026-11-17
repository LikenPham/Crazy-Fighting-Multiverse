using UnityEngine;

public class PlayerHurtState : PlayerState
{
    private int hurtHash;

    public PlayerHurtState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        hurtHash = stateMachine.animData.GetHash(AnimKey.Hurt);
    }

    public override void Enter()
    {
        stateMachine.movementModule.Stop();
        stateMachine.hitbox.Deactivate();   // đang đánh mà bị trúng thì ngắt đòn
        stateMachine.flashModule.Flash();
        stateMachine.animationModule.PlayAnimation(hurtHash, 0f, true);
    }

    public override void LogicUpdate()
    {
        if (!stateMachine.animationModule.IsAnimationFinished(hurtHash)) return;

        bool grounded = stateMachine.movementModule.IsGrounded();
        if (grounded) stateMachine.ChangeState<PlayerIdleState>();
        else stateMachine.ChangeState<PlayerFallState>();
    }
}