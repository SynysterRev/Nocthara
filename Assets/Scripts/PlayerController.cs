using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    
    [SerializeField]
    private InputManager PlayerInputs = default;
    
    private Vector2 _movement;
    
    private Rigidbody2D _rb;
    private Vector2 _move;

    private void OnEnable()
    {
        EnableInput(true);
    }

    private void OnDisable()
    {
        EnableInput(false);
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _movement.Set(_move.x, _move.y);
        
        _rb.linearVelocity = _movement.normalized * moveSpeed;
    }
    
    private void OnMove(Vector2 value)
    {
        _move = value;
    }
    
    public void EnableInput(bool bEnable)
    {
        if(bEnable)
        {
            // iM.JumpEvent += OnJump;
            // iM.SprintEvent += OnSprint;
            // iM.SprintEventCanceled += OnSprintCanceled;
            PlayerInputs.MoveEvent += OnMove;
            // iM.LookEvent += OnLook;
            // iM.InteractEvent += OnInteract;
        }
        else
        {
            // iM.JumpEvent -= OnJump;
            // iM.SprintEvent -= OnSprint;
            // iM.SprintEventCanceled -= OnSprintCanceled;
            PlayerInputs.MoveEvent -= OnMove;
            // iM.LookEvent -= OnLook;
            // iM.InteractEvent -= OnInteract;
        }
    }
}
