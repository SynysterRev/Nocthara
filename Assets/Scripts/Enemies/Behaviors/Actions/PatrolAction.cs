using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviors/Actions/Patrol")]
public class PatrolAction : ActionBehavior
{
    public override void Initialize(AIStateMachine stateMachine)
    {
        var wayPoint = stateMachine.GetComponent<WayPoint>();
        stateMachine.SetDestination(wayPoint.GetWayPoint());
    }

    public override void Execute(AIStateMachine stateMachine)
    {
        stateMachine.UpdateDirection();
    }

    public override void Exit(AIStateMachine stateMachine)
    {
       
    }
}
