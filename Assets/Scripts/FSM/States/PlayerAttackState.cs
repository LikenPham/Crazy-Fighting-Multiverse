using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private AnimKey currentKey;
    private int attackHash;

    public PlayerAttackState (PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        attackHash = stateMachine.animData.GetHash(currentKey);
        AttackData data = stateMachine.attackData.GetAttack(currentKey);

        stateMachine.movementModule.Stop();

        if (data != null && data.type == AttackType.Ranged)
            stateMachine.launcher.SelectAttack(currentKey);
        else
            stateMachine.hitbox.SelectAttack(currentKey);

        stateMachine.animationModule.PlayAnimation(attackHash,0f, true);
    }

    public override void LogicUpdate()
    {
        if (stateMachine.animationModule.IsAnimationFinished(attackHash))
            stateMachine.ChangeState<PlayerIdleState>();
    }

    public override void Exit()
    {
        stateMachine.hitbox.Deactivate();   // phòng khi bị ngắt giữa chừng
    }

    public void SetAttack(AnimKey key) => currentKey = key;
}
