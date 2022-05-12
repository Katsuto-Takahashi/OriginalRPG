﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(HasSkillList))]
[RequireComponent(typeof(MovementCharacterStateMachine), typeof(BattleCharacterStateMachine))]
public class Character : CharacterParameter, ITakableDamage
{
    /// <summary>自分のTransform</summary>
    protected Transform m_myTransform;

    [SerializeField]
    [Tooltip("移動に必要なパラメーター")]
    protected MoveParameters m_param = new MoveParameters();

    protected Animator m_animator;
    protected Rigidbody m_rigidbody;
    protected CapsuleCollider m_capsuleCollider;
    protected HasSkillList m_hsl;
    protected MovementCharacterStateMachine m_mcsm;
    protected BattleCharacterStateMachine m_bcsm;

    public BattleCharacterStateMachine BCSM => m_bcsm;

    public MovementCharacterStateMachine MCSM => m_mcsm;

    bool m_isContact = false;
    public bool IsContact { get => m_isContact; set => m_isContact = value; }
    
    BoolReactiveProperty m_levelUP = new BoolReactiveProperty(false);
    public IReactiveProperty<bool> LevelUP => m_levelUP;

    void Awake()
    {
        m_myTransform = transform;

        m_hsl = GetComponent<HasSkillList>();
        m_animator = GetComponentInChildren<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_capsuleCollider = GetComponent<CapsuleCollider>();
        m_mcsm = GetComponent<MovementCharacterStateMachine>();
        m_bcsm = GetComponent<BattleCharacterStateMachine>();
    }

    void Start()
    {
        SetUp();
        Observable.EveryUpdate().Subscribe(_ => OnUpdate())
            .AddTo(this);
        Observable.EveryFixedUpdate().Subscribe(_ => OnFixedUpdate())
            .AddTo(this);
    }

    protected virtual void SetUp() { }

    protected virtual void OnUpdate()
    {
        if (HP.Value >= MaxHP.Value)
        {
            HP.Value = MaxHP.Value;
        }
        else if (HP.Value < 1)
        {
            HP.Value = 0;
        }
        if (AP.Value >= MaxAP.Value)
        {
            AP.Value = MaxAP.Value;
        }
        else if (AP.Value < 1)
        {
            AP.Value = 0;
        }
    }

    protected virtual void OnFixedUpdate() { }

    public virtual void TakeDamage(int damage)
    {
        if (HP.Value - damage < 1)
        {
            HP.Value = 0;
        }
        else
        {
            HP.Value -= damage;
        }
    }

    void LevelUp()
    {
        m_levelUP.Value = true;
        Level.Value++;
        CalculateNextExp(Level.Value);
        ParameterUp(Level.Value);
    }

    public void GetExp(int getExp)
    {
        CalculateExp(getExp);
    }

    void CalculateExp(int getExp)
    {
        NowExp.Value += getExp;
        TotalExp.Value += getExp;
        if (NextExp.Value <= NowExp.Value)
        {
            int exp = NextExp.Value - NowExp.Value;
            if (Level.Value != 100)
            {
                LevelUp();
                NowExp.Value -= exp;
            }
            else
            {
                NowExp.Value = 0;
            }
        }
    }

    void CalculateNextExp(int level)
    {
        if (level % 10 == 0)
        {
            m_levelCorrection *= 1.002f;
        }
        if (level - 10 <= 1)
        {
            NextExp.Value = (int)((1 * m_charaCorrection * level * level + level * 2) * m_levelCorrection);
        }
        else
        {
            NextExp.Value = (int)(((level - 10) * m_charaCorrection * level * level + level * 2) * m_levelCorrection);
        }
    }

    void ParameterUp(int level)//上昇の仕方は検討中
    {
        int baseParameter = level % 5 + 1;
        MaxHP.Value += baseParameter;
        HP.Value += baseParameter;
        MaxAP.Value += baseParameter;
        AP.Value += baseParameter;
        Strength.Value += baseParameter;
        Defense.Value += baseParameter;
        MagicPower.Value += baseParameter;
        MagicResist.Value += baseParameter;
        Speed.Value += baseParameter;
        Luck.Value++;
        SkillPoint.Value += baseParameter;
    }

    public void GetSkill(SkillData skillData)
    {
        if (skillData.attackType == SkillData.AttackType.physicalAttack)
        {
            m_hsl.SkillDatas.Add(skillData);
        }
        else if (skillData.attackType == SkillData.AttackType.magicAttack)
        {
            m_hsl.MagicDatas.Add(skillData);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & m_param.TargetLayer) != 0 && !m_isContact && !m_bcsm.IsBattle)
        {
            m_isContact = true;
            NewBattleManager.Instance.SetBattle(this, other.gameObject);
        }
    }
}
