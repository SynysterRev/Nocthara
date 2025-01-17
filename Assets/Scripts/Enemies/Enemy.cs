using System;
using UnityEngine;

public class EnemyStat
{
    public int Health;
    public int Damage;
    public float Speed;
}

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] 
    private EnemyData Data;

    private EnemyStat _stats;

    private void Awake()
    {
        _stats = new EnemyStat
        {
            Health = Data.Health,
            Damage = Data.Damage,
            Speed = Data.Speed
        };
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
}
