using System.Collections;
using UnityEngine;
using State = StateMachine<BattleCharacterStateMachine>.State;

public partial class BattleCharacterStateMachine : MonoBehaviour
{
    StateMachine<BattleCharacterStateMachine> m_stateMachine;
    enum ActEvent : byte
    {
        Wait,
        Standby,
        Move,
        BattleAction,
        ActionEnd,
        NoBattle,
        Dead
    }
    Animator m_animator;

    void SetState()
    {
        //m_animator = GetComponent<Animator>();

        m_stateMachine = new StateMachine<BattleCharacterStateMachine>(this);

        m_stateMachine.AddAnyTransition<BattleCharacterState.Wait>((int)ActEvent.Wait);
        m_stateMachine.AddAnyTransition<BattleCharacterState.Standby>((int)ActEvent.Standby);
        m_stateMachine.AddAnyTransition<BattleCharacterState.Move>((int)ActEvent.Move);
        m_stateMachine.AddAnyTransition<BattleCharacterState.BattleAction>((int)ActEvent.BattleAction);
        m_stateMachine.AddAnyTransition<BattleCharacterState.ActionEnd>((int)ActEvent.ActionEnd);
        m_stateMachine.AddAnyTransition<BattleCharacterState.NoBattle>((int)ActEvent.NoBattle);
        m_stateMachine.AddAnyTransition<BattleCharacterState.Dead>((int)ActEvent.Dead);
        
        m_stateMachine.Start<BattleCharacterState.Wait>();
    }
    public void SetUp(Animator animator)
    {
        SetAnim(animator);
        SetState();
    }
    //void Update()
    //{
    //    m_stateMachine.Update();
    //}
    public void OnUpdate()
    {
        m_stateMachine.Update();
    }
    void SetAnim(Animator animator)
    {
        m_animator = animator;
    }

    bool FinishedAnimation(int layer = 0)
    {
        return AnimationController.Instance.FinishedAnimation(m_animator, layer);
        /*AnimatorStateInfo animatorStateInfo = m_animator.GetCurrentAnimatorStateInfo(layer);
        if (animatorStateInfo.loop) return false;
        return animatorStateInfo.normalizedTime > 1f;*/
    }
    void PlayAnimation(string stateName, float transitionDuration = 0.1f)
    {
        AnimationController.Instance.PlayAnimation(m_animator, stateName, transitionDuration);
        //m_animator.CrossFadeInFixedTime(stateName, transitionDuration);
    }
}
