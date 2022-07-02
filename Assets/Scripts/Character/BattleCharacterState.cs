using UnityEngine;
using State = StateMachine<BattleCharacterStateMachine>.State;

public partial class BattleCharacterStateMachine : MonoBehaviour
{
    public class BattleCharacterState
    {
        public class Wait : State
        {
            protected override void OnEnter(State prevState)
            {
                Debug.Log("Wait");
            }
            protected override void OnUpdate()
            {
                if (owner.m_isDead)
                {
                    StateMachine.Dispatch((int)ActEvent.Dead);
                }
                else if (owner.m_isBind)
                {
                    StateMachine.Dispatch((int)ActEvent.Bind);
                }
                else if (!owner.m_isBattle)
                {
                    StateMachine.Dispatch((int)ActEvent.NoBattle);
                }
                if (!owner.m_canSelect.Value && owner.m_actionCount > 0)
                {
                    StateMachine.Dispatch((int)ActEvent.Standby);
                }
            }
            protected override void OnExit(State nextState)
            {
            }
        }

        public class Standby : State
        {
            protected override void OnEnter(State prevState)
            {
                Debug.Log("Standby");
                owner.m_canSelect.Value = true;
            }

            protected override void OnUpdate()
            {
                if (owner.m_isDead)
                {
                    StateMachine.Dispatch((int)ActEvent.Dead);
                }
                else if (owner.m_isBind)
                {
                    StateMachine.Dispatch((int)ActEvent.Bind);
                }
                else if (!owner.m_isBattle)
                {
                    StateMachine.Dispatch((int)ActEvent.NoBattle);
                }
                else if (!owner.m_canSelect.Value)
                {
                    StateMachine.Dispatch((int)ActEvent.Move);
                }
            }

            protected override void OnExit(State nextState)
            {
                if (nextState is Move)
                {
                    owner.m_actionCount--;
                }
            }
        }

        public class Move : State
        {
            float m_x;
            float m_z;
            //Enemy enemy;
            //Skill skill;
            protected override void OnEnter(State prevState)
            {
                Debug.Log("Move");
                owner.m_canInput.Value = false;
                //owner.PlayAnimation("Run");
                //enemy = BattleManager.Instance.SetEnemy;
                owner.m_selectTarget = BattleManager.Instance.SelectTargetList[owner.m_battleID.Value];
                //owner.m_selectSkill = BattleManager.Instance.SetSkill;
                owner.m_selectSkill = BattleManager.Instance.SelectSkillList[owner.m_battleID.Value];
            }

            protected override void OnUpdate()
            {
                //m_x = enemy.transform.position.x - owner.transform.position.x;
                m_x = owner.m_selectTarget.transform.position.x - owner.transform.position.x;
                //m_z = enemy.transform.position.z - owner.transform.position.z;
                m_z = owner.m_selectTarget.transform.position.z - owner.transform.position.z;
                //owner.m_distance = (enemy.transform.position.x - owner.transform.position.x) * (enemy.transform.position.x - owner.transform.position.x) + (enemy.transform.position.z - owner.transform.position.z) * (enemy.transform.position.z - owner.transform.position.z);
                owner.m_distance = (owner.m_selectTarget.transform.position.x - owner.transform.position.x) * (owner.m_selectTarget.transform.position.x - owner.transform.position.x) + (owner.m_selectTarget.transform.position.z - owner.transform.position.z) * (owner.m_selectTarget.transform.position.z - owner.transform.position.z);
                if (owner.m_isDead)
                {
                    StateMachine.Dispatch((int)ActEvent.Dead);
                }
                else if (owner.m_isBind)
                {
                    StateMachine.Dispatch((int)ActEvent.Bind);
                }
                else if (!owner.m_isBattle)
                {
                    StateMachine.Dispatch((int)ActEvent.NoBattle);
                }
                //else if (true)//行動キャンセル
                //{
                //    StateMachine.Dispatch((int)ActEvent.Wait);
                //}
                if (owner.m_distance > owner.m_selectSkill.SkillParameter.SkillRange * owner.m_selectSkill.SkillParameter.SkillRange)
                {
                    //Vector3 target = enemy.transform.position;
                    Vector3 target = owner.m_selectTarget.transform.position;
                    target.y = owner.transform.position.y;
                    owner.transform.LookAt(target);
                    owner.m_moveDirection.x = m_x;
                    owner.m_moveDirection.y = m_z;
                    owner.m_moveDirection.Normalize();
                }
                else
                {
                    owner.m_moveDirection.x = 0.0f;
                    owner.m_moveDirection.y = 0.0f;
                    Vector3 target = owner.m_targetPosition;
                    target.y = owner.transform.position.y;
                    owner.m_targetRotation = Quaternion.LookRotation(target);
                    StateMachine.Dispatch((int)ActEvent.BattleAction);
                }
            }

            protected override void OnExit(State nextState)
            {
                if (nextState is Wait)
                {
                    owner.m_canInput.Value = true;
                }
                if (nextState is BattleAction)
                {
                    owner.m_playAction.Value = true;
                }
            }
        }

        public class BattleAction : State
        {
            protected override void OnEnter(State prevState)
            {
                Debug.Log("BattleAction");
                BattleManager.Instance.PlaySkillEffect(owner.gameObject, owner.m_selectTarget, owner.m_selectSkill);
            }

            protected override void OnUpdate()
            {
                if (!owner.m_playAction.Value)
                {
                    StateMachine.Dispatch((int)ActEvent.ActionEnd);
                }
            }

            protected override void OnExit(State nextState)
            {
            }
        }

        public class ActionEnd : State
        {
            protected override void OnEnter(State prevState)
            {
                Debug.Log("ActionEnd");
                owner.m_canInput.Value = true;
            }

            protected override void OnUpdate()
            {
                if (owner.m_isDead)
                {
                    StateMachine.Dispatch((int)ActEvent.Dead);
                }
                else if (owner.m_isBind)
                {
                    StateMachine.Dispatch((int)ActEvent.Bind);
                }
                else if (!owner.m_isBattle)
                {
                    StateMachine.Dispatch((int)ActEvent.NoBattle);
                }
                else
                {
                    StateMachine.Dispatch((int)ActEvent.Wait);
                }
            }

            protected override void OnExit(State nextState)
            {
            }
        }

        public class Bind : State
        {
            protected override void OnEnter(State prevState)
            {
                Debug.Log("Bind");
                owner.m_actionCount = 0;
                owner.m_canInput.Value = false;
                owner.PlayAnimation("");
            }

            protected override void OnUpdate()
            {
                if (owner.m_isBind)
                {
                    if (owner.m_isDead)
                    {
                        StateMachine.Dispatch((int)ActEvent.Dead);
                    }
                    else if (!owner.m_isBattle)
                    {
                        StateMachine.Dispatch((int)ActEvent.NoBattle);
                    }
                }
                else
                {
                    if (owner.m_isBattle)
                    {
                        StateMachine.Dispatch((int)ActEvent.Wait);
                    }
                    else
                    {
                        StateMachine.Dispatch((int)ActEvent.NoBattle);
                    }
                }
            }

            protected override void OnExit(State nextState)
            {
                owner.m_canInput.Value = true;
            }
        }

        public class NoBattle : State
        {
            protected override void OnEnter(State prevState)
            {
                Debug.Log("NoBattle");
                owner.m_canInput.Value = true;
                owner.m_isBattle = false;
                owner.m_currentTimer = 0f;
                owner.m_actionCount = 1;
            }

            protected override void OnUpdate()
            {
                if (owner.m_isDead)
                {
                    StateMachine.Dispatch((int)ActEvent.Dead);
                }
                else if (owner.m_isBattle)
                {
                    StateMachine.Dispatch((int)ActEvent.Wait);
                }
            }

            protected override void OnExit(State nextState)
            {
            }
        }

        public class Dead : State
        {
            protected override void OnEnter(State prevState)
            {
                Debug.Log("Dead");
                owner.m_actionCount = 0;
                owner.m_canInput.Value = false;
                owner.PlayAnimation("");
            }

            protected override void OnUpdate()
            {
                if (!owner.m_isDead)
                {
                    if (owner.m_isBattle)
                    {
                        StateMachine.Dispatch((int)ActEvent.Wait);
                    }
                    else
                    {
                        StateMachine.Dispatch((int)ActEvent.NoBattle);
                    }
                }
            }

            protected override void OnExit(State nextState)
            {
                owner.PlayAnimation("");
                if (owner.FinishedAnimation())
                {
                    owner.m_canInput.Value = true;
                }
            }
        }
    }
}