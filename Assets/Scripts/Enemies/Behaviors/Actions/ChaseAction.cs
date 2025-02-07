using UnityEngine;

[CreateAssetMenu(menuName = "Behaviors/Actions/Chase")]
public class ChaseAction : ActionBehavior
{
    // public override void ExecuteBehaviour(AIStateMachine stateMachine)
    // {
    //     var enemy = stateMachine.GetComponent<Enemy>();
    //     float distance = Vector3.Distance(stateMachine.Target.position, enemy.transform.position);
    //     stateMachine.UpdateDirection();
    //     if (distance > enemy.RadiusChase)
    //     {
    //         stateMachine.Target = null;
    //         // stateMachine.ChangeState(new PatrolBehavior());
    //     }
    //     else if (distance <= enemy.RangeAttack)
    //     {
    //         stateMachine.StopMove();
    //         // stateMachine.ChangeState(new AttackBehavior());
    //     }
    //     else
    //     {
    //         stateMachine.SetDestination(stateMachine.Target.position);
    //     }
    // }

    public override void Execute(AIStateMachine stateMachine)
    {
        Debug.Log("chase");
    }
}
