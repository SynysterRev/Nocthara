using UnityEngine;

[CreateAssetMenu(menuName = "Behaviors/Transitions/CustomWaitTransition")]
public class CustomWaitTransition : BehaviorTransition
{
    public override void Execute(AIStateMachine stateMachine)
    {
        if (Decision.Decide(stateMachine))
        {
            stateMachine.StartStayingCoroutine();
            stateMachine.ChangeState(Behavior);
        }
    }
}
