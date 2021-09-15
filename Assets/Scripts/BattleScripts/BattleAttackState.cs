using UnityEngine;

public partial class BattleStateMachine : MonoBehaviour
{
    public class BattleAttackState : BattleStateMachineBase
    {
        public override void OnEnter(BattleStateMachine owner)
        {
            //攻撃
            //owner.PlayAnimation("Attack");
            //攻撃対象のTakeDamage()を呼ぶ
            if (owner.CompareTag("Player"))
            {
                owner.m_targetCharacters[owner.m_targetNumber].GetComponent<EnemyManager>().TakeDamage(owner.battleManager.Damage(owner.gameObject, owner.m_targetCharacters[owner.m_targetNumber], owner.hasSkillList.m_normalSkill[0]));
            }
            else if (owner.CompareTag("Enemy"))
            {
                owner.m_targetCharacters[owner.m_targetNumber].GetComponent<CharacterParameterManager>().TakeDamage(owner.battleManager.Damage(owner.gameObject, owner.m_targetCharacters[owner.m_targetNumber], owner.hasSkillList.m_normalSkill[0]));
            }
        }

        public override void OnExit(BattleStateMachine owner)
        {
            if (owner.CompareTag("Player"))
            {
                if (owner.playerControllerCC)
                {
                    owner.playerControllerCC.enabled = true;
                }
                else if (owner.playerControllerRB)
                {
                    owner.playerControllerRB.enabled = true;
                }
            }
            owner.m_open = false;
        }

        public override void OnUpdate(BattleStateMachine owner)
        {
            owner.ChangeState(owner.battleIdleState);
        }
    }
}
