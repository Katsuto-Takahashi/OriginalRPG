using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public partial class PlayerControllerCC : MonoBehaviour
{
    /// <summary>歩く速さ</summary>
    [SerializeField] float m_walkingSpeed = 7f;
    /// <summary>回転の速さ</summary>
    [SerializeField] float m_rotatingSpeed = 10f;
    /// <summary>ジャンプ力</summary>
    [SerializeField] float m_jumpingPower = 5f;
    /// <summary>重力</summary>
    [SerializeField]float m_gravityScale = 1f;

    CharacterController m_characterController;
    Animator m_animator;

    ActionStateBaseCC currentState;
    IdleStateCC idleState = new IdleStateCC();
    WalkStateCC walkState = new WalkStateCC();
    JumpStateCC jumpState = new JumpStateCC();
    FallStateCC fallState = new FallStateCC();
    LandStateCC landState = new LandStateCC();

    /// <summary>現在のベクトル</summary>
    Vector3 m_currentVelocity = Vector3.zero;
    /// <summary>プレイヤーのtransfom</summary>
    Transform m_nowTransform;
    /// <summary>入力方向のベクトル</summary>
    Vector3 m_direction;
    /// <summary>プレイヤーの回転に使用</summary>
    Quaternion m_targetRotation;
    /// <summary>メインカメラを基準にdirectionを変換する</summary>
    Vector3 m_moveForward = Vector3.zero;

    void OnEnable()
    {
        m_nowTransform = this.transform;
        var dir = m_nowTransform.forward;
        dir.y = 0f;
        m_targetRotation = Quaternion.LookRotation(dir);
    }

    void Start()
    {
        //m_nowTransform = this.transform;

        m_characterController = GetComponent<CharacterController>();
        m_animator = GetComponentInChildren<Animator>();

        ChangeState(idleState);
    }

    void Update()
    {
        m_moveForward = Camera.main.transform.TransformDirection(m_direction);

        ApplyInputAxis();
        ApplyGravity();
        ApplyMovement();
        ApplyRotation();

        currentState.OnUpdate(this);
    }

    void ApplyInputAxis()
    {
        //入力を保存
        float v = Input.GetAxis("Lstick_v");
        float h = Input.GetAxis("Lstick_h");
        //入力方向のベクトル
        //direction = new Vector3(h, 0, v);//これだとカメラの向きによっては傾いたり、浮いたりしてしまう
        m_direction = Vector3.forward * v + Vector3.right * h;
    }

    void ApplyMovement()
    {
        //プレイヤーの移動
        Vector3 velocity = Vector3.Scale(m_currentVelocity, new Vector3(m_walkingSpeed, 1f, m_walkingSpeed));
        m_characterController.Move(Time.deltaTime * velocity);
    }

    void ApplyRotation()
    {
        //プレイヤーの回転
        m_nowTransform.rotation = Quaternion.Slerp(this.transform.rotation, m_targetRotation, Time.deltaTime * m_rotatingSpeed);
    }

    void ApplyGravity()
    {
        if (!m_characterController.isGrounded)
        {
            m_currentVelocity.y += m_gravityScale * Physics.gravity.y * Time.deltaTime;
        }
    }

    void PlayAnimation(string stateName, float transitionDuration = 0.1f)
    {
        m_animator.CrossFadeInFixedTime(stateName, transitionDuration);
    }

    void ChangeState(ActionStateBaseCC nextState)
    {
        currentState?.OnExit(this);
        nextState.OnEnter(this);
        currentState = nextState;
    }
}
