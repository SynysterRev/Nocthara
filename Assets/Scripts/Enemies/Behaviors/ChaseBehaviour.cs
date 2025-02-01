using UnityEngine;

public class ChaseBehaviour : AIBehavior
{
    public override void Initialize(GameObject npc)
    {
        base.Initialize(npc);
        _aiStateMachine.SetDestination(_aiStateMachine.Target.position);
    }
    
    public override void ExecuteBehaviour()
    {
        float distance = Vector3.Distance(_aiStateMachine.Target.position, _enemy.transform.position);
        _aiStateMachine.UpdateDirection();
        if (distance > _enemy.RadiusChase)
        {
            _aiStateMachine.Target = null;
            _aiStateMachine.ChangeState(new PatrolBehavior());
        }
        else if (distance <= _enemy.RangeAttack)
        {
            _aiStateMachine.StopMove();
            _aiStateMachine.ChangeState(new AttackBehavior());
        }
        else
        {
            _aiStateMachine.SetDestination(_aiStateMachine.Target.position);
        }
    }

    public override void ExitBehaviour()
    {
        
    }
}
