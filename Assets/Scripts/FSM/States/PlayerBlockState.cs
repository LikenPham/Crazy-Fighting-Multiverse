using UnityEngine;

public class PlayerBlockState : PlayerState
{
    private int blockHash;

    public PlayerBlockState (PlayerStateMachine stateMachine): base(stateMachine) 
    {
        blockHash = stateMachine.animData.GetHash(AnimKey.Block);
    }

    public override void Enter()
    {
        stateMachine.movementModule.Stop();
        stateMachine.animationModule.PlayAnimation(blockHash);
        stateMachine.healthModule.isBlocking = true;
    }

    public override void LogicUpdate()
    {
        if (!stateMachine.InputReader.blockHeld)
            stateMachine.ChangeState<PlayerIdleState>();
    }

    public override void Exit()
    {
        stateMachine.healthModule.isBlocking = false;
    }
}
