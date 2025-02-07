using UnityEngine;

[CreateAssetMenu(menuName = "Behaviors/Decision/BreakOver")]
public class BreakOver : Decision
{
    public override bool Decide(AIStateMachine stateMachine)
    {
        return !stateMachine.IsStaying;
    }
}
