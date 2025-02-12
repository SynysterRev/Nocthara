using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviors/State")]
public class AIState : ScriptableObject
{
    public List<BehaviorTransition> Transitions = new List<BehaviorTransition>();
    public List<ActionBehavior> Actions = new List<ActionBehavior>();

    public virtual void InitializeBehaviour(AIStateMachine stateMachine)
    {
        foreach (var actionBehavior in Actions)
        {
            actionBehavior.Initialize(stateMachine);
        }
    }
    
    public virtual void ExecuteBehaviour(AIStateMachine stateMachine)
    {
        foreach (var actionBehavior in Actions)
        {
            actionBehavior.Execute(stateMachine);
        }
        
        foreach (var transition in Transitions)
        {
            transition.Execute(stateMachine);
        }
    }

    public virtual void ExitBehaviour(AIStateMachine stateMachine)
    {
        foreach (var actionBehavior in Actions)
        {
            actionBehavior.Exit(stateMachine);
        }
    }
}