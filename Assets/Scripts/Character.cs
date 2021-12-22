using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Character : CharacterParameter, ITakableDamage
{
    /// <summary>自分のTransform</summary>
    protected Transform m_myTransform;

    [SerializeField]
    [Tooltip("移動に必要なパラメーター")]
    protected Parameters m_param = new Parameters();

    protected HasSkillList m_hsl;
    protected Animator m_animator;
    protected Rigidbody m_rigidbody;
    protected CapsuleCollider m_capsuleCollider;

    void Awake()
    {
        m_myTransform = transform;

        m_hsl = GetComponent<HasSkillList>();
        m_animator = GetComponentInChildren<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_capsuleCollider = GetComponent<CapsuleCollider>();
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
}

[Serializable]
public class Parameters
{
    [SerializeField]
    [Tooltip("回転速度")]
    float m_rotatingSpeed = 50f;

    [SerializeField]
    [Tooltip("歩く速度")]
    float m_walkingSpeed = 10f;

    [SerializeField]
    [Tooltip("走る速度")]
    float m_runningSpeed = 15f;

    [SerializeField]
    [Tooltip("ジャンプ力")]
    float m_jumpingPower = 5f;

    [SerializeField]
    [Tooltip("重力の大きさ")]
    float m_gravityScale = 0.5f;

    [SerializeField]
    [Tooltip("接地判定に使用するRayの長さ")]
    float m_isGroundLength = 0.76f;

    [SerializeField]
    [Tooltip("地面のレイヤー")]
    LayerMask m_groundLayer = 0;

    [SerializeField]
    [Tooltip("坂のレイヤー")]
    LayerMask m_slopeLayer = 0;

    /// <summary>回転速度</summary>
    public float RotatingSpeed => m_rotatingSpeed;

    /// <summary>歩く速度</summary>
    public float WalkingSpeed => m_walkingSpeed;

    /// <summary>走る速度</summary>
    public float RunningSpeed => m_runningSpeed;

    /// <summary>ジャンプ力</summary>
    public float JumpingPower => m_jumpingPower;

    /// <summary>重力の大きさ</summary>
    public float GravityScale => m_gravityScale;

    /// <summary>接地判定に使用するRayの長さ</summary>
    public float IsGroundLength => m_isGroundLength;

    /// <summary>地面のレイヤー</summary>
    public LayerMask GroundLayer => m_groundLayer;

    /// <summary>坂のレイヤー</summary>
    public LayerMask SlopeLayer => m_slopeLayer;
}
