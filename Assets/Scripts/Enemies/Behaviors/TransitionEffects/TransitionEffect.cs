using UnityEngine;

public abstract class TransitionEffect : ScriptableObject
{
    public abstract void Apply(AIStateMachine stateMachine);
}