using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<MovementCharacterStateMachine>.State;

public partial class MovementCharacterStateMachine : MonoBehaviour
{
    StateMachine<MovementCharacterStateMachine> m_stateMachine;
    enum ActEvent : byte
    {
        Idole,
        Walk,
        Run,
        Jump,
        Fall,
        Land,
        Dead
    }

    Animator m_animator;
    Rigidbody m_rigidbody;
    /// <summary>入力の方向</summary>
    Vector3 m_inputDirection = Vector3.zero;
    /// <summary>プレイヤーのtransfom</summary>
    Transform m_myTransform;
    /// <summary>正面</summary>
    Vector3 m_moveForward;
    /// <summary>現在の速度</summary>
    Vector3 m_currentVelocity;
    /// <summary>目標のrotation</summary>
    Quaternion m_targetRotation;

    /// <summary>回転速度</summary>
    float m_rotatingSpeed;
    /// <summary>歩く速度</summary>
    float m_walkingSpeed;
    /// <summary>走る速度</summary>
    float m_runningSpeed;
    /// <summary>ジャンプ力</summary>
    float m_jumpingPower;
    /// <summary>重力</summary>
    float m_gravityScale;
    /// <summary>接地判定での中心からの距離</summary>
    float m_isGroundLength;
    /// <summary>地面のレイヤー</summary>
    LayerMask m_groundLayer;

    void SetState()
    {
        m_stateMachine = new StateMachine<MovementCharacterStateMachine>(this);

        m_stateMachine.AddAnyTransition<MovementCharacterState.Idole>((int)ActEvent.Idole);
        m_stateMachine.AddAnyTransition<MovementCharacterState.Walk>((int)ActEvent.Walk);
        m_stateMachine.AddAnyTransition<MovementCharacterState.Run>((int)ActEvent.Run);
        m_stateMachine.AddAnyTransition<MovementCharacterState.Jump>((int)ActEvent.Jump);
        m_stateMachine.AddAnyTransition<MovementCharacterState.Fall>((int)ActEvent.Fall);
        m_stateMachine.AddAnyTransition<MovementCharacterState.Land>((int)ActEvent.Land);
        m_stateMachine.AddAnyTransition<MovementCharacterState.Dead>((int)ActEvent.Dead);

        m_stateMachine.Start<MovementCharacterState.Idole>();
    }

    public void SetUp(Animator setAnimator, Rigidbody setRigidbody, Transform setTransform, float setRotationSpeed, float setWalkSpeed, float setRunSpeed, float setJumpPower, float setGravityScale, float setIsGroundLength, LayerMask setGroundLayer)
    {
        SetState();
        SetParam(setAnimator, setRigidbody, setTransform, setRotationSpeed, setWalkSpeed, setRunSpeed, setJumpPower, setGravityScale, setIsGroundLength, setGroundLayer);
    }

    public void OnUpdate()
    {
        m_stateMachine.Update();

        m_moveForward = Camera.main.transform.TransformDirection(m_inputDirection);
    }

    void SetParam(Animator setAnimator, Rigidbody setRigidbody, Transform setTransform, float setRotationSpeed, float setWalkSpeed, float setRunSpeed, float setJumpPower, float setGravityScale, float setIsGroundLength, LayerMask setGroundLayer)
    {
        m_animator = setAnimator;
        m_rigidbody = setRigidbody;
        m_myTransform = setTransform;
        m_rotatingSpeed = setRotationSpeed;
        m_walkingSpeed = setWalkSpeed;
        m_runningSpeed = setRunSpeed;
        m_jumpingPower = setJumpPower;
        m_gravityScale = setGravityScale;
        m_isGroundLength = setIsGroundLength;
        m_groundLayer = setGroundLayer;
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
        m_myTransform.rotation = Quaternion.Slerp(transform.rotation, m_targetRotation, Time.deltaTime * m_rotatingSpeed);
    }

    public void UserInput(float h, float v)
    {
        //入力方向のベクトル
        //direction = new Vector3(h, 0, v);//これだとカメラの向きによっては傾いたり、浮いたりしてしまう
        m_inputDirection = Vector3.forward * v + Vector3.right * h;
    }

    bool FinishedAnimation(int layer = 0)
    {
        return AnimationController.Instance.FinishedAnimation(m_animator, layer);
    }
    void PlayAnimation(string stateName, float transitionDuration = 0.1f)
    {
        AnimationController.Instance.PlayAnimation(m_animator, stateName, transitionDuration);
    }
}
