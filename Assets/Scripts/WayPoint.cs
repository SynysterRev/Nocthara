using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WayPoint : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    public List<Vector2> WayPoints;
    
    private int _currentWayPointIndex = 0;
    private int _wayPointReachedIndex = -1;

    public Vector2 GetWayPoint()
    {
        if (_wayPointReachedIndex == _currentWayPointIndex)
        {
            _currentWayPointIndex = (_currentWayPointIndex + 1) % WayPoints.Count;
        }

        return WayPoints[_currentWayPointIndex];
    }

    public bool HasReachedWayPoint(NavMeshAgent agent)
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            _wayPointReachedIndex = _currentWayPointIndex;
            return true;
        }

        return false;
    }
}
