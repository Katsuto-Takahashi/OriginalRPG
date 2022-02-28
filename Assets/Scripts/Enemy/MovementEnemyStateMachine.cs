using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using State = StateMachine<MovementEnemyStateMachine>.State;

public partial class MovementEnemyStateMachine : MonoBehaviour
{
    StateMachine<MovementEnemyStateMachine> m_stateMachine;
    public enum ActEvent : byte
    {
        Idle,
        Walk,
        Run,
        Chase,
        Jump,
        Fall,
        Land,
        Fly,
        Stop,
        Dead
    }

    Animator m_animator;
    Rigidbody m_rigidbody;
    CapsuleCollider m_capsuleCollider;
    SphereCollider m_sphereCollider;
    GameObject m_targetObject;
    public GameObject TargetObject => m_targetObject;
    LayerMask m_targetLayer;
    [SerializeReference, SubclassSelector]
    Node m_childNode;

    /// <summary>transfom</summary>
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

    void SetState()
    {
        m_stateMachine = new StateMachine<MovementEnemyStateMachine>(this);

        m_stateMachine.AddAnyTransition<MovementEnemyState.Idle>((int)ActEvent.Idle);
        m_stateMachine.AddAnyTransition<MovementEnemyState.Walk>((int)ActEvent.Walk);
        m_stateMachine.AddAnyTransition<MovementEnemyState.Run>((int)ActEvent.Run);
        m_stateMachine.AddAnyTransition<MovementEnemyState.Chase>((int)ActEvent.Chase);
        m_stateMachine.AddAnyTransition<MovementEnemyState.Jump>((int)ActEvent.Jump);
        m_stateMachine.AddAnyTransition<MovementEnemyState.Fall>((int)ActEvent.Fall);
        m_stateMachine.AddAnyTransition<MovementEnemyState.Land>((int)ActEvent.Land);
        m_stateMachine.AddAnyTransition<MovementEnemyState.Fly>((int)ActEvent.Fly);
        m_stateMachine.AddAnyTransition<MovementEnemyState.Stop>((int)ActEvent.Stop);
        m_stateMachine.AddAnyTransition<MovementEnemyState.Dead>((int)ActEvent.Dead);

        m_stateMachine.Start<MovementEnemyState.Idle>();
    }

    void SetPram(Animator setAnimator, Rigidbody setRigidbody, CapsuleCollider setCollider, SphereCollider setSphere, Transform setTransform, MoveParameters setParam)
    {
        m_animator = setAnimator;
        m_rigidbody = setRigidbody;
        m_capsuleCollider = setCollider;
        m_sphereCollider = setSphere;
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

    public void SetUP(Animator setAnimator, Rigidbody setRigidbody, CapsuleCollider setCollider, SphereCollider setSphere, Transform setTransform, MoveParameters setParam)
    {
        SetPram(setAnimator, setRigidbody, setCollider, setSphere, setTransform, setParam);
        m_childNode.GetNode();
        SetState();
    }

    public void OnUpdate()
    {
        m_stateMachine.Update();

        //m_moveForward = Camera.main.transform.TransformDirection(m_inputDirection);

        ApplyGravity(m_stateMachine.CurrentSate);
    }

    public void OnFixedUpdate()
    {
        ApplyMovement();
        ApplyRotation();
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

    void ApplyGravity(State state)
    {
        if (state is MovementEnemyState.Jump) return;

        if (IsSlope()) m_currentVelocity.y = m_gravityScale * Physics.gravity.y;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == m_targetLayer)
        {
            m_targetObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == m_targetLayer)
        {
            m_targetObject = null;
        }
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

    bool FinishedAnimation(int layer = 0)
    {
        return AnimationController.Instance.FinishedAnimation(m_animator, layer);
    }

    void PlayAnimation(string stateName, float transitionDuration = 0.1f)
    {
        AnimationController.Instance.PlayAnimation(m_animator, stateName, transitionDuration);
    }

    public void Chenge(int actEvent)
    {
        m_stateMachine.Dispatch(actEvent);
    }
}
