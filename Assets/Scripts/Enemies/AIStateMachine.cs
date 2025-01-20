using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class AIStateMachine : MonoBehaviour
{
    [SerializedDictionary("Behavior Name", "SO Behavior")]
    public SerializedDictionary<string, AIBehavior> Behaviors;
    private AIBehavior _currentState;

    private void Start()
    {
        _currentState = Behaviors["Patrol"];
        _currentState?.Initialize(gameObject);
    }

    private void Update()
    {
        _currentState?.ExecuteBehaviour();
    }

    public void ChangeState(string newStateName)
    {
        if (!Behaviors.ContainsKey(newStateName))
        {
            Debug.LogError($"State \"{newStateName}\" not found");
            return;
        }
        AIBehavior newState = Behaviors[newStateName];
        if (_currentState != newState)
        {
            _currentState?.ExitBehaviour();
            _currentState = newState;
            _currentState?.Initialize(gameObject);
        }
    }
}
