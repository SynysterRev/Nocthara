using UnityEngine;

[CreateAssetMenu(menuName = "Behaviors/Actions/Stay")]
public class StayAction : ActionBehavior
{
    public override void Execute(AIStateMachine stateMachine)
    {
        if (!stateMachine.IsStaying)
            stateMachine.StartStayingCoroutine();
    }
}