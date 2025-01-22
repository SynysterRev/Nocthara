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
        CanTakeDamage = true;
        _takeDamageCoroutine = null;
    }
}