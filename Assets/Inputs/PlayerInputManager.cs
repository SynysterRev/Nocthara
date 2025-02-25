using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputManager", menuName = "Data/PlayerInputManager")]
public class PlayerInputManager : ScriptableObject, PlayerInput_Actions.IPlayerActions
{
    public event UnityAction<Vector2> MoveEvent;
    public event UnityAction DashEvent;
    public event UnityAction AttackEvent;
    public event UnityAction InteractEvent;
    public event UnityAction UseSkillEvent;

    private PlayerInput_Actions _playerInput;

    private void OnEnable()
    {
        if (_playerInput == null)
        {
            _playerInput = new PlayerInput_Actions();
            _playerInput.Player.Enable();
            _playerInput.Player.SetCallbacks(this);
        }
    }

    private void OnDisable()
    {
        _playerInput.Player.Disable();
    }

    public void EnableInput(bool enable)
    {
        if (enable)
            _playerInput.Player.Enable();
        else 
            _playerInput.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase is InputActionPhase.Performed or InputActionPhase.Canceled)
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase is InputActionPhase.Started)
            AttackEvent?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase is InputActionPhase.Started)
        {
            InteractEvent?.Invoke();
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
    }

    public void OnJump(InputAction.CallbackContext context)
    {
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.phase is InputActionPhase.Started)
            DashEvent?.Invoke();
    }

    public void OnSkill(InputAction.CallbackContext context)
    {
        if (context.phase is InputActionPhase.Started)
            UseSkillEvent?.Invoke();
    }
}