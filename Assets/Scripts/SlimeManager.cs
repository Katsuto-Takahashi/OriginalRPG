using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SlimeManager : Enemy
{
    protected override void SetUp()
    {
        HP.DistinctUntilChanged().Subscribe(_ => m_besm.Parameter(this));
        AP.DistinctUntilChanged().Subscribe(_ => m_besm.Parameter(this));

        MaxHP.DistinctUntilChanged().Subscribe(_ => m_besm.Parameter(this));
        MaxAP.DistinctUntilChanged().Subscribe(_ => m_besm.Parameter(this));
        Strength.DistinctUntilChanged().Subscribe(_ => m_besm.Parameter(this));
        Defense.DistinctUntilChanged().Subscribe(_ => m_besm.Parameter(this));
        MagicPower.DistinctUntilChanged().Subscribe(_ => m_besm.Parameter(this));
        MagicResist.DistinctUntilChanged().Subscribe(_ => m_besm.Parameter(this));
        Speed.DistinctUntilChanged().Subscribe(_ => m_besm.Parameter(this));
        Intelligence.DistinctUntilChanged().Subscribe(_ => m_besm.Parameter(this));

        m_mesm.SetUP(m_animator, m_rigidbody, m_capsuleCollider, m_sphereCollider, m_myTransform, m_param);
        m_besm.SetUP(m_animator, m_hsl, m_param);

        m_besm.Move.DistinctUntilChanged().Subscribe(_ => Move());
        m_besm.Stop.DistinctUntilChanged().Subscribe(s => Stop(s));
        m_besm.Look.DistinctUntilChanged().Subscribe(_ => Look());
    }

    protected override void OnUpdate()
    {
        m_mesm.OnUpdate();
        m_besm.OnUpdate();
    }

    protected override void OnFixedUpdate()
    {
        m_mesm.OnFixedUpdate();
    }

    void Move()
    {
        m_mesm.Chenge(MovementEnemyStateMachine.ActEvent.Run);
    }

    void Stop(bool stop)
    {
        m_mesm.CanMove = !stop;
    }

    void Look()
    {
        m_mesm.SetTarget(m_besm.TargetObject);
    }
}
