using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public int Health;
    public int MaxHealth;
    public int Mana;
    public int MaxMana;
}
