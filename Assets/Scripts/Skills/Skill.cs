using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/Skill")]
public class Skill : ScriptableObject
{
    public int Mana;
    public int Damage;
    public float Speed;
    public float Cooldown;
    public float Duration;
    public float MaxRange;
    public bool CanBeHold;
    public GameObject Prefab;
}
