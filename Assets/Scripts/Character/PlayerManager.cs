using UnityEngine;

public class CharacterStats
{
    public int Health;
    public int MaxHealth;
    public int Mana;
    public int MaxMana;
}

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField]
    private CharacterData CharacterData;
    
    private CharacterStats _stats;

    protected override void Awake()
    {
        base.Awake();
        _stats = new CharacterStats
        {
            Health = CharacterData.Health,
            MaxHealth = CharacterData.MaxHealth,
            Mana = CharacterData.Mana,
            MaxMana = CharacterData.MaxMana
        };
    }

}
