using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using BehaviorTree;
using State = StateMachine<BattleEnemyStateMachine>.State;

public partial class BattleEnemyStateMachine : MonoBehaviour
{
    StateMachine<BattleEnemyStateMachine> m_stateMachine;

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
    [SerializeReference, SubclassSelector]
    Node m_childNode;

    /// <summary>戦闘中かどうか</summary>
    bool m_isBattle = false;
    /// <summary>戦闘中かのフラグ</summary>
    public bool IsBattle { get => m_isBattle; set => m_isBattle = value; }

    /// <summary>死亡しているかどうか</summary>
    bool m_isDead = false;
    /// <summary>死亡しているかのフラグ</summary>
    public bool IsDead { get => m_isDead; set => m_isDead = value; }

    /// <summary>動きを止めるかどうか</summary>
    BoolReactiveProperty m_stop = new BoolReactiveProperty(false);
    /// <summary>動きを止めるかのフラグ</summary>
    public BoolReactiveProperty Stop => m_stop;

    /// <summary>バインドされているかどうか</summary>
    bool m_isBind = false;
    /// <summary>バインドされているかのフラグ</summary>
    public bool IsBind { get => m_isBind; set => m_isBind = value; }

    /// <summary>動かすかどうか</summary>
    BoolReactiveProperty m_move = new BoolReactiveProperty(false);
    /// <summary>動かすかのフラグ</summary>
    public BoolReactiveProperty Move  => m_move;

    /// <summary>注目するかどうか</summary>
    BoolReactiveProperty m_look = new BoolReactiveProperty(false);
    /// <summary>注目するかのフラグ</summary>
    public BoolReactiveProperty Look  => m_look;

    /// <summary>対象との距離</summary>
    float m_distance;
    /// <summary>現在の待機時間</summary>
    float m_currentTimer;

    /// <summary>行動の対象List</summary>
    List<GameObject> m_targets = new List<GameObject>();
    /// <summary>行動の対象List</summary>
    public List<GameObject> Targets => m_targets;

    /// <summary>行動の対象</summary>
    GameObject m_targetObject;
    /// <summary>行動の対象</summary>
    public GameObject TargetObject => m_targetObject;

    /// <summary>対象のIndex</summary>
    int m_targetIndex;
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
    /// <summary>賢さ</summary>
    int m_intelligence;


    /// <summary>移動速度</summary>
    float m_moveSpeed;
    /// <summary>待機時間</summary>
    float m_actionTimer;
    #endregion

    /// <summary>通常攻撃</summary>
    List<OldSkillData> m_normalSkill = new List<OldSkillData>();
    /// <summary>スキル</summary>
    List<OldSkillData> m_skillDatas = new List<OldSkillData>();
    /// <summary>魔法</summary>
    List<OldSkillData> m_magicDatas = new List<OldSkillData>();

    void SetState()
    {
        m_stateMachine = new StateMachine<BattleEnemyStateMachine>(this);

        m_stateMachine.AddAnyTransition<BattleEnemyState.Wait>((int)ActEvent.Wait);
        m_stateMachine.AddAnyTransition<BattleEnemyState.Standby>((int)ActEvent.Standby);
        m_stateMachine.AddAnyTransition<BattleEnemyState.Move>((int)ActEvent.Move);
        m_stateMachine.AddAnyTransition<BattleEnemyState.BattleAction>((int)ActEvent.BattleAction);
        m_stateMachine.AddAnyTransition<BattleEnemyState.ActionEnd>((int)ActEvent.ActionEnd);
        m_stateMachine.AddAnyTransition<BattleEnemyState.Bind>((int)ActEvent.Bind);
        m_stateMachine.AddAnyTransition<BattleEnemyState.NoBattle>((int)ActEvent.NoBattle);
        m_stateMachine.AddAnyTransition<BattleEnemyState.Dead>((int)ActEvent.Dead);

        m_stateMachine.Start<BattleEnemyState.NoBattle>();
    }

    void SetParam(Enemy enemy)
    {
        m_nowHP = enemy.HP.Value;
        m_hp = enemy.MaxHP.Value;
        m_nowAP = enemy.AP.Value;
        m_ap = enemy.MaxAP.Value;
        m_strength = enemy.Strength.Value;
        m_defense = enemy.Defense.Value;
        m_magicPower = enemy.MagicPower.Value;
        m_magicResist = enemy.MagicResist.Value;
        m_intelligence = enemy.Intelligence.Value;

        m_actionTimer = (1000 - enemy.Speed.Value) / 100f;
        m_maxActionCount = enemy.MaxActionCount;
    }

    public void Parameter(Enemy enemy)
    {
        SetParam(enemy);
    }

    public void SetUP(Animator animator, HasSkillList hasSkill, MoveParameters param)
    {
        m_animator = animator;
        m_childNode.GetNode();
        SetState();
        SetSkill(hasSkill);
        SetParam(param);
    }

    public void OnUpdate()
    {
        m_stateMachine.Update();

        Timer(m_stateMachine.CurrentSate);
    }

    void Timer(State state)
    {
        if (state is BattleEnemyState.Wait || state is BattleEnemyState.Standby)
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

    void SetParam(MoveParameters param)
    {
        m_moveSpeed = (param.WalkingSpeed + param.RunningSpeed) / 2f;
    }

    void SetSkill(HasSkillList hasSkill)
    {
        for (int i = 0; i < hasSkill.NormalSkill.Count; i++)
        {
            m_normalSkill.Add(hasSkill.NormalSkill[i]);
        }
        GetSkill(hasSkill);
    }

    void GetSkill(HasSkillList hasSkill)
    {
        for (int i = 0; i < hasSkill.SkillDatas.Count; i++)
        {
            m_skillDatas.Add(hasSkill.SkillDatas[i]);
        }
        for (int i = 0; i < hasSkill.MagicDatas.Count; i++)
        {
            m_magicDatas.Add(hasSkill.MagicDatas[i]);
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

    //後に変更予定
    public void StateChenge(int actEvent)
    {
        m_stateMachine.Dispatch(actEvent);
    }
}
