using UnityEngine;

[CreateAssetMenu(menuName = "Behaviors/Transitions/BaseTransition")]
public class BehaviorTransition : ScriptableObject
{
    public Decision Decision;
    public AIState Behavior;

    public virtual void Execute(AIStateMachine stateMachine)
    {
        if (Decision.Decide(stateMachine))
            stateMachine.ChangeState(Behavior);
    }
}
