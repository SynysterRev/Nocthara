using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviors/Actions/Patrol")]
public class PatrolAction : ActionBehavior
{
    private void Patrol(AIStateMachine stateMachine)
    {
        var enemy = stateMachine.GetComponent<Enemy>();
        var wayPoint = stateMachine.GetComponent<WayPoint>();
        if (!enemy.Agent.hasPath)
        {
            Debug.Log("new patrol");
            stateMachine.SetDestination(wayPoint.GetWayPoint());
        }
    }

    public override void Execute(AIStateMachine stateMachine)
    {
        Patrol(stateMachine);
        stateMachine.UpdateDirection();
    }
}
