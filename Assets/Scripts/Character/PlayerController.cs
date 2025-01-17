using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

enum State
{
    Idle,
    Walk,
    Dash,
    Attack
}



[Serializable]
public class AttackBox
{
    public Vector2 BoxSize = new Vector2(0.8f, 0.3f);
    public float BoxOffset = 0.55f;
}

public class PlayerController : MonoBehaviour
{
    [FormerlySerializedAs("moveSpeed")] [SerializeField]
    private float MoveSpeed = 5f;
    
    [SerializeField]
    private InputManager PlayerInputs;

    [SerializeField] 
    private AttackBox BoxAttack;
    
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

            _rb.linearVelocity = _movement.normalized * MoveSpeed;
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
            _rb.linearVelocity = _lastFacedDirection * MoveSpeed * 4.0f;
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
        float angle = Vector2.SignedAngle(Vector2.down, _lastFacedDirection.normalized);
        Vector2 point = new Vector2(transform.position.x, transform.position.y) + _lastFacedDirection.normalized * BoxAttack.BoxOffset;
        var colliders = Physics2D.OverlapBoxAll(point, BoxAttack.BoxSize, angle, LayerMask.GetMask("Enemies"));
        if (colliders.Length > 0)
        {
            foreach (var col in colliders)
            {
                var damageable = col.GetComponent<IDamageable>();
                damageable?.Damage(1);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        var oldMatrix = Gizmos.matrix;
        float angle = Vector2.SignedAngle(Vector2.down, _lastFacedDirection.normalized);
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Vector2 point = new Vector2(transform.position.x, transform.position.y) + _lastFacedDirection.normalized * BoxAttack.BoxOffset;
        Gizmos.matrix = Matrix4x4.TRS(point, rotation, BoxAttack.BoxSize);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        Gizmos.matrix = oldMatrix;
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
            angle = GetAngle(_lastFacedDirection);
            prefix = "dash";
        }
        else if (_currentState == State.Walk)
        {
            angle = GetAngle(_move);
            prefix = "walk";
        }
        else
        {
            angle = GetAngle(_lastFacedDirection);
            prefix = "idle";
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

    private float GetAngle(Vector2 direction)
    {
        float angle = Vector2.SignedAngle(Vector2.up, direction.normalized);
        if (angle < 0.0f)
        {
            angle += 360.0f;
        }
        return angle;
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
