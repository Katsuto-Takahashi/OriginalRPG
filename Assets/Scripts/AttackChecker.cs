using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackChecker : MonoBehaviour
{
    Animator animator;

    AnimatorOverrideController controller;

    string overrideClipName = "Effect";

    GameObject m_mine;

    Skill m_skillData;

    LayerMask m_target;

    public void SetAttackParam(GameObject mine, Skill skill, LayerMask targetLayer)
    {
        m_mine = mine;
        m_skillData = skill;
        m_target = targetLayer;
        ChangeClip(skill.SkillParameter.SkillAnimationClip);
        AnimationController.Instance.PlayAnimation(animator, overrideClipName);
    }

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

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & m_target) != 0)
        {
            other.GetComponent<ITakableDamage>().TakeDamage(BattleManager.Instance.Damage(m_mine, other.gameObject, m_skillData.SkillParameter));
        }
    }
}
