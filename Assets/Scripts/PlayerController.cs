using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State
{
    Idle,
    Walk,
    Dash,
    Attack
}

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    
    [SerializeField]
    private InputManager PlayerInputs = default;

    [SerializeField]
    private Transform TransformAttack;
    [SerializeField]
    private float AttackRange;
    
    [SerializeField]
    private Animator Animator;
    private string _currentStateAnim = "";
    private List<string> _currentStateNames = new List<string>{ "_up", "_leftup", "_left", "_leftdown", "_down", "_rightdown", "_right", "_rightup"  };
    
    private Vector2 _movement;
    private Vector2 _lastFacedDirection = Vector2.down;
    private Rigidbody2D _rb;
    private Vector2 _move;

    private State _currentState = State.Idle;

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
        if (CanMove())
        {
            _movement.Set(_move.x, _move.y);

            _rb.linearVelocity = _movement.normalized * moveSpeed;
        }

        UpdateAnimState();
    }
    
    private void OnMove(Vector2 value)
    {
        _move = value;
        if (value.magnitude > 0)
        {
            _lastFacedDirection = value.normalized;
            _currentState = State.Walk;
        }
        else
        {
            _currentState = State.Idle;
        }
    }

    private void OnDash()
    {
        if (_currentState != State.Dash)
        {
            _currentState = State.Dash;
            _rb.linearVelocity = _lastFacedDirection * moveSpeed * 4.0f;
            StartCoroutine(StartDash());
        }
    }

    private bool CanMove()
    {
        return _currentState is State.Walk or State.Idle;
    }

    IEnumerator StartDash()
    {
        yield return new WaitForSeconds(0.2f);
        if( _move.magnitude > 0.1f )
            _currentState = State.Walk;
        else 
            _currentState = State.Idle;
    }

    private void OnAttack()
    {
        //play animation
        _currentState = State.Attack;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(TransformAttack.position, AttackRange);
    }
    
    private void UpdateAnimState()
    {
        int step = 360 / 8;
        float angle;
        string prefix;

        // if(isAttacking)
        // {
        //     angle = Vector2.SignedAngle(Vector2.up, lastDirection.normalized);
        //     prefix = "attack";
        // }
        //player is moving
        if (_currentState == State.Dash)
        {
            angle = Vector2.SignedAngle(Vector2.up, _lastFacedDirection.normalized);
            prefix = "dash";
        }
        else if (_currentState == State.Walk)
        {
            angle = Vector2.SignedAngle(Vector2.up, _move.normalized);
            prefix = "walk";
        }
        else
        {
            angle = Vector2.SignedAngle(Vector2.up, _lastFacedDirection.normalized);
            prefix = "idle";
        }
        if (angle < 0.0f)
        {
            angle += 360.0f;
        }
        angle /= step;
        int index = (int)angle;
        ChangeAnimationState(prefix + _currentStateNames[index]);
    }
    private void ChangeAnimationState(string newState)
    {
        if (_currentStateAnim == newState)
            return;

        _currentStateAnim = newState;
        Animator.Play(_currentStateAnim, 0);
    }

    public void EnableInput(bool bEnable)
    {
        if(bEnable)
        {
            // iM.JumpEvent += OnJump;
            // iM.SprintEvent += OnSprint;
            // iM.SprintEventCanceled += OnSprintCanceled;
            PlayerInputs.MoveEvent += OnMove;
            PlayerInputs.DashEvent += OnDash;
            PlayerInputs.AttackEvent += OnAttack;
            // iM.LookEvent += OnLook;
            // iM.InteractEvent += OnInteract;
        }
        else
        {
            // iM.JumpEvent -= OnJump;
            // iM.SprintEvent -= OnSprint;
            // iM.SprintEventCanceled -= OnSprintCanceled;
            PlayerInputs.MoveEvent -= OnMove;
            PlayerInputs.DashEvent -= OnDash;
            PlayerInputs.AttackEvent -= OnAttack;
            // iM.LookEvent -= OnLook;
            // iM.InteractEvent -= OnInteract;
        }
    }
}
