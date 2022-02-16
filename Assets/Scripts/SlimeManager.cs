using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SlimeManager : Enemy
{
    protected override void SetUp()
    {
        m_mesm.SetUP(m_animator, m_rigidbody, m_capsuleCollider, m_myTransform, m_param);
        m_besm.SetUP();
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
}
