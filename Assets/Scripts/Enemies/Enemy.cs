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

    private void Awake()
    {
        _stats = new EnemyStat
        {
            Health = Data.Health,
            Damage = Data.Damage,
            Speed = Data.Speed
        };
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int damage)
    {
        _stats.Health -= damage;
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
    
    public Vector3 DirFromAngle(float angleInDegrees)
    {
        //move cone in direction of enemy movement
        float angle = -Vector3.SignedAngle(Vector3.up, Direction, Vector3.forward);
        return new Vector3(Mathf.Sin((angleInDegrees + angle) * Mathf.Deg2Rad), Mathf.Cos((angleInDegrees + angle) * Mathf.Deg2Rad),0.0f);
    }
}
