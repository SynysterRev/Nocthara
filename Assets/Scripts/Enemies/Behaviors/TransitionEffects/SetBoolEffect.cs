using UnityEngine;

[CreateAssetMenu(menuName = "Behaviors/TransitionEffects/SetBoolEffect")]
public class SetBoolEffect : TransitionEffect
{
    public string BoolName;
    public bool BoolValue;
    
    public override void Apply(AIStateMachine stateMachine)
    {
        stateMachine.SetParameter(BoolName, BoolValue);
    }
}
