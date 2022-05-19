using System.Collections.Generic;
using UnityEngine;
using UniRx;
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
        Bind,
        NoBattle,
        Dead
    }

    Animator m_animator;

    /// <summary>戦闘中かどうか</summary>
    bool m_isBattle = false;
    /// <summary>戦闘中かのフラグ</summary>
    public bool IsBattle { get => m_isBattle; set => m_isBattle = value; }

    /// <summary>死亡しているかどうか</summary>
    bool m_isDead = false;
    /// <summary>死亡しているかのフラグ</summary>
    public bool IsDead { get => m_isDead; set => m_isDead = value; }

    /// <summary>入力による動きを止めるかどうか</summary>
    BoolReactiveProperty m_isStop = new BoolReactiveProperty(false);
    /// <summary>入力による動きを止めるかのフラグ</summary>
    public BoolReactiveProperty IsStop => m_isStop;

    /// <summary>バインドされているかどうか</summary>
    bool m_isBind = false;
    /// <summary>バインドされているかのフラグ</summary>
    public bool IsBind { get => m_isBind; set => m_isBind = value; }

    /// <summary>選択できるかどうか</summary>
    BoolReactiveProperty m_canSelect = new BoolReactiveProperty(false);
    /// <summary>選択できるかのフラグ</summary>
    public BoolReactiveProperty CanSelect => m_canSelect;

    /// <summary>対象との距離</summary>
    float m_distance;
    /// <summary>現在の待機時間</summary>
    float m_currentTimer;
    /// <summary>行動の対象</summary>
    List<GameObject> m_targets = new List<GameObject>();
    /// <summary>行動の対象のList</summary>
    public List<GameObject> Targets => m_targets;
    /// <summary>対象のPosition</summary>
    Vector3 m_targetPosition;
    /// <summary>行動回数</summary>
    int m_actionCount;
    /// <summary>蓄積可能な行動回数</summary>
    int m_maxActionCount;

    #region パラメーター
    /// <summary>現在のHP</summary>
    int m_nowHP;
    /// <summary>最大のHP</summary>
    int m_hp;
    /// <summary>現在のAP</summary>
    int m_nowAP;
    /// <summary>最大のAP</summary>
    int m_ap;
    /// <summary>物理攻撃力</summary>
    int m_strength;
    /// <summary>物理防御力</summary>
    int m_defense;
    /// <summary>魔法攻撃力</summary>
    int m_magicPower;
    /// <summary>魔法防御力</summary>
    int m_magicResist;


    /// <summary>移動速度</summary>
    float m_moveSpeed;
    /// <summary>待機時間</summary>
    float m_actionTimer;
    #endregion

    /// <summary>通常攻撃</summary>
    List<SkillData> m_normalSkill = new List<SkillData>();
    /// <summary>スキル</summary>
    List<SkillData> m_skillDatas = new List<SkillData>();
    /// <summary>魔法</summary>
    List<SkillData> m_magicDatas = new List<SkillData>();
    /// <summary>選択した攻撃</summary>
    SkillData m_selectSkill;

    void SetState()
    {
        m_stateMachine = new StateMachine<BattleCharacterStateMachine>(this);

        m_stateMachine.AddAnyTransition<BattleCharacterState.Wait>((int)ActEvent.Wait);
        m_stateMachine.AddAnyTransition<BattleCharacterState.Standby>((int)ActEvent.Standby);
        m_stateMachine.AddAnyTransition<BattleCharacterState.Move>((int)ActEvent.Move);
        m_stateMachine.AddAnyTransition<BattleCharacterState.BattleAction>((int)ActEvent.BattleAction);
        m_stateMachine.AddAnyTransition<BattleCharacterState.ActionEnd>((int)ActEvent.ActionEnd);
        m_stateMachine.AddAnyTransition<BattleCharacterState.Bind>((int)ActEvent.Bind);
        m_stateMachine.AddAnyTransition<BattleCharacterState.NoBattle>((int)ActEvent.NoBattle);
        m_stateMachine.AddAnyTransition<BattleCharacterState.Dead>((int)ActEvent.Dead);
        
        m_stateMachine.Start<BattleCharacterState.NoBattle>();
    }

    public void SetUp(Animator animator, MoveParameters param)
    {
        SetAnim(animator);
        SetMoveParam(param);
        SetState();
    }

    public void OnUpdate()
    {
        m_stateMachine.Update();

        Timer(m_stateMachine.CurrentSate);
    }

    void SetAnim(Animator animator)
    {
        m_animator = animator;
    }

    public void SetParam(Character character)
    {
        SetCharaParam(character);
    }

    void SetCharaParam(Character character)
    {
        m_nowHP = character.HP.Value;
        m_nowAP = character.AP.Value;

        m_hp = character.MaxHP.Value;
        m_ap = character.MaxAP.Value;
        m_strength = character.Strength.Value;
        m_defense = character.Defense.Value;
        m_magicPower = character.MagicPower.Value;
        m_magicResist = character.MagicResist.Value;

        m_actionTimer = (1000 - character.Speed.Value) / 100f;
        m_maxActionCount = character.MaxActionCount;
    }

    void SetMoveParam(MoveParameters param)
    {
        m_moveSpeed = (param.WalkingSpeed + param.RunningSpeed) / 2f;
    }

    //void SetSkill(HasSkillList hasSkill)
    //{
    //    for (int i = 0; i < hasSkill.NormalSkill.Count; i++)
    //    {
    //        m_normalSkill.Add(hasSkill.NormalSkill[i]);
    //    }
    //    GetSkill(hasSkill);
    //}

    //void GetSkill(HasSkillList hasSkill)
    //{
    //    for (int i = 0; i < hasSkill.SkillDatas.Count; i++)
    //    {
    //        m_skillDatas.Add(hasSkill.SkillDatas[i]);
    //    }
    //    for (int i = 0; i < hasSkill.MagicDatas.Count; i++)
    //    {
    //        m_magicDatas.Add(hasSkill.MagicDatas[i]);
    //    }
    //}

    void Timer(State state)
    {
        if (state is BattleCharacterState.Wait || state is BattleCharacterState.Standby)
        {
            if (m_actionCount < m_maxActionCount)
            {
                m_currentTimer += Time.deltaTime;
            }
            if (m_currentTimer > m_actionTimer)
            {
                m_currentTimer -= m_actionTimer;
                m_actionCount++;
            }
        }
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
