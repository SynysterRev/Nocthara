using UnityEngine;

[CreateAssetMenu(menuName = "Behaviors/Decision/PlayerTooFar")]
public class PlayerTooFar : Decision
{
    public override bool Decide(AIStateMachine stateMachine)
    {
        if (PlayerManager.Instance.IsDead) return true;
        var enemy = stateMachine.GetComponent<Enemy>();
        float distance = Vector3.Distance(stateMachine.Target.position, stateMachine.transform.position);
        return distance > enemy.RadiusChase;
    }
}
