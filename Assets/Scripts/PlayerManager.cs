using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(MovementCharacterStateMachine), typeof(BattleCharacterStateMachine))]
public class PlayerManager : CharacterManager
{
    BoolReactiveProperty m_isDead = new BoolReactiveProperty(false);
    public IReadOnlyReactiveProperty<bool> IsDead => m_isDead;

    BattleCharacterStateMachine m_bcsm;
    MovementCharacterStateMachine m_mcsm;

    protected override void SetUp()
    {
        m_myTransform = transform;

        m_animator = GetComponentInChildren<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_capsuleCollider = GetComponent<CapsuleCollider>();

        m_mcsm = GetComponent<MovementCharacterStateMachine>();
        m_bcsm = GetComponent<BattleCharacterStateMachine>();
        m_hsl = GetComponent<HasSkillList>();

        Character.HP.DistinctUntilChanged().Subscribe(hp => CheckHP(hp));
        Character.AP.DistinctUntilChanged().Subscribe(ap => CheckAP(ap));

        m_bcsm.Stop.DistinctUntilChanged().Subscribe(s => StopMove(s));

        m_mcsm.SetUp(m_animator, m_rigidbody, m_capsuleCollider, m_myTransform, m_param);
        m_bcsm.SetUp(m_animator, m_hsl, m_param);
    }

    protected override void OnUpdate()
    {
        m_mcsm.OnUpdate();
        m_bcsm.OnUpdate();

        ApplyGetAxis();
    }

    protected override void OnFixedUpdate()
    {
        m_mcsm.OnFixedUpdate();
    }

    void ApplyGetAxis()
    {
        float h = Input.GetAxis("Lstick_h");
        float v = Input.GetAxis("Lstick_v");
        m_mcsm.UserInput(h, v);
    }

    void StopMove(bool stop)
    {
        m_mcsm.NotOperation = stop;
    }

    void CheckHP(int hp)
    {
        if (hp > 0)
        {
            Alive();
        }
        else
        {
            CheckDead();
        }
    }
    void CheckDead()
    {
        m_bcsm.IsDead = true;
        m_isDead.Value = true;
    }
    void Alive()
    {
        m_bcsm.IsDead = false;
        m_isDead.Value = false;
    }
    void CheckAP(int ap)
    {

    }
}
