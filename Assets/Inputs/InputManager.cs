using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputManager", menuName = "Data/Input Manager")]
public class InputManager : ScriptableObject, PlayerInput_Actions.IPlayerActions
{
    public event UnityAction<Vector2> MoveEvent;
    public event UnityAction DashEvent;
    public event UnityAction AttackEvent;

    private PlayerInput_Actions _playerInput;

    private void OnEnable()
    {
        if (_playerInput == null)
        {
            _playerInput = new PlayerInput_Actions();
            _playerInput.Enable();
            _playerInput.Player.SetCallbacks(this);
        }
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase is InputActionPhase.Performed or InputActionPhase.Canceled)
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase is InputActionPhase.Performed)
            AttackEvent?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
    }

    public void OnJump(InputAction.CallbackContext context)
    {
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
    }

    public void OnNext(InputAction.CallbackContext context)
    {
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.phase is InputActionPhase.Performed)
            DashEvent?.Invoke();
    }
}