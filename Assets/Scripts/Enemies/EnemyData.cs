using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Characteristics")]
    public int Health;
    public int Damage;
    public float Speed;
    public float RangeAttack;
    
    [Space(10)]
    [Header("Detection")] 
    public float ViewRange;
    [Range(0, 360)]
    public float ViewAngle;

    public float RadiusChase;
}
