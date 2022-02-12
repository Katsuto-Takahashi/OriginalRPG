using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveParameters
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
