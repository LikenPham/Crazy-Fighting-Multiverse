public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void LogicUpdate()
    {
        float moveIput = stateMachine.InputReader.moveDirection.x;

        stateMachine.movementModule.Move(moveIput);
    }
}
