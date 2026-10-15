using UnityEngine;

public class AnimationModule : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    public void PlayAnimation(int animationHash, float transitionDuration = 0.05f)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool isTransitioning = animator.IsInTransition(0);

        if (isTransitioning && animator.GetNextAnimatorStateInfo(0).shortNameHash == animationHash)
        {
            return;
        }

        if (!isTransitioning && currentInfo.shortNameHash == animationHash)
        {
            return;
        }

        animator.CrossFadeInFixedTime(animationHash, transitionDuration);
    }

    public bool IsAnimationFinished(int animationHash)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.shortNameHash == animationHash)
        {
            return stateInfo.normalizedTime >= 0.95f && !animator.IsInTransition(0);
        }

        return false;
    }
}
