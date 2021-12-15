using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class CharacterManager : MonoBehaviour
{
    /// <summary>自分のTransform</summary>
    protected Transform m_myTransform;

    [SerializeField]
    [Tooltip("移動に必要なパラメーター")]
    protected Parameters m_param = new Parameters();

    protected HasSkillList m_hsl;
    protected Character m_character;
    protected Animator m_animator;
    protected Rigidbody m_rigidbody;
    protected CapsuleCollider m_capsuleCollider;

    void Start()
    {
        SetUp();
        this.UpdateAsObservable().Subscribe(_ => OnUpdate());
        this.FixedUpdateAsObservable().Subscribe(_ => OnFixedUpdate());
    }

    protected virtual void SetUp() { }

    //void Update()
    //{
    //    OnUpdate();
    //}

    protected virtual void OnUpdate() { }

    //void FixedUpdate()
    //{
    //    OnFixedUpdate();
    //}

    protected virtual void OnFixedUpdate() { }
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
