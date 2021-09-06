using UnityEngine;

public partial class BattleStateMachine : MonoBehaviour
{
    public class BattleIdleState : BattleStateMachineBase
    {
        public override void OnEnter(BattleStateMachine owner)
        {
            //待機
        }

        public override void OnExit(BattleStateMachine owner)
        {
            //タイマーリセット
            owner.m_countTimer = owner.m_actionTimer;
        }

        public override void OnUpdate(BattleStateMachine owner)
        {
            if (owner.m_countTimer > -1f)
            {
                owner.m_countTimer -= Time.deltaTime;
            }
            //タイマースタート
            if (owner.m_countTimer < 0f)
            {
                if (owner.CompareTag("Enemy"))
                {
                    owner.ChangeState(owner.battleAtatckState);
                }
                else if (owner.CompareTag("Player"))
                {
                    owner.ChangeState(owner.battleAtatckState);
                }
            }
        }
    }
}