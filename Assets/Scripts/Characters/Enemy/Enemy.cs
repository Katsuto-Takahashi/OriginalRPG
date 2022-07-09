using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(MovementEnemyStateMachine), typeof(BattleEnemyStateMachine))]
public class Enemy : EnemyParameter, IDamageable
{
    /// <summary>自分のTransform</summary>
    protected Transform m_myTransform;

    [SerializeField]
    [Tooltip("移動に必要なパラメーター")]
    protected MoveParameters m_param = new MoveParameters();

    protected Animator m_animator;
    protected Rigidbody m_rigidbody;
    protected CapsuleCollider m_capsuleCollider;
    protected SphereCollider m_sphereCollider;
    //protected HasSkillList m_hsl;
    protected MovementEnemyStateMachine m_mesm;
    protected BattleEnemyStateMachine m_besm;

    public BattleEnemyStateMachine BESM => m_besm;
    public MovementEnemyStateMachine MESM => m_mesm;

    void Awake()
    {
        m_myTransform = transform;

        //m_hsl = GetComponent<HasSkillList>();
        m_animator = GetComponentInChildren<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_capsuleCollider = GetComponent<CapsuleCollider>();
        m_sphereCollider = GetComponentInChildren<SphereCollider>();
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
        HP.Value = HP.Value - damage < 1 ? 0 : HP.Value - damage;
    }

    public void ChengeKinematic(bool flag)
    {
        m_rigidbody.isKinematic = flag;
    }
}
