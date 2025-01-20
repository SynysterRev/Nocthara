using UnityEngine;

[CreateAssetMenu(fileName = "Patrol Behavior", menuName = "Scriptable Objects/Patrol Behavior")]
public class PatrolBehavior : AIBehavior
{
    public override void Initialize(GameObject npc)
    {
        base.Initialize(npc);
        if (_enemy)
        {
            _enemy.Agent.SetDestination(_enemy.WayPoints[_enemy.IndexDestination]);
            _enemy.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public override void ExecuteBehaviour()
    {
        if (!_enemy) return;
        Patrol();
    }

    public override void ExitBehaviour()
    {
        _enemy.GetComponent<SpriteRenderer>().color = Color.green;
    }

    private void Patrol()
    {
        if (_enemy.Agent.remainingDistance > _enemy.Agent.stoppingDistance)
        {
            return;
        }

        _enemy.IndexDestination = _enemy.IndexDestination == _enemy.WayPoints.Count - 1 ? 0 : _enemy.IndexDestination + 1;
        _owner.GetComponent<AIStateMachine>().ChangeState("Searching");
        // _enemy.ChangeState(new SearchOnPlaceState(_enemy)); 
    }
}
