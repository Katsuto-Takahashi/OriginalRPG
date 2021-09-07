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
            owner.m_countTimer = owner.m_actionTimer;
        }

        public override void OnExit(BattleStateMachine owner)
        {
            
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
                        Debug.Log("敵の攻撃");
                        owner.ChangeState(owner.battleAtatckState);
                    }
                    else if (owner.CompareTag("Player"))
                    {
                        Debug.Log("攻撃");
                        owner.ChangeState(owner.battleAtatckState);
                    }
                }
            }
        }
    }
}