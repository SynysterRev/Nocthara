using UnityEngine;

[CreateAssetMenu(menuName = "Behaviors/Actions/Chase")]
public class ChaseAction : ActionBehavior
{
    public override void Execute(AIStateMachine stateMachine)
    {
        stateMachine.UpdateDirection();
        stateMachine.SetDestination(stateMachine.Target.position);
    }
}
