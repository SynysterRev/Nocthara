using UnityEngine;

public class Tools
{
    public static float GetAnimationLength(Animator animator, string animationName)
    {
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;	//Get Animator controller
        foreach (var t in ac.animationClips)
        {
            if(t.name == animationName)            //If it has the same name as your clip
            {
                return t.length;
            }
        }

        return 0.0f;
    }
}
