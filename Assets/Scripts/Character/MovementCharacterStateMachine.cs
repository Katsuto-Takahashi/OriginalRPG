using UnityEngine;
using State = StateMachine<MovementCharacterStateMachine>.State;

public partial class MovementCharacterStateMachine : MonoBehaviour
{
    StateMachine<MovementCharacterStateMachine> m_stateMachine;
    enum ActEvent : byte
    {
        Idle,
        Walk,
        Run,
        Jump,
        Fall,
        Land,
        Stop,
        Dead
    }

    Animator m_animator;
    Rigidbody m_rigidbody;
    CapsuleCollider m_capsuleCollider;

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
    /// <summary>移動速度</summary>
    float m_movingSpeed;
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
    /// <summary>坂のレイヤー</summary>
    LayerMask m_slopeLayer;

    /// <summary>操作可能かどうか</summary>
    bool m_notOperation = false;
    /// <summary>操作可能かのフラグ</summary>
    public bool NotOperation { get => m_notOperation; set => m_notOperation = value; }

    void SetState()
    {
        m_stateMachine = new StateMachine<MovementCharacterStateMachine>(this);

        m_stateMachine.AddAnyTransition<MovementCharacterState.Idle>((int)ActEvent.Idle);
        m_stateMachine.AddAnyTransition<MovementCharacterState.Walk>((int)ActEvent.Walk);
        m_stateMachine.AddAnyTransition<MovementCharacterState.Run>((int)ActEvent.Run);
        m_stateMachine.AddAnyTransition<MovementCharacterState.Jump>((int)ActEvent.Jump);
        m_stateMachine.AddAnyTransition<MovementCharacterState.Fall>((int)ActEvent.Fall);
        m_stateMachine.AddAnyTransition<MovementCharacterState.Land>((int)ActEvent.Land);
        m_stateMachine.AddAnyTransition<MovementCharacterState.Stop>((int)ActEvent.Stop);
        m_stateMachine.AddAnyTransition<MovementCharacterState.Dead>((int)ActEvent.Dead);

        m_stateMachine.Start<MovementCharacterState.Idle>();
    }

    public void SetUp(Animator setAnimator, Rigidbody setRigidbody, CapsuleCollider setCollider, Transform setTransform, Parameters setParam)
    {
        SetParam(setAnimator, setRigidbody, setCollider, setTransform, setParam);
        SetState();
    }

    public void OnUpdate()
    {
        m_stateMachine.Update();

        m_moveForward = Camera.main.transform.TransformDirection(m_inputDirection);

        ApplyGravity(m_stateMachine.CurrentSate);
    }

    public void OnFixedUpdate()
    {
        ApplyMovement();
        ApplyRotation();
    }

    void SetParam(Animator setAnimator, Rigidbody setRigidbody, CapsuleCollider setCollider, Transform setTransform, Parameters setParam)
    {
        m_animator = setAnimator;
        m_rigidbody = setRigidbody;
        m_capsuleCollider = setCollider;
        m_myTransform = setTransform;
        m_rotatingSpeed = setParam.RotatingSpeed;
        m_walkingSpeed = setParam.WalkingSpeed;
        m_runningSpeed = setParam.RunningSpeed;
        m_jumpingPower = setParam.JumpingPower;
        m_gravityScale = setParam.GravityScale;
        m_isGroundLength = setParam.IsGroundLength;
        m_groundLayer = setParam.GroundLayer;
        m_slopeLayer = setParam.SlopeLayer;
        m_movingSpeed = m_walkingSpeed;
    }

    void ApplyMovement()
    {
        Vector3 velocity;
        velocity = Vector3.Scale(m_currentVelocity, new Vector3(m_movingSpeed, 1f, m_movingSpeed));
        m_rigidbody.velocity = velocity;
    }

    void ApplyRotation()
    {
        m_myTransform.rotation = Quaternion.Slerp(transform.rotation, m_targetRotation, Time.deltaTime * m_rotatingSpeed);
    }

    public void UserInput(float h, float v)
    {
        m_inputDirection = Vector3.forward * v + Vector3.right * h;
    }

    bool IsGround()
    {
        Vector3 start = new Vector3(m_myTransform.position.x, m_myTransform.position.y + m_capsuleCollider.center.y, m_myTransform.position.z);
        Ray ray = new Ray(start, Vector3.down);
        bool isGround = Physics.SphereCast(ray, 0.19f, out _, m_isGroundLength, m_groundLayer);
        return isGround;
    }

    bool IsSlope()
    {
        Vector3 start = new Vector3(m_myTransform.position.x, m_myTransform.position.y + m_capsuleCollider.center.y, m_myTransform.position.z);
        //15°までの坂なら上れる
        Vector3 end = start + 1.1f * m_isGroundLength * Vector3.down;
        bool isGround = Physics.Linecast(start, end, m_slopeLayer);
        return isGround;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 start = new Vector3(transform.position.x, transform.position.y + 0.75f, transform.position.z);
        Gizmos.DrawLine(start, start + 1.1f * m_isGroundLength * Vector3.down);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + 0.75f, transform.position.z) + Vector3.down * m_isGroundLength, 0.19f);
    }
#endif

    void ApplyGravity(State state)
    {
        if (state is MovementCharacterState.Jump) return;

        if (IsSlope()) m_currentVelocity.y = m_gravityScale * Physics.gravity.y;
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
