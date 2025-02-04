using UnityEngine;

[System.Serializable]
public class AIBehavior
{
    protected GameObject _owner;
    protected Enemy _enemy;
    protected AIStateMachine _aiStateMachine;

    public virtual void Initialize(GameObject npc)
    {
        _owner = npc;
        _enemy = _owner?.GetComponent<Enemy>();
        _aiStateMachine = _owner?.GetComponent<AIStateMachine>();
    }
    public virtual void ExecuteBehaviour() {}

    public virtual void ExitBehaviour() {}
}
