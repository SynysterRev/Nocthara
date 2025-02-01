using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class AIStateMachine : MonoBehaviour
{
    private AIBehavior _currentState;
    private Enemy _enemy;

    private Coroutine _checkCoroutine;

    public Transform Target { get; set; }
    public Animator Animator;

    private readonly float _checkPlayerTime = 0.3f;
    private float _checkPlayerTimer = 0.0f;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _currentState = new PatrolBehavior();
        _currentState?.Initialize(gameObject);
        _checkPlayerTimer = _checkPlayerTime;
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

        _currentState?.ExecuteBehaviour();
    }

    public void ChangeState(AIBehavior newState)
    {
        if (_currentState != newState)
        {
            _currentState?.ExitBehaviour();
            _currentState = newState;
            _currentState?.Initialize(gameObject);
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
                if (pc)
                {
                    Target = pc.transform;
                    ChangeState(new ChaseBehaviour());
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
}