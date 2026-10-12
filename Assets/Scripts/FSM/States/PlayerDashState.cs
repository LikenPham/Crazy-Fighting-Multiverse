using UnityEngine;

public class PlayerDashState : PlayerState
{

    private const float dashDuration = 0.2f;
    private float timer;
    public PlayerDashState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        timer = dashDuration;
    }

    public override void LogicUpdate()
    {
        timer -= Time.deltaTime;

        if (timer <= 0) 
            ChangeOtherState();
    }

    public override void PhysicsUpdate()
    {
        stateMachine.movementModule.Dash();
    }

    public override void Exit()
    {
        stateMachine.movementModule.EndDash();
    }

    public void ChangeOtherState()
    {
        if (stateMachine.InputReader.moveDirection.x != 0)
        {
            stateMachine.ChangeState<PlayerMoveState>();
        }
        else
        {
            stateMachine.ChangeState<PlayerIdleState>();
        }
    }
}
