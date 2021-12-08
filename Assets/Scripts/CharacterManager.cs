using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(MovementCharacterStateMachine), typeof(BattleCharacterStateMachine))]
public class CharacterManager : MonoBehaviour
{
    [Tooltip("自分のTransform")]
    Transform m_myTransform;
    [SerializeField, Tooltip("情報")]
    Parameters m_param = new Parameters();

    bool m_isBattle;
    bool m_donotMove;

    CharacterParameterManager m_cpm;
    BattleCharacterStateMachine m_bcsm;
    MovementCharacterStateMachine m_mcsm;
    HasSkillList m_hsl;

    Animator m_animator;
    Rigidbody m_rigidbody;
    CapsuleCollider m_capsuleCollider;

    void Start()
    {
        m_myTransform = transform;

        m_animator = GetComponentInChildren<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_capsuleCollider = GetComponent<CapsuleCollider>();

        //m_cp = GetComponent<CharacterParameterManager>();
        m_mcsm = GetComponent<MovementCharacterStateMachine>();
        m_bcsm = GetComponent<BattleCharacterStateMachine>();
        m_hsl = GetComponent<HasSkillList>();

        m_mcsm.SetUp(m_animator, m_rigidbody, m_myTransform, m_param, m_capsuleCollider);
        //m_bcsm.SetUp(m_animator, m_cpm, m_param);
    }

    void Update()
    {
        ApplyGetAxis();
        m_mcsm.OnUpdate();
        //m_bcsm.OnUpdate();
    }

    void FixedUpdate()
    {
        m_mcsm.OnFixedUpdate();
    }

    void ApplyGetAxis()
    {
        float h = Input.GetAxis("Lstick_h");
        float v = Input.GetAxis("Lstick_v");
        m_mcsm.UserInput(h, v);
    }
}

[System.Serializable]
public class Parameters
{
    [SerializeField, Tooltip("回転速度")]
    float m_rotatingSpeed = 0f;

    [SerializeField, Tooltip("歩く速度")]
    float m_walkingSpeed = 0f;

    [SerializeField, Tooltip("走る速度")]
    float m_runningSpeed = 0f;

    [SerializeField, Tooltip("ジャンプ力")]
    float m_jumpingPower = 0f;

    [SerializeField, Tooltip("重力の大きさ")]
    float m_gravityScale = 0f;

    [SerializeField, Tooltip("接地判定に使用するRayの長さ")]
    float m_isGroundLength = 0f;

    [SerializeField, Tooltip("地面のレイヤー")]
    LayerMask m_groundLayer = 0;

    [SerializeField, Tooltip("坂のレイヤー")]
    LayerMask m_slopeLayer = 0;

    public float RotatingSpeed => m_rotatingSpeed;
    public float WalkingSpeed => m_walkingSpeed;
    public float RunningSpeed => m_runningSpeed;
    public float JumpingPower => m_jumpingPower;
    public float GravityScale => m_gravityScale;
    public float IsGroundLength => m_isGroundLength;
    public LayerMask GroundLayer => m_groundLayer;
    public LayerMask SlopeLayer => m_slopeLayer;
}
