﻿using UnityEngine;
using UniRx;

//[RequireComponent(typeof(MovementCharacterStateMachine), typeof(BattleCharacterStateMachine))]
public class PlayerManager : Character
{
    //BattleCharacterStateMachine m_bcsm;
    //MovementCharacterStateMachine m_mcsm;

    protected override void SetUp()
    {
        //m_mcsm = GetComponent<MovementCharacterStateMachine>();
        //m_bcsm = GetComponent<BattleCharacterStateMachine>();

        HP.DistinctUntilChanged().Subscribe(hp => CheckHP(hp));
        AP.DistinctUntilChanged().Subscribe(ap => CheckAP(ap));


        HP.DistinctUntilChanged().Subscribe(_ => m_bcsm.Parameter(this));
        AP.DistinctUntilChanged().Subscribe(_ => m_bcsm.Parameter(this));

        MaxHP.DistinctUntilChanged().Subscribe(_ => m_bcsm.Parameter(this));
        MaxAP.DistinctUntilChanged().Subscribe(_ => m_bcsm.Parameter(this));
        Strength.DistinctUntilChanged().Subscribe(_ => m_bcsm.Parameter(this));
        Defense.DistinctUntilChanged().Subscribe(_ => m_bcsm.Parameter(this));
        MagicPower.DistinctUntilChanged().Subscribe(_ => m_bcsm.Parameter(this));
        MagicResist.DistinctUntilChanged().Subscribe(_ => m_bcsm.Parameter(this));
        Speed.DistinctUntilChanged().Subscribe(_ => m_bcsm.Parameter(this));

        m_bcsm.Stop.DistinctUntilChanged().Subscribe(s => StopMove(s));

        m_mcsm.SetUp(m_animator, m_rigidbody, m_capsuleCollider, m_myTransform, m_param);
        m_bcsm.SetUp(m_animator, m_hsl, m_param);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
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
        m_mcsm.UserInput(InputController.Instance.Move());
    }

    void StopMove(bool stop)
    {
        m_mcsm.CanOperation = !stop;
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
    }
    void Alive()
    {
        m_bcsm.IsDead = false;
    }
    void CheckAP(int ap)
    {

    }
}
