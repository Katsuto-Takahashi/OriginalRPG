using UnityEngine;
using UniRx;

//[RequireComponent(typeof(MovementCharacterStateMachine), typeof(BattleCharacterStateMachine))]
public class PlayerManager : Character
{
    //BattleCharacterStateMachine m_bcsm;
    //MovementCharacterStateMachine m_mcsm;

    protected override void SetUp()
    {
        base.SetUp();
        //m_mcsm = GetComponent<MovementCharacterStateMachine>();
        //m_bcsm = GetComponent<BattleCharacterStateMachine>();

        HP.DistinctUntilChanged().Subscribe(hp => CheckHP(hp));
        AP.DistinctUntilChanged().Subscribe(ap => CheckAP(ap));


        HP.DistinctUntilChanged().Subscribe(_ => m_bcsm.SetParam(this));
        AP.DistinctUntilChanged().Subscribe(_ => m_bcsm.SetParam(this));

        MaxHP.DistinctUntilChanged().Subscribe(_ => m_bcsm.SetParam(this));
        MaxAP.DistinctUntilChanged().Subscribe(_ => m_bcsm.SetParam(this));
        Strength.DistinctUntilChanged().Subscribe(_ => m_bcsm.SetParam(this));
        Defense.DistinctUntilChanged().Subscribe(_ => m_bcsm.SetParam(this));
        MagicPower.DistinctUntilChanged().Subscribe(_ => m_bcsm.SetParam(this));
        MagicResist.DistinctUntilChanged().Subscribe(_ => m_bcsm.SetParam(this));
        Speed.DistinctUntilChanged().Subscribe(_ => m_bcsm.SetParam(this));

        m_bcsm.CanInput.DistinctUntilChanged().Subscribe(s => StopMove(s));
        m_bcsm.PlayAction.Where(x => x == true).Subscribe(_ => PlayAction());
        m_mcsm.PlayAction.Where(x => x == false).Subscribe(_ => FinishAction());
        m_mcsm.SetUp(m_animator, m_rigidbody, m_capsuleCollider, m_myTransform, m_param);
        m_bcsm.SetUp(m_animator, m_param);
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
        if (m_mcsm.CanOperation)
        {
            //Debug.Log($"入力の移動方向{InputController.Instance.Move()}");
            m_mcsm.UserInput(InputController.Instance.Move());
        }
        else
        {
            //Debug.Log($"入力のかわり移動方向{m_bcsm.MoveDirection}");
            m_mcsm.UserInput(m_bcsm.MoveDirection);
            m_mcsm.InBattleRotation(m_bcsm.TargetRotation);
        }
    }

    void StopMove(bool can)
    {
        Debug.Log($"CanOperationフラグ{m_mcsm.CanOperation}");
        m_mcsm.CanOperation = can;
        Debug.Log($"CanOperationフラグ変更{m_mcsm.CanOperation}");
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

    void PlayAction()
    {
        m_mcsm.PlayAction.Value = true;
    }

    void FinishAction()
    {
        m_bcsm.PlayAction.Value = false;
    }
}
