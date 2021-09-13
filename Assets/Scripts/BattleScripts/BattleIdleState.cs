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
                    owner.m_countTimer = 0f;
                }
                owner.m_firstAction = false;
            }
        }

        public override void OnExit(BattleStateMachine owner)
        {
            owner.m_countTimer += owner.m_actionTimer;
        }

        public override void OnUpdate(BattleStateMachine owner)
        {
            if (owner.m_battle && owner.m_characterActionCount < 3)
            {
                if (owner.m_countTimer <= 0f)
                {
                    if (owner.CompareTag("Enemy"))
                    {
                        owner.m_characterActionCount++;
                        owner.ChangeState(owner.battleWaitActionState);
                    }
                    else if (owner.CompareTag("Player"))
                    {
                        owner.m_characterActionCount++;
                        owner.ChangeState(owner.battleWaitActionState);
                    }
                }
            }
        }
    }
}