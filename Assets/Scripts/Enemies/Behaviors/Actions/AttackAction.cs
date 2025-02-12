using UnityEngine;

[CreateAssetMenu(menuName = "Behaviors/Actions/Attack")]
public class AttackAction : ActionBehavior
{
    public override void Initialize(AIStateMachine stateMachine)
    {
        stateMachine.StartAttack();
    }

    public override void Execute(AIStateMachine stateMachine)
    {
        
    }

    public override void Exit(AIStateMachine stateMachine)
    {

    }
}