using UnityEngine;

public partial class BattleStateMachine : MonoBehaviour
{
    public class BattleWaitActionState : BattleStateMachineBase
    {
        public override void OnEnter(BattleStateMachine owner)
        {
            if (owner.CompareTag("Enemy"))
            {
                owner.m_targetNumber = Random.Range(0, owner.m_targetCharacters.Count);
            }
            else if (owner.CompareTag("Player"))
            {
                if (!owner.m_open)
                {
                    owner.m_battlePanel.SetActive(true);
                    owner.m_open = true;
                }
            }
        }

        public override void OnExit(BattleStateMachine owner)
        {
            owner.m_action = false;
        }

        public override void OnUpdate(BattleStateMachine owner)
        {
            if (owner.CompareTag("Enemy"))
            {
                if (owner.m_action)
                {
                    Debug.Log("敵の攻撃");
                    owner.ChangeState(owner.battleMoveState);
                }
                else
                {
                    Debug.Log("敵の待機");
                    owner.ChangeState(owner.battleIdleState);
                }
            }
            else if (owner.CompareTag("Player"))
            {
                if (owner.m_action)
                {
                    Debug.Log("攻撃");
                    owner.ChangeState(owner.battleMoveState);
                }
            }
        }
    }
}
