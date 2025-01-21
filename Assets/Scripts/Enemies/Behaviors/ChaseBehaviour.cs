using UnityEngine;

public class ChaseBehaviour : AIBehavior
{
    public override void Initialize(GameObject npc)
    {
        base.Initialize(npc);
        _enemy.GetComponent<SpriteRenderer>().color = Color.red;
        _enemy.Agent.SetDestination(_aiStateMachine.Target.position);
    }
    
    public override void ExecuteBehaviour()
    {
        float distance = Vector3.Distance(_aiStateMachine.Target.position, _enemy.transform.position);
        if (distance > _enemy.RadiusChase)
        {
            _aiStateMachine.Target = null;
            _aiStateMachine.ChangeState(new PatrolBehavior());
        }
        else
        {
            _enemy.Agent.SetDestination(_aiStateMachine.Target.position);
        }
    }

    public override void ExitBehaviour()
    {
        _enemy.GetComponent<SpriteRenderer>().color = Color.blue;
    }
}
