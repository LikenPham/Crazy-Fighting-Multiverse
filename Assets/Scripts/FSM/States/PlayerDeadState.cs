using UnityEngine;

public class PlayerDeadState : PlayerState
{
    private int deadHash;

    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        deadHash = stateMachine.animData.GetHash(AnimKey.Dead);
    }

    public override void Enter()
    {
        stateMachine.movementModule.Stop();
        stateMachine.hitbox.Deactivate();
        stateMachine.animationModule.PlayAnimation(deadHash, 0f, true);
        stateMachine.InputReader.enabled = false;   // khóa input, OnDisable đã reset moveDirection và blockHeld
    }
}