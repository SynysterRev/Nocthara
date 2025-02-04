using UnityEngine;

[System.Serializable]
public class PatrolBehavior : AIBehavior
{
    public override void Initialize(GameObject npc)
    {
        base.Initialize(npc);
        if(_aiStateMachine)
            _aiStateMachine.SetDestination(_enemy.WayPoints[_enemy.IndexDestination]);
    }

    public override void ExecuteBehaviour()
    {
        if (!_enemy) return;
        Patrol();
        _aiStateMachine.UpdateDirection();
    }

    public override void ExitBehaviour()
    {
       
    }

    private void Patrol()
    {
        if (_enemy.Agent.remainingDistance > _enemy.Agent.stoppingDistance)
        {
            return;
        }
        _enemy.IndexDestination = _enemy.IndexDestination == _enemy.WayPoints.Count - 1 ? 0 : _enemy.IndexDestination + 1;
        _owner.GetComponent<AIStateMachine>().ChangeState(new SearchingBehavior());
        // _enemy.ChangeState(new SearchOnPlaceState(_enemy)); 
    }
}
