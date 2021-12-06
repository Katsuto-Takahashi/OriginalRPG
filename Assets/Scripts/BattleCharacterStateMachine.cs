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

    bool m_isBattle;

    #region パラメーター
    int m_nowHP;
    int m_hp;
    int m_nowAP;
    int m_ap;
    float m_currentTimer;
    float m_actionTimer;
    #endregion

    void SetState()
    {
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
    public void SetUp(Animator animator, CharacterParameterManager cp)
    {
        m_isBattle = false;
        SetAnim(animator);
        SetParam(cp);
        SetState();
    }
    public void OnUpdate()
    {
        m_stateMachine.Update();
    }
    void SetAnim(Animator animator)
    {
        m_animator = animator;
    }

    void SetParam(CharacterParameterManager cpm)
    {
        m_nowHP = cpm.NowHP;
        m_nowAP = cpm.NowAP;
        m_hp = cpm.MaxHP;
        m_ap = cpm.MaxAP;
        m_actionTimer = SetTime(cpm);
    }

    float SetTime(CharacterParameterManager cpm)
    {
        return (1000 - cpm.Speed) / 100f;
    }

    bool FinishedAnimation(int layer = 0)
    {
        return AnimationController.Instance.FinishedAnimation(m_animator, layer);
    }
    void PlayAnimation(string stateName, float transitionDuration = 0.1f)
    {
        AnimationController.Instance.PlayAnimation(m_animator, stateName, transitionDuration);
    }
}
