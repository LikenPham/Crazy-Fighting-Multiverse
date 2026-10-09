using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, PlayerInput.IPlayerActions
{
    public Vector2 moveDirection { get; private set; }

    public event Action dashPressed; 
    public event Action jumpPressed;
    public event Action normalAttack;
    public event Action specialAttack;
    public event Action rangedAttack;

    private PlayerInput playerInput;

    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInput();
            playerInput.Player.SetCallbacks(this);
        }
        playerInput.Player.Enable();
    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
        moveDirection = Vector2.zero;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) jumpPressed?.Invoke();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed) dashPressed?.Invoke();
    }

    public void OnNormalAttack(InputAction.CallbackContext context)
    {
        if (context.performed) normalAttack?.Invoke();
    }

    public void OnSpecialAttack(InputAction.CallbackContext context)
    {
        if (context.performed) specialAttack?.Invoke();
    }

    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (context.performed) rangedAttack?.Invoke();
    }
}
