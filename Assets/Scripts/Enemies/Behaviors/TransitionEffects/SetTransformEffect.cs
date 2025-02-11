using UnityEngine;

[CreateAssetMenu(menuName = "Behaviors/TransitionEffects/SetTransformEffect")]
public class SetTransformEffect : TransitionEffect
{
    public string TransformName;
    public Transform TransformValue;
    
    public override void Apply(AIStateMachine stateMachine)
    {
        stateMachine.SetParameter(TransformName, TransformValue);
    }
}
