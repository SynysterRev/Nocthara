using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterStats
{
    public int Health;
    public int MaxHealth;
    public int Mana;
    public int MaxMana;
    public float Speed;
    public float InvulnerabilityTime;
}

public class PlayerManager : Singleton<PlayerManager>
{
    public delegate void HealthChange(int currentHealth, int maxHealth);

    public event HealthChange OnTakeDamage;
    public event HealthChange OnRestaureHealth;
    public event HealthChange OnDie;

    public float Speed => _stats.Speed;
    public float InvulnerabilityTime => _stats.InvulnerabilityTime;

    public bool CanTakeDamage = true;

    [SerializeField] 
    private CharacterData CharacterData;

    private CharacterStats _stats;
    private Coroutine _takeDamageCoroutine;

    private PlayerController _playerController;
    
    public PlayerController PlayerController => _playerController;

    private int _money;
    
    public int Money => _money;
    public int Health => _stats.Health;
    public int MaxHealth => _stats.MaxHealth;

    protected override void Awake()
    {
        base.Awake();
        _stats = new CharacterStats
        {
            Health = CharacterData.Health,
            MaxHealth = CharacterData.MaxHealth,
            Mana = CharacterData.Mana,
            MaxMana = CharacterData.MaxMana,
            Speed = CharacterData.Speed,
            InvulnerabilityTime = CharacterData.InvulnerabilityTime,
        };
    }

    private void Start()
    {
        _playerController = FindFirstObjectByType<PlayerController>();
    }

    public bool IsDead => _stats.Health <= 0;

    public void TakeDamage(int damage)
    {
        if (!CanTakeDamage) return;
        CanTakeDamage = false;
        _takeDamageCoroutine = StartCoroutine(TakeDamageCoroutine());
        _stats.Health = Mathf.Clamp(_stats.Health - damage, 0, _stats.MaxHealth);
        if (_stats.Health <= 0)
        {
            OnDie?.Invoke(_stats.Health, _stats.MaxHealth);
        }
        else
        {
            OnTakeDamage?.Invoke(_stats.Health, _stats.MaxHealth);
        }
    }

    private IEnumerator TakeDamageCoroutine()
    {
        yield return new WaitForSeconds(_stats.InvulnerabilityTime);
        if (!IsDead)
            CanTakeDamage = true;
        _takeDamageCoroutine = null;
    }

    public void RestoreHealth(int restoreHealth)
    {
        _stats.Health = Mathf.Clamp(_stats.Health + restoreHealth, 0, _stats.MaxHealth);
    }

    public void AddMoney(int amount)
    {
        _money += amount;
    }

    public void UseMoney(int amount)
    {
        if (_money >= amount)
        {
            _money -= amount;
        }
    }
}