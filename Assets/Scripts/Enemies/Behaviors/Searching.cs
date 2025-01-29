using System.Collections;
using UnityEngine;

public class SearchingBehavior : AIBehavior
{
    private Coroutine _searchingCoroutine;
    public override void Initialize(GameObject npc)
    {
        base.Initialize(npc);
        if (_aiStateMachine && _aiStateMachine.Animator)
        {
            _aiStateMachine.Animator.SetBool("IsWalking", false);
            _aiStateMachine.Animator.SetFloat("MoveX", _enemy.LastDirection.x);
            _aiStateMachine.Animator.SetFloat("MoveY", _enemy.LastDirection.y);
        }
        if (_enemy)
        {
            _searchingCoroutine = _enemy.StartCoroutine(Wait());
        }

    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.0f);
        _enemy.GetComponent<AIStateMachine>().ChangeState(new PatrolBehavior());
    }

    public override void ExitBehaviour()
    {
        if (_searchingCoroutine != null)
        {
            _enemy.StopCoroutine(_searchingCoroutine);
        }
    }
}
