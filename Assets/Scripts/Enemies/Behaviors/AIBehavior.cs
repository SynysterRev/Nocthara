using UnityEngine;

[CreateAssetMenu(fileName = "AIBehavior", menuName = "Scriptable Objects/AIBehavior")]
public abstract class  AIBehavior : ScriptableObject
{
    protected GameObject _owner;
    protected Enemy _enemy;

    public virtual void Initialize(GameObject npc)
    {
        _owner = npc;
        _enemy = _owner?.GetComponent<Enemy>();
    }
    public abstract void ExecuteBehaviour();
    public abstract void ExitBehaviour();
}
