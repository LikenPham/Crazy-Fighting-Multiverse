using UnityEngine;

public class PlayerMoveState : PlayerState
{
    private int moveHash;
    private float idleDelay;
    private const float idleDelayTime = 0.06f;

    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        moveHash = stateMachine.animData.GetHash(AnimKey.Move);
    }

    public override void Enter()
    {
        idleDelay = 0f;
        stateMachine.InputReader.dashPressed += OnDash;
    }

    public override void LogicUpdate()
    {
        if (stateMachine.TryJump()) return;

        float moveInput = stateMachine.InputReader.moveDirection.x;

        if (moveInput == 0)
        {
            idleDelay += Time.deltaTime;
            if (idleDelay >= idleDelayTime)
            {
                stateMachine.ChangeState<PlayerIdleState>();
                Debug.Log(idleDelay);
                return;
            }
        }
        else
        {
            idleDelay = 0f;
        }
    }

    public override void PhysicsUpdate()
    {
        float moveInput = stateMachine.InputReader.moveDirection.x;
        stateMachine.movementModule.Move(moveInput);
        stateMachine.animationModule.PlayAnimation(moveHash);
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
