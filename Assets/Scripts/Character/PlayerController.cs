using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

enum State
{
    Idle,
    Walk,
    Dash,
    Attack,
    Bump,
}

public struct CollisionResolve
{
    public bool IsColliding;
    public GameObject CollidingObject;
}

[Serializable]
public class AttackBox
{
    public Vector2 BoxSize = new Vector2(0.8f, 0.3f);
    public float BoxOffset = 0.55f;
}

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Input")]
    [SerializeField]
    private InputManager PlayerInputs;

    [Header("Attack")]
    [SerializeField] 
    private AttackBox BoxAttack;

    [SerializeField] 
    private Transform WeaponAnchor;
    private Quaternion _baseRotation;

    [SerializeField] 
    private float BumpForce;
        
    [Header("Animation")]
    [SerializeField] 
    private Animator Animator;
    private string _currentStateAnim = "";
    private List<string> _currentStateNames = new List<string>{ "_up", "_leftup", "_left", "_leftdown", "_down", "_rightdown", "_right", "_rightup"  };
    
    private Vector2 _movement;
    private Vector2 _lastFacedDirection = Vector2.down;
    private Rigidbody2D _rb;
    private Vector2 _move;

    private State _currentState = State.Idle;
    
    private PlayerManager _playerManager;
    private float _moveSpeed = 5f;
    
    private CollisionResolve _collisionResolve;

    [Header("Damage effect")]
    [ColorUsage(true, true)]
    [SerializeField] 
    private Color FlashColor;
    [SerializeField]
    private AnimationCurve FlashCurve;
    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;
    private Coroutine _flashCoroutine;

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
        _baseRotation = WeaponAnchor.rotation;
        WeaponAnchor.gameObject.SetActive(false);
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        
        _materials = new Material[_spriteRenderers.Length];
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;
            _materials[i].SetColor("_FlashColor", FlashColor);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerManager = PlayerManager.Instance;
        _moveSpeed = _playerManager.Speed;
        _playerManager.OnDie += OnDie;
    }

    private void OnDestroy()
    {
        _playerManager.OnDie -= OnDie;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove())
        {
            _movement.Set(_move.x, _move.y);

            _rb.linearVelocity = _movement.normalized * _moveSpeed;
        }
        UpdateAnimState();
    }

    private void FixedUpdate()
    {
        if (_collisionResolve.IsColliding)
        {
            _collisionResolve.IsColliding = false;
            Vector2 direction = (transform.position - _collisionResolve.CollidingObject.transform.position).normalized;
            _rb.linearVelocity = Vector2.zero;
            // _rb.linearVelocity = direction * 5f;
            _rb.AddForce(direction * BumpForce, ForceMode2D.Impulse);
            _currentState = State.Bump;
            _collisionResolve.CollidingObject = null;
            EnableInput(false);
            StartCoroutine(Bump());
        }
    }

    private IEnumerator Bump()
    {
        yield return new WaitForSeconds(0.3f);
        _rb.linearVelocity = Vector2.zero;
        EnableInput(true);
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
            // _rb.linearVelocity = _lastFacedDirection * _moveSpeed * 4.0f;
            _rb.linearVelocity = Vector2.zero;
            _rb.AddForce(_lastFacedDirection * _moveSpeed * 2.0f, ForceMode2D.Impulse);
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
        WeaponAnchor.rotation = _baseRotation * Quaternion.AngleAxis(angle, Vector3.forward);
        WeaponAnchor.gameObject.SetActive(true);
        _rb.linearVelocity = Vector2.zero;
        StartCoroutine(Attack());
        if (colliders.Length > 0)
        {
            foreach (var col in colliders)
            {
                var damageable = col.GetComponent<IDamageable>();
                damageable?.TakeDamage(1);
            }
        }
    }

    // Temporary
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.2f);
        WeaponAnchor.gameObject.SetActive(false);
        _currentState = _move.magnitude > 0.1f ? State.Walk : State.Idle;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>())
        {
            _collisionResolve.IsColliding = true;
            _collisionResolve.CollidingObject = other.gameObject;
            TakeDamage(1);
        }
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

    public void TakeDamage(int damage)
    {
        if (_playerManager.CanTakeDamage)
        {
            _playerManager.TakeDamage(damage);
            _flashCoroutine = StartCoroutine(DamageFlasher());
        }
    }

    private void OnDie(int health, int maxHealth)
    {
        _move = Vector2.zero;
        EnableInput(false);
    }

    private IEnumerator DamageFlasher()
    {
        float currentFlashAmount = 0.0f;
        float elapsedTime = 0.0f;
        
        while (elapsedTime <= _playerManager.InvulnerabilityTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1.0f, FlashCurve.Evaluate(elapsedTime), elapsedTime / _playerManager.InvulnerabilityTime);
            SetFlashAmount(currentFlashAmount);
            
            yield return null;
        }
    }

    private void SetFlashAmount(float flashAmount)
    {
        for (int i = 0; i < _materials.Length; ++i)
        {
            _materials[i].SetFloat("_FlashAmount", flashAmount);
        }
    }
}
