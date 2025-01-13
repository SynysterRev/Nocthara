using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 Movement;
    
    private PlayerInput _playerInput;
    private InputAction _moveAction;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        
        _moveAction = _playerInput.actions["Move"];
    }

    private void Update()
    {
        Movement = _moveAction.ReadValue<Vector2>();
    }
}
