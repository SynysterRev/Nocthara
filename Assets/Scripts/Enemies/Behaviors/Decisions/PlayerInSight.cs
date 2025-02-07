using UnityEngine;

[CreateAssetMenu(menuName = "Behaviors/Decision/PlayerInSight")]
public class PlayerInSight : Decision
{
    public override bool Decide(AIStateMachine stateMachine)
    {
        return stateMachine.Target != null;
    }
}
