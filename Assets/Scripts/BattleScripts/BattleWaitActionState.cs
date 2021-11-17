using UnityEngine;

public partial class BattleCharacterStateMachine : MonoBehaviour
{
    public class BattleWaitActionState : BattleStateMachineBase
    {
        public override void OnEnter(BattleCharacterStateMachine owner)
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

        public override void OnExit(BattleCharacterStateMachine owner)
        {
            owner.m_action = false;
        }

        public override void OnUpdate(BattleCharacterStateMachine owner)
        {
            if (owner.CompareTag("Enemy"))
            {
                if (owner.m_action)
                {
                    owner.ChangeState(owner.battleMoveState);
                }
                else
                {
                    owner.ChangeState(owner.battleIdleState);
                }
            }
            else if (owner.CompareTag("Player"))
            {
                if (owner.m_action)
                {
                    owner.ChangeState(owner.battleMoveState);
                }
            }
        }
    }
}
