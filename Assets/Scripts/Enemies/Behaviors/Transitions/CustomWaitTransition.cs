using UnityEngine;

[CreateAssetMenu(menuName = "Behaviors/Transitions/CustomWaitTransition")]
public class CustomWaitTransition : BehaviorTransition
{
    public override void Execute(AIStateMachine stateMachine)
    {
        if (Decision.Decide(stateMachine))
        {
            Debug.Log("go patrol");
            stateMachine.StartStayingCoroutine();
            stateMachine.ChangeState(Behavior);
        }
    }
}
