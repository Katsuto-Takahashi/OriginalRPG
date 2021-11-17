using UnityEngine;

public partial class BattleCharacterStateMachine : MonoBehaviour
{
    public class BattleAttackState : BattleStateMachineBase
    {
        float attackTime = 1f;
        float time = 1f;
        public override void OnEnter(BattleCharacterStateMachine owner)
        {
            //攻撃
            if (owner.CompareTag("Enemy"))
            {
                owner.PlayAnimation("Attack");
            }
            else if (owner.CompareTag("Player"))
            {
                owner.PlayAnimation("Attack");
            }            
        }

        public override void OnExit(BattleCharacterStateMachine owner)
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
            time = attackTime;
        }

        public override void OnUpdate(BattleCharacterStateMachine owner)
        {
            time -= Time.deltaTime;

            if (time < 0f)
            {
                //攻撃対象のTakeDamage()を呼ぶ
                if (owner.CompareTag("Player"))
                {
                    owner.m_targetCharacters[owner.m_targetNumber].GetComponent<EnemyManager>().TakeDamage(owner.battleManager.Damage(owner.gameObject, owner.m_targetCharacters[owner.m_targetNumber], owner.hasSkillList.m_normalSkill[0]));
                }
                else if (owner.CompareTag("Enemy"))
                {
                    owner.m_targetCharacters[owner.m_targetNumber].GetComponent<CharacterParameterManager>().TakeDamage(owner.battleManager.Damage(owner.gameObject, owner.m_targetCharacters[owner.m_targetNumber], owner.hasSkillList.m_normalSkill[0]));
                }
                owner.ChangeState(owner.battleIdleState);
            }
        }
    }
}
