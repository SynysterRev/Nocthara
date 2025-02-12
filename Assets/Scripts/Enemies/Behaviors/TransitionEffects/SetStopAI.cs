using UnityEngine;

[CreateAssetMenu(menuName = "Behaviors/TransitionEffects/SetStopAIEffect")]
public class SetStopAI : TransitionEffect
{
    public override void Apply(AIStateMachine stateMachine)
    {
        stateMachine.StopMove();
    }
}
