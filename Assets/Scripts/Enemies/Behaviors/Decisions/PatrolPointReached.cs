using UnityEngine;

[CreateAssetMenu(menuName = "Behaviors/Decision/PatrolPointReached")]
public class PatrolPointReached : Decision
{
    public override bool Decide(AIStateMachine stateMachine)
    {
        var wayPoint = stateMachine.GetComponent<WayPoint>();
        var agent = stateMachine.GetComponent<Enemy>().Agent;
        return wayPoint.HasReachedWayPoint(agent);
    }
}
