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
            owner.m_targetCharacters[owner.m_targetNumber].GetComponent<EnemyManager>().TakeDamage(owner.battleManager.damageCalculator.DecideEnemyDamege(owner.hasSkillList.m_normalSkill[0].attackType, owner.hasSkillList.m_normalSkill[0].attackAttributes,owner.battleManager.damageCalculator.CalculateNormalDamage(owner.hasSkillList.m_normalSkill[0], owner.m_targetCharacters[owner.m_targetNumber].GetComponent<EnemyManager>().enemyParameters.Defense,owner.GetComponent<CharacterParameterManager>().Strength) ,owner.m_targetCharacters[owner.m_targetNumber].GetComponent<EnemyManager>().enemyParameters));
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
