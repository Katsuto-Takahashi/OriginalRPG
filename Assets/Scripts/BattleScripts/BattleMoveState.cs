using UnityEngine;

public partial class BattleCharacterStateMachine : MonoBehaviour
{
    public class BattleMoveState : BattleStateMachineBase
    {
        public override void OnEnter(BattleCharacterStateMachine owner)
        {
            if (owner.CompareTag("Enemy"))
            {
                owner.PlayAnimation("Move");
            }
            else if (owner.CompareTag("Player"))
            {
                owner.PlayAnimation("Move");
            }
            if (owner.CompareTag("Player"))
            {
                if (owner.playerControllerCC)
                {
                    owner.playerControllerCC.enabled = false;
                }
                else if (owner.playerControllerRB)
                {
                    owner.playerControllerRB.enabled = false;
                }
            }
        }

        public override void OnExit(BattleCharacterStateMachine owner)
        {
            owner.m_characterActionCount--;
        }

        public override void OnUpdate(BattleCharacterStateMachine owner)
        {
            owner.m_targetPosition = owner.m_targetCharacters[owner.m_targetNumber].transform.position;
            owner.m_distance = (owner.m_targetPosition.x - owner.transform.position.x) * (owner.m_targetPosition.x - owner.transform.position.x) + (owner.m_targetPosition.z - owner.transform.position.z) * (owner.m_targetPosition.z - owner.transform.position.z);
            if (owner.m_distance > 2f)
            {
                Vector3 target = owner.m_targetPosition;
                target.y = 0f;
                owner.transform.position = new Vector3(owner.transform.position.x, 0f, owner.transform.position.z);
                owner.transform.LookAt(target);
                owner.transform.position = Vector3.MoveTowards(owner.transform.position, owner.m_targetPosition, owner.m_moveSpeed * Time.deltaTime);
            }
            else
            {
                Vector3 target = owner.m_targetPosition;
                target.y = 0f;
                owner.transform.position = new Vector3(owner.transform.position.x, 0f, owner.transform.position.z);
                owner.transform.LookAt(target);
                owner.ChangeState(owner.battleAttackState);
            }
        }
    }
}
