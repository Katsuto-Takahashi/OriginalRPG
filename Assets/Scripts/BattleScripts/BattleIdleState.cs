using UnityEngine;

public partial class BattleStateMachine : MonoBehaviour
{
    public class BattleIdleState : BattleStateMachineBase
    {
        public override void OnEnter(BattleStateMachine owner)
        {
            if (owner.m_battle)
            {
                //owner.PlayAnimation("");
            }
            //待機
            //タイマーセット
            if (owner.m_firstAction)
            {
                if (owner.CompareTag("Enemy"))
                {
                    owner.m_countTimer = owner.m_actionTimer;
                }
                else if (owner.CompareTag("Player"))
                {
                    owner.m_countTimer = -0.9f;
                }
                owner.m_firstAction = false;
            }
            else
            {
                owner.m_countTimer = owner.m_actionTimer;
            }
        }

        public override void OnExit(BattleStateMachine owner)
        {
            owner.m_countTimer = owner.m_actionTimer;
        }

        public override void OnUpdate(BattleStateMachine owner)
        {
            if (owner.m_battle)
            {
                if (owner.m_countTimer > -1f)
                {
                    owner.m_countTimer -= Time.deltaTime;
                    Debug.Log(owner.m_countTimer);
                }
                //タイマースタート
                if (owner.m_countTimer < 0f)
                {
                    if (owner.CompareTag("Enemy"))
                    {
                        owner.ChangeState(owner.battleWaitActionState);
                    }
                    else if (owner.CompareTag("Player"))
                    {
                        owner.ChangeState(owner.battleWaitActionState);
                    }
                }
            }
        }
    }
}