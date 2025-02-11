using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Behaviors/Transitions/BaseTransition")]
public class BehaviorTransition : ScriptableObject
{
    public Decision Decision;
    public AIState Behavior;
    public List<TransitionEffect> TransitionEffects = new List<TransitionEffect>();

    public virtual void Execute(AIStateMachine stateMachine)
    {
        if (Decision.Decide(stateMachine))
        {
            foreach (TransitionEffect effect in TransitionEffects)
                effect.Apply(stateMachine);
            
            stateMachine.ChangeState(Behavior);
        }
    }
}
