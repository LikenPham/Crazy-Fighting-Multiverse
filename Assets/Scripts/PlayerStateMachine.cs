using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    public InputReader InputReader => inputReader;

    public MovementModule movementModule;

    private readonly Dictionary<Type, PlayerState> states = new();
    public PlayerState currentState { get; private set; }

    private void Awake()
    {
        movementModule = GetComponent<MovementModule>();

        RegisterStates(
            new PlayerIdleState(this),
            new PlayerMoveState(this)
        );
    }

    public void Start()
    {
        ChangeState<PlayerIdleState>();
    }

    private void Update()
    {
        currentState?.LogicUpdate();
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
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

}
