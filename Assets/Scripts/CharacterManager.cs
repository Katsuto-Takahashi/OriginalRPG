using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField, Tooltip("回転速度")]
    float m_rotatingSpeed;

    [SerializeField, Tooltip("歩く速度")]
    float m_walkingSpeed;

    [SerializeField, Tooltip("走る速度")]
    float m_runningSpeed;

    [SerializeField, Tooltip("ジャンプ力")]
    float m_jumpingPower;

    [SerializeField, Tooltip("重力の大きさ")]
    float m_gravityScale;

    [SerializeField, Tooltip("接地判定に使用するRayの距離")]
    float m_isGroundLength;

    [SerializeField, Tooltip("地面のレイヤー")]
    LayerMask m_groundLayer;
    [Tooltip("自分のTransform")]
    Transform m_myTransform;

    BattleCharacterStateMachine m_csm;
    MovementCharacterStateMachine m_mcsm;
    Animator m_animator;
    Rigidbody m_rigidbody;

    void Start()
    {
        m_myTransform = transform;

        m_animator = GetComponentInChildren<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_mcsm = GetComponent<MovementCharacterStateMachine>();
        m_csm = GetComponent<BattleCharacterStateMachine>();

        m_mcsm.SetUp(m_animator
            , m_rigidbody
            , m_myTransform
            , m_rotatingSpeed
            , m_walkingSpeed
            , m_runningSpeed
            , m_jumpingPower
            , m_gravityScale
            , m_isGroundLength
            , m_groundLayer);
        m_csm.SetUp(m_animator);
    }

    void Update()
    {
        ApplyGetAxis();
        m_mcsm.OnUpdate();
        m_csm.OnUpdate();
    }

    void ApplyGetAxis()
    {
        float h = Input.GetAxis("");
        float v = Input.GetAxis("");
        m_mcsm.UserInput(h, v);
    }

    void ApplyRotation()
    {
        //プレイヤーの回転
        //m_myTransform.rotation = Quaternion.Slerp(this.transform.rotation, m_targetRotation, Time.deltaTime * m_rotatingSpeed);
    }
}
