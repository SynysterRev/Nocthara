using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class AIStateMachine : MonoBehaviour
{
    [SerializeField] 
    private AIState StartState;

    [SerializeField] 
    private Transform AnchorWeapon;
    private Quaternion _baseRotation;

    private AIState _currentState;
    private Enemy _enemy;

    private Dictionary<Type, Component> _cachedComponents;

    public Transform Target
    {
        get => GetParameter<Transform>(nameof(Target));
        set => SetParameter(nameof(Target), value);
    }
    public Animator Animator;

    private readonly float _checkPlayerTime = 0.3f;
    private float _checkPlayerTimer = 0.0f;

    public bool IsStaying
    {
        get => GetParameter<bool>(nameof(IsStaying));
        set => SetParameter(nameof(IsStaying), value);
    }
    
    public bool IsAttacking { get; private set; }
    public bool CanChangeState { get; set; } = true;

    private Coroutine _stayCoroutine;
    private Coroutine _attackCoroutine;

    private Dictionary<string, object> _parameters = new Dictionary<string, object>();
    public void SetParameter<T>(string key, T value) => _parameters[key] = value;

    public T GetParameter<T>(string key)
    {
        if (_parameters.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }

        return default;
    }
    
    private void Awake()
    {
        _cachedComponents = new Dictionary<Type, Component>();
        _baseRotation = AnchorWeapon.rotation;
        AnchorWeapon.gameObject.SetActive(false);
    }

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _checkPlayerTimer = _checkPlayerTime;
        _currentState = StartState;
        _currentState?.InitializeBehaviour(this);
    }

    private void Update()
    {
        if (!Target)
        {
            _checkPlayerTimer -= Time.deltaTime;
            if (_checkPlayerTimer <= 0.0f)
            {
                _checkPlayerTimer += _checkPlayerTime;
                CheckForPlayer();
            }
        }

        _currentState?.ExecuteBehaviour(this);
    }

    public void ChangeState(AIState newState)
    {
        if (_currentState != null && _currentState != newState)
        {
            _currentState?.ExitBehaviour(this);
            _currentState = newState;
            _currentState?.InitializeBehaviour(this);
        }
    }

    private void CheckForPlayer()
    {
        Collider2D[] collider2Ds =
            Physics2D.OverlapCircleAll(transform.position, _enemy.ViewRange, LayerMask.GetMask("Player"));
        if (collider2Ds.Length > 0)
        {
            foreach (var coll2D in collider2Ds)
            {
                Vector3 targetDirection = coll2D.transform.position - transform.position;
                float angle = Vector3.Angle(_enemy.LastDirection, targetDirection);
                if (angle > _enemy.ViewAngle / 2.0f + 10.0f) break;
                PlayerController pc = coll2D.GetComponent<PlayerController>();
                if (pc && !pc.IsDead())
                {
                    Target = pc.transform;
                }
            }
        }
    }

    public void SetDestination(Vector3 destination)
    {
        _enemy.Agent.SetDestination(destination);
        _enemy.Agent.isStopped = false;
        if (Animator)
        {
            Animator.SetBool("IsWalking", true);
            Animator.SetFloat("MoveX", _enemy.Direction.x);
            Animator.SetFloat("MoveY", _enemy.Direction.y);
        }
    }

    public void StopMove()
    {
        _enemy.Agent.velocity = Vector3.zero;
        _enemy.Agent.isStopped = true;
    }

    public void UpdateDirection()
    {
        if (Animator)
        {
            _enemy.Direction = _enemy.Agent.velocity.normalized;
            Animator.SetFloat("MoveX", _enemy.Direction.x);
            Animator.SetFloat("MoveY", _enemy.Direction.y);
            if (_enemy.Direction != Vector2.zero)
            {
                Animator.SetFloat("LastDirectionX", _enemy.Direction.x);
                Animator.SetFloat("LastDirectionY", _enemy.Direction.y);
                _enemy.LastDirection = _enemy.Direction;
            }
        }
    }

    public new T GetComponent<T>() where T : Component
    {
        if (_cachedComponents.ContainsKey(typeof(T)))
            return _cachedComponents[typeof(T)] as T;

        var component = base.GetComponent<T>();
        if (component != null)
        {
            _cachedComponents.Add(typeof(T), component);
        }

        return component;
    }

    public void StartStayingCoroutine()
    {
        if (_stayCoroutine == null)
        {
            Animator.SetBool("IsWalking", false);
            Animator.SetFloat("MoveX", _enemy.LastDirection.x);
            Animator.SetFloat("MoveY", _enemy.LastDirection.y);
            _stayCoroutine = StartCoroutine(StayCoroutine(1.0f));
        }
    }

    private IEnumerator StayCoroutine(float duration)
    {
        IsStaying = true;
        yield return new WaitForSeconds(duration);
        IsStaying = false;
        _stayCoroutine = null;
    }

    public void StartAttack()
    {
        if (_attackCoroutine == null)
        {
            Animator.SetBool("IsWalking", false);
            Animator.SetTrigger("Attack");
            CanChangeState = false;
            float angle = Vector2.SignedAngle(Vector2.down, Target.position - _enemy.transform.position);
            AnchorWeapon.rotation = _baseRotation * Quaternion.AngleAxis(angle, Vector3.forward);
            AnchorWeapon.gameObject.SetActive(true);
            float animationLength = Tools.GetAnimationLength(Animator, "attack");
            _attackCoroutine = StartCoroutine(AttackCoroutine(animationLength));
        }
    }
    
    private IEnumerator AttackCoroutine(float duration)
    {
        IsAttacking = true;
        yield return new WaitForSeconds(duration);
        IsAttacking = false;
        _attackCoroutine = null;
        AnchorWeapon.gameObject.SetActive(false);
        CanChangeState = true;
    }
}