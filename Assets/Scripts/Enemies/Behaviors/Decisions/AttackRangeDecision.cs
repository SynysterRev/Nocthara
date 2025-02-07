using UnityEngine;

[CreateAssetMenu(menuName = "Behaviors/Decision/AttackRangeDecision")]
public class AttackRangeDecision : Decision
{
    public override bool Decide(AIStateMachine stateMachine)
    {
        var enemy = stateMachine.GetComponent<Enemy>();
        float distance = Vector3.Distance(stateMachine.Target.position, stateMachine.transform.position);
        return distance <= enemy.RangeAttack;
    }
}
