using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(HasSkillList))]
[RequireComponent(typeof(MovementEnemyStateMachine), typeof(BattleEnemyStateMachine))]
public class Enemy : EnemyParameter, ITakableDamage
{
    /// <summary>自分のTransform</summary>
    protected Transform m_myTransform;

    [SerializeField]
    [Tooltip("移動に必要なパラメーター")]
    protected MoveParameters m_param = new MoveParameters();

    protected Animator m_animator;
    protected Rigidbody m_rigidbody;
    protected CapsuleCollider m_capsuleCollider;
    //protected SphereCollider m_sphereCollider;
    protected HasSkillList m_hsl;
    protected MovementEnemyStateMachine m_mesm;
    protected BattleEnemyStateMachine m_besm;

    public BattleEnemyStateMachine BESM => m_besm;

    void Awake()
    {
        m_myTransform = transform;

        m_hsl = GetComponent<HasSkillList>();
        m_animator = GetComponentInChildren<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_capsuleCollider = GetComponent<CapsuleCollider>();
        m_mesm = GetComponent<MovementEnemyStateMachine>();
        m_besm = GetComponent<BattleEnemyStateMachine>();
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

    protected virtual void OnUpdate() { }

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
}

[System.Serializable]
public class Parameter
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