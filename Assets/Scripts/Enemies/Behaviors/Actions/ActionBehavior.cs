using UnityEngine;

public abstract class ActionBehavior : ScriptableObject
{
    public abstract void Execute(AIStateMachine stateMachine);
}
