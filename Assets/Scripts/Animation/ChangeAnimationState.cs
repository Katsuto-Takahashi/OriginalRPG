using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAnimationState : MonoBehaviour
{
    Animator animator;

    AnimatorOverrideController controller;

    string overrideClipName = "Effect";

    public void SetAnim()
    {
        animator = GetComponent<Animator>();
        controller = new AnimatorOverrideController();
        controller.runtimeAnimatorController = animator.runtimeAnimatorController;
        animator.runtimeAnimatorController = controller;
    }

    void ChangeClip(AnimationClip clip)
    {
        Debug.Log("ïœçXÇ∑ÇÈ");
        AnimatorStateInfo[] layerInfo = new AnimatorStateInfo[animator.layerCount];
        for (int i = 0; i < animator.layerCount; i++)
        {
            layerInfo[i] = animator.GetCurrentAnimatorStateInfo(i);
        }

        controller[overrideClipName] = clip;
        animator.Update(0.0f);

        for (int i = 0; i < animator.layerCount; i++)
        {
            //animator.Play(layerInfo[i].nameHash, i, layerInfo[i].normalizedTime);
            animator.Play(layerInfo[i].fullPathHash, i, layerInfo[i].normalizedTime);
        }
    }
}
