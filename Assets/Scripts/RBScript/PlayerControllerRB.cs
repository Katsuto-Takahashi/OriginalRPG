using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public partial class PlayerControllerRB : MonoBehaviour
{
    /// <summary>歩く速さ</summary>
    [SerializeField] float m_walkingSpeed = 7f;
    /// <summary>回転の速さ</summary>
    [SerializeField] float m_rotatingSpeed = 10f;
    /// <summary>ジャンプ力</summary>
     [SerializeField] float m_jumpingPower = 50f;
    /// <summary>接地判定での中心からの距離</summary>
     [SerializeField] float m_isGroundLength = 0.05f;
    /// <summary>重力</summary>
    [SerializeField] float m_gravityScale = 1f;
    /// <summary>地面のレイヤー</summary>
    public LayerMask m_groundLayer;

    Rigidbody m_rigidbody;
    Animator m_animator;

    ActionStateBaseRB currentState;
    IdleStateRB idleState = new IdleStateRB();
    WalkStateRB walkState = new WalkStateRB();
    JumpStateRB jumpState = new JumpStateRB();
    FallStateRB fallState = new FallStateRB();
    LandStateRB landState = new LandStateRB();

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

    void Start()
    {
        m_nowTransform = this.transform;

        m_rigidbody = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();

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
        m_rigidbody.velocity = velocity;
    }

    void ApplyRotation()
    {
        //プレイヤーの回転
        m_nowTransform.rotation = Quaternion.Slerp(this.transform.rotation, m_targetRotation, Time.deltaTime * m_rotatingSpeed);
    }

    /// <summary>接地判定</summary>
    /// <returns>判定結果</returns>
    bool IsGround()
    {
        Vector3 start = this.transform.position;
        Vector3 end = start + Vector3.down * m_isGroundLength;
        Debug.DrawLine(start, end);
        bool isGround = Physics.Linecast(start, end,m_groundLayer);
        return isGround;
    }
    void ApplyGravity()
    {
        if (IsGround())
        {
            m_currentVelocity.y = 0f;
        }
    }
    void PlayAnimation(string stateName, float transitionDuration = 0.1f)
    {
        m_animator.CrossFadeInFixedTime(stateName, transitionDuration);
    }
    void ChangeState(ActionStateBaseRB nextState)
    {
        currentState?.OnExit(this);
        nextState.OnEnter(this);
        currentState = nextState;
    }
}
