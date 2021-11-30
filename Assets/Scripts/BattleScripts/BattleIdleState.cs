using UnityEngine;

public partial class BCharacterStateMachine : MonoBehaviour
{
    public class BattleIdleState : BattleStateMachineBase
    {
        public override void OnEnter(BCharacterStateMachine owner)
        {
            if (owner.CompareTag("Enemy"))
            {
                owner.PlayAnimation("Idle");
            }
            else if (owner.CompareTag("Player"))
            {
                owner.PlayAnimation("Idle");
            }
            if (owner.m_battle)
            {
                if (owner.CompareTag("Enemy"))
                {
                    owner.m_action = true;
                }
                owner.m_countTimer += owner.m_actionTimer;
            }         
        }

        public override void OnExit(BCharacterStateMachine owner)
        {
        }

        public override void OnUpdate(BCharacterStateMachine owner)
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
            else
            {
                if (owner.m_firstAction)
                {
                    if (owner.CompareTag("Enemy"))
                    {
                        owner.m_countTimer = owner.m_actionTimer;
                        owner.m_action = true;
                    }
                    else if (owner.CompareTag("Player"))
                    {
                        owner.m_countTimer = 0f;
                    }
                    owner.m_firstAction = false;
                }
            }
        }
    }
}