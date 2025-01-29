using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyStat
{
    public int Health;
    public int Damage;
    public float Speed;

    // public float ViewRange;
    // [Range(0, 360)]
    // public float ViewAngle;
}

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] 
    private EnemyData Data;

    private EnemyStat _stats;
    
    private FlashDamage _flashDamage;
    
    [HideInInspector]
    public bool HasReachDestination = false;
    [HideInInspector]
    public int IndexDestination = 0;
    [HideInInspector]
    [SerializeField]
    public List<Vector2> WayPoints;
    
    [HideInInspector]
    public Vector2 Direction = Vector2.down;
    [HideInInspector]
    public Vector2 LastDirection = Vector2.down;

    public NavMeshAgent Agent { get; private set; } = null;
    public float ViewRange => Data.ViewRange;
    public float ViewAngle => Data.ViewAngle;
    public float RadiusChase => Data.RadiusChase;
    public float RangeAttack => Data.RangeAttack;

    private void Awake()
    {
        _stats = new EnemyStat
        {
            Health = Data.Health,
            Damage = Data.Damage,
            Speed = Data.Speed
        };
        Agent = GetComponent<NavMeshAgent>();
        Agent.speed = Data.Speed;
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _flashDamage = GetComponent<FlashDamage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        _stats.Health -= damage;
        _flashDamage?.PlayFlash(0.1f);
        if (_stats.Health <= 0)
        {
            _stats.Health = 0;
            Die();
        }
    }

    private void Die()
    {
        //temporary
        Destroy(gameObject);
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.GetComponent<PlayerController>() != null)
    //     {
    //         other.GetComponent<IDamageable>().Damage(_stats.Damage);
    //     }
    // }

    public Vector3 DirFromAngle(float angleInDegrees)
    {
        //move cone in direction of enemy movement
        float angle = -Vector3.SignedAngle(Vector3.up, LastDirection, Vector3.forward);
        return new Vector3(Mathf.Sin((angleInDegrees + angle) * Mathf.Deg2Rad), Mathf.Cos((angleInDegrees + angle) * Mathf.Deg2Rad),0.0f);
    }
}
