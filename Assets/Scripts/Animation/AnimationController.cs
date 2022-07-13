using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : SingletonMonoBehaviour<AnimationController>
{
    public bool FinishedAnimation(Animator animator, int layer = 0)
    {
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(layer);
        if (animatorStateInfo.loop) return false;
        return animatorStateInfo.normalizedTime > 1f;
    }
    public void PlayAnimation(Animator animator, string stateName, float transitionDuration = 0.1f)
    {
        animator.CrossFadeInFixedTime(stateName, transitionDuration);
    }
}
