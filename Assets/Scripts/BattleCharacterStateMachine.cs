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
    /// <summary>戦闘中かどうか</summary>
    bool m_isBattle;
    /// <summary>対象との距離</summary>
    float m_distance;
    /// <summary>現在の待機時間</summary>
    float m_currentTimer;

    #region パラメーター
    /// <summary>現在のHP</summary>
    int m_nowHP;
    /// <summary>最大のHP</summary>
    int m_hp;
    /// <summary>現在のAP</summary>
    int m_nowAP;
    /// <summary>最大のAP</summary>
    int m_ap;
    /// <summary>移動速度</summary>
    float m_moveSpeed;
    /// <summary>待機時間</summary>
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
    public void SetUp(Animator animator, CharacterParameterManager cp, Parameters param)
    {
        m_isBattle = false;
        SetAnim(animator);
        SetCharaParam(cp);
        SetParam(param);
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

    void SetCharaParam(CharacterParameterManager cpm)
    {
        m_nowHP = cpm.NowHP;
        m_nowAP = cpm.NowAP;
        m_hp = cpm.MaxHP;
        m_ap = cpm.MaxAP;
        m_actionTimer = (1000 - cpm.Speed) / 100f;
    }

    void SetParam(Parameters param)
    {
        m_moveSpeed = (param.WalkingSpeed + param.RunningSpeed) / 2f;
    }

    void Timer()
    {
        m_currentTimer += Time.deltaTime;
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
