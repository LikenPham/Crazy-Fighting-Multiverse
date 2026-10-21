using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    [SerializeField] public MovementModule movementModule;
    [SerializeField] public AnimationModule animationModule;

    [SerializeField] public AnimationDataSO animData;

    [SerializeField] private float coyoteTime = 0.1f;

    [SerializeField] private float jumpBufferTime = 0.12f;

    private float coyoteTimer;
    private float jumpBufferTimer;

    public InputReader InputReader => inputReader;

    private readonly Dictionary<Type, PlayerState> states = new();
    public PlayerState currentState { get; private set; }

    private void Awake()
    {
        movementModule = GetComponent<MovementModule>();
        animationModule = GetComponent<AnimationModule>();

        animData.InitializeHashes();

        RegisterStates(
            new PlayerIdleState(this),
            new PlayerMoveState(this), 
            new PlayerDashState(this),
            new PlayerJumpState(this),
            new PlayerFallState(this),
            new PlayerLandState(this)
        );
    }

    public void Start()
    {
        inputReader.jumpPressed += BufferJump;
        ChangeState<PlayerIdleState>();
    }

    private void OnDestroy()
    {
        inputReader.jumpPressed -= BufferJump;
    }

    private void Update()
    {
        TickJumpTimers();
        currentState?.LogicUpdate();
    }

    private void FixedUpdate()
    {
        currentState?.PhysicsUpdate();
        movementModule.ApplyBetterGravity();
    }

    private void RegisterStates(params PlayerState[] stateLists)
    {
        foreach (PlayerState state in stateLists)
            states[state.GetType()] = state;
    }

    public T GetState<T>() where T : PlayerState => (T)states[typeof(T)];

    public void ChangeState<T>() where T : PlayerState => ChangeState(GetState<T>());

    public void ChangeState(PlayerState newState)
    {
        Debug.Log($"{currentState?.GetType().Name} -> {newState.GetType().Name}");
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    private void BufferJump() => jumpBufferTimer = jumpBufferTime;

    public void TickJumpTimers()
    {
        if (movementModule.IsGrounded() && movementModule.VerticalVelocity <= 0.01f)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;

        jumpBufferTimer -= Time.deltaTime;
    }

    public bool TryJump()
    {
        if (jumpBufferTimer <=0f || coyoteTimer <= 0f) return false;

        jumpBufferTimer = 0f;
        coyoteTimer = 0f;
        ChangeState<PlayerJumpState>();
        return true;
    }

}
