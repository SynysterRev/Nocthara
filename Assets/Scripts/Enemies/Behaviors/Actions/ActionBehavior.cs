using UnityEngine;

public abstract class ActionBehavior : ScriptableObject
{
    public abstract void Initialize(AIStateMachine stateMachine);
    public abstract void Execute(AIStateMachine stateMachine);
    public abstract void Exit(AIStateMachine stateMachine);
}
