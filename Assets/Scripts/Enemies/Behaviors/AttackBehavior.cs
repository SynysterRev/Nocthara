using UnityEngine;

public class AttackBehavior : AIBehavior
{
    private float _animTime = 0.0f;
    private float _timer = 0.0f;

    public override void Initialize(GameObject npc)
    {
        base.Initialize(npc);
        _animTime = new Tools().GetAnimationLength(_aiStateMachine.Animator, "attack");
        if (_animTime <= 0.0f)
        {
            _animTime = 1.5f;
        }

        _timer = 0.0f;
    }

    public override void ExecuteBehaviour()
    {
        _timer += Time.deltaTime;
        if (_timer >= _animTime)
        {
            _aiStateMachine.ChangeState(new ChaseBehaviour());
        }
    }

    public override void ExitBehaviour()
    {
    }
}