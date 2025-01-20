using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Searching", menuName = "Scriptable Objects/Searching")]
public class Searching : AIBehavior
{
    public override void ExecuteBehaviour()
    {
        if (_enemy)
        {
            _enemy.StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        _enemy.GetComponent<AIStateMachine>().ChangeState("Patrol");
    }

    public override void ExitBehaviour()
    {
    }
}
