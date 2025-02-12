using UnityEngine;

[CreateAssetMenu(menuName = "Behaviors/Actions/Stay")]
public class StayAction : ActionBehavior
{
    public override void Initialize(AIStateMachine stateMachine)
    {
        stateMachine.StartStayingCoroutine();
    }

    public override void Execute(AIStateMachine stateMachine)
    {
            
    }

    public override void Exit(AIStateMachine stateMachine)
    {
        
    }
}