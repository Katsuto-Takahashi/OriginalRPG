using System.Collections;
using UnityEngine;
using State = StateMachine<CharacterStateMachine>.State;

public partial class CharacterStateMachine : MonoBehaviour
{
    StateMachine<CharacterStateMachine> m_stateMachine;
    enum ActEvent : byte
    {
        Wait,
        Standby,
        Move,
        Action,
        ActionEnd,
        NoBattle,
        Dead
    }
    Animator m_animator;

    void Start()
    {
        m_animator = GetComponent<Animator>();

        m_stateMachine = new StateMachine<CharacterStateMachine>(this);

        m_stateMachine.AddAnyTransition<BattleCharacterState.WaitState>((int)ActEvent.Wait);
        m_stateMachine.AddAnyTransition<BattleCharacterState.StandbyState>((int)ActEvent.Standby);
        
        m_stateMachine.Start<BattleCharacterState.WaitState>();
    }

    void Update()
    {
        m_stateMachine.Update();
    }

    bool FinishedAnimation(int layer = 0)
    {
        AnimatorStateInfo animatorStateInfo = m_animator.GetCurrentAnimatorStateInfo(layer);
        if (animatorStateInfo.loop) return false;
        return animatorStateInfo.normalizedTime > 1f;
    }
    void PlayAnimation(string stateName, float transitionDuration = 0.1f)
    {
        m_animator.CrossFadeInFixedTime(stateName, transitionDuration);
    }
}
