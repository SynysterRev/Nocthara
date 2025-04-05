using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
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
    [FormerlySerializedAs("PlayerPlayerInputs")]
    [Header("Input")]
    [SerializeField]
    private PlayerInputManager PlayerInputs;

    [Header("Attack")]
    [SerializeField] 
    private AttackBox BoxAttack;

    [SerializeField] 
    private Transform WeaponAnchor;
    private Quaternion _baseRotation;
    
    [Header("Movement")]
    [SerializeField] 
    private Transform StartBoxCast;
    
    [SerializeField]
    private Vector2 BoxCastSize;

    [SerializeField] 
    private float DashTime = 0.2f;
    [SerializeField] 
    private float DashSpeed = 5.0f;

    [SerializeField] 
    private float BumpForce;
        
    [Header("Animation")]
    [SerializeField] 
    private Animator Animator;
    private string _currentStateAnim = "";
    private List<string> _currentStateNames = new List<string>{ "_up", "_leftup", "_left", "_leftdown", "_down", "_rightdown", "_right", "_rightup"  };
    
    [Header("Debug")]
    [SerializeField]
    private bool DebugMode;
    
    private Vector2 _movement;
    private Vector2 _lastFacedDirection = Vector2.down;
    private Vector2 _move;
    
    public Vector2 LastFacedDirection => _lastFacedDirection;

    private State _currentState = State.Idle;
    
    private PlayerManager _playerManager;
    private float _moveSpeed = 5f;
    
    private CollisionResolve _collisionResolve;
    private FlashDamage _flashDamage;

    private void OnEnable()
    {
        EnablePlayerInput(true);
    }

    private void OnDisable()
    {
        EnablePlayerInput(false);
    }

    private void Awake()
    {
        _baseRotation = WeaponAnchor.rotation;
        WeaponAnchor.gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BindPlayerInput(true);
        _playerManager = PlayerManager.Instance;
        _moveSpeed = _playerManager.Speed;
        _playerManager.OnDie += OnDie;
        _flashDamage = GetComponent<FlashDamage>();
    }

    private void OnDestroy()
    {
        BindPlayerInput(false);
        _playerManager.OnDie -= OnDie;
    }

    // Update is called once per frame
    void Update()
    {
        TryMove();
        UpdateAnimState();
    }

    private void TryMove()
    {
        if (CanMove())
        {
            Vector2 moveX = new Vector2(_move.x, 0.0f);
            if (!CheckForCollision(_moveSpeed, moveX))
            {
                transform.Translate(moveX * (_moveSpeed * Time.deltaTime), Space.World);
            }
            Vector2 moveY = new Vector2(0.0f, _move.y);
            if (!CheckForCollision(_moveSpeed, moveY))
            {
                transform.Translate(moveY * (_moveSpeed * Time.deltaTime), Space.World);
            }
        }
    }

    private void FixedUpdate()
    {
        if (_collisionResolve.IsColliding)
        {
            _collisionResolve.IsColliding = false;
            Vector2 direction = (transform.position - _collisionResolve.CollidingObject.transform.position).normalized;
            transform.Translate(direction * BumpForce, Space.World);
            _currentState = State.Bump;
            _collisionResolve.CollidingObject = null;
            BindPlayerInput(false);
            StartCoroutine(Bump());
        }
    }
    
    private bool CanMove()
    {
        return _currentState is State.Walk or State.Idle;
    }

    private bool CheckForCollision(float speed, Vector2 direction)
    {
        float distance = speed * Time.deltaTime;
        Vector2 newPosition = (Vector2)StartBoxCast.position + direction * distance;

        RaycastHit2D hit = Physics2D.BoxCast(newPosition, BoxCastSize, 0, direction,
            distance, ~LayerMask.GetMask("Player"));
        if (DebugMode)
            DrawDebug();
        if (hit.collider && !hit.collider.isTrigger)
        {
            return true;
        }
        return false;
    }

    private IEnumerator Bump()
    {
        yield return new WaitForSeconds(0.3f);
        BindPlayerInput(true);
    }

    private void OnMove(Vector2 value)
    {
        _move = value;
        _move.Normalize();
        if (value.magnitude > 0)
        {
            _lastFacedDirection = value;
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
            StartCoroutine(StartDash());
        }
    }
    
    IEnumerator StartDash()
    {
        float timer = 0.0f;
        Vector2 direction = _lastFacedDirection;
        while (timer < DashTime)
        {
            timer += Time.deltaTime;
            if (!CheckForCollision(DashSpeed, direction))
                transform.Translate(direction * (Time.deltaTime * DashSpeed), Space.World);
            yield return null;
        }
        
        _currentState = _move.magnitude > 0.1f ? State.Walk : State.Idle;
    }

    private void DrawDebug()
    {
        Vector2 newPosition = (Vector2)StartBoxCast.position + _move * _moveSpeed * Time.deltaTime;
        // Afficher un rectangle représentant la BoxCast
        Debug.DrawLine(StartBoxCast.position, StartBoxCast.position + new Vector3(_lastFacedDirection.x, _lastFacedDirection.y) * _moveSpeed * Time.deltaTime, Color.red);

// Calculer les coins de la boîte
        Vector2 boxCenter = newPosition;
        Vector2 boxSize = new Vector2(0.3f, 0.1f); // Remplacer par la taille de ta boîte
        float angle = GetAngle(_lastFacedDirection); // L'angle de rotation de la boîte

// Dessiner les bords de la boîte
        Vector2 topLeft = new Vector2(-boxSize.x / 2, boxSize.y / 2) + boxCenter;
        Vector2 topRight =  new Vector2(boxSize.x / 2, boxSize.y / 2) + boxCenter;
        Vector2 bottomLeft = new Vector2(-boxSize.x / 2, -boxSize.y / 2) + boxCenter;
        Vector2 bottomRight =  new Vector2(boxSize.x / 2, -boxSize.y / 2) + boxCenter;

        Debug.DrawLine(topLeft, topRight, Color.blue);  // Dessiner la ligne supérieure
        Debug.DrawLine(topRight, bottomRight, Color.blue); // Dessiner la ligne droite
        Debug.DrawLine(bottomRight, bottomLeft, Color.blue); // Dessiner la ligne inférieure
        Debug.DrawLine(bottomLeft, topLeft, Color.blue);  // Dessiner la ligne gauche
    }

    private void OnAttack()
    {
        if (!_playerManager.CanAttack)
            return;
        //play animation
        _currentState = State.Attack;
        float angle = Vector2.SignedAngle(Vector2.down, _lastFacedDirection.normalized);
        Vector2 point = new Vector2(transform.position.x, transform.position.y) + _lastFacedDirection.normalized * BoxAttack.BoxOffset;
        var colliders = Physics2D.OverlapBoxAll(point, BoxAttack.BoxSize, angle, LayerMask.GetMask("Enemies"));
        WeaponAnchor.rotation = _baseRotation * Quaternion.AngleAxis(angle, Vector3.forward);
        WeaponAnchor.gameObject.SetActive(true);
        StartCoroutine(Attack());
        if (colliders.Length > 0)
        {
            foreach (var col in colliders)
            {
                var damageable = col.GetComponentInParent<IDamageable>();
                damageable?.TakeDamage(1);
            }
        }
    }

    private void OnInteract()
    {
        RaycastHit2D hit = Physics2D.Raycast(StartBoxCast.position, _lastFacedDirection.normalized, 3.0f, ~LayerMask.GetMask("Player"));
        if (hit.collider)
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {        
                interactable.TryInteract(gameObject, hit.collider.gameObject);
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
        if (other.GetComponentInParent<Enemy>())
        {
            // _collisionResolve.IsColliding = true;
            // _collisionResolve.CollidingObject = other.gameObject;
            // TakeDamage(1);
        }
    }

    private void BindPlayerInput(bool bEnable)
    {
        if(bEnable)
        {
            PlayerInputs.MoveEvent += OnMove;
            PlayerInputs.DashEvent += OnDash;
            PlayerInputs.AttackEvent += OnAttack;
            PlayerInputs.InteractEvent += OnInteract;
        }
        else
        {
            PlayerInputs.MoveEvent -= OnMove;
            PlayerInputs.DashEvent -= OnDash;
            PlayerInputs.AttackEvent -= OnAttack;
            PlayerInputs.InteractEvent -= OnInteract;
        }
    }

    public void EnablePlayerInput(bool enable)
    {
        PlayerInputs.EnableInput(enable);
    }
    public void TakeDamage(int damage)
    {
        if (_playerManager.CanTakeDamage)
        {
            _playerManager.TakeDamage(damage);
            _flashDamage.PlayFlash(_playerManager.InvulnerabilityTime);
        }
    }

    private void OnDie(int health, int maxHealth)
    {
        _move = Vector2.zero;
        BindPlayerInput(false);
        Animator.SetTrigger("IsDead");
        Animator.SetFloat("DirectionX", _lastFacedDirection.x);
        Animator.SetFloat("DirectionY", _lastFacedDirection.y);
    }

    public bool IsDead()
    {
        return PlayerManager.Instance.IsDead;
    }

    public void ChangeFacingDirection(Vector2 direction)
    {
        _lastFacedDirection = direction;
    }

    public void EnteringDoor(Vector2 direction, float speed)
    {
        _move = direction;
        _moveSpeed = speed;
        _currentState = State.Walk;
    }

    public void ResetMoveSpeed()
    {
        _moveSpeed = _playerManager.Speed;
    }

    public void StopMoving()
    {
        _currentState = State.Idle;
        _move = Vector2.zero;
    }
}
