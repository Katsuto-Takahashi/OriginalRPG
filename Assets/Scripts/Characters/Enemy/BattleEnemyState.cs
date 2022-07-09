using UnityEngine;
using State = StateMachine<BattleEnemyStateMachine>.State;

public partial class BattleEnemyStateMachine : MonoBehaviour
{
    public class BattleEnemyState
    {
        public class Wait : State
        {
            protected override void OnEnter(State prevState)
            {
                Debug.Log($"{owner.name}Wait");
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
                if (owner.m_actionCount > 0)
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
                Debug.Log($"{owner.name}Standby");
                //賢さによって行動を選択する
                owner.m_childNode.Result();
                owner.m_targetIndex = Random.Range(0, owner.m_targets.Count);
                owner.m_targetObject = owner.m_targets[owner.m_targetIndex];
                owner.m_look.Value = true;
                owner.m_targetPosition = owner.m_targets[owner.m_targetIndex].transform.position;
                owner.m_distance = (owner.m_targetPosition.x - owner.transform.position.x) * (owner.m_targetPosition.x - owner.transform.position.x) + (owner.m_targetPosition.z - owner.transform.position.z) * (owner.m_targetPosition.z - owner.transform.position.z);
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
                else if (owner.m_distance > 0)
                {
                    StateMachine.Dispatch((int)ActEvent.Move);
                }
                else
                {
                    StateMachine.Dispatch((int)ActEvent.BattleAction);
                }
            }

            protected override void OnExit(State nextState)
            {
                if (nextState is Move || nextState is BattleAction)
                {
                    owner.m_actionCount--;
                }
            }
        }

        public class Move : State
        {
            protected override void OnEnter(State prevState)
            {
                Debug.Log($"{owner.name}Move");
                owner.m_stop.Value = true;
                owner.PlayAnimation("Run");
            }

            protected override void OnUpdate()
            {
                owner.m_targetPosition = owner.m_targets[owner.m_targetIndex].transform.position;
                owner.m_distance = (owner.m_targetPosition.x - owner.transform.position.x) * (owner.m_targetPosition.x - owner.transform.position.x) + (owner.m_targetPosition.z - owner.transform.position.z) * (owner.m_targetPosition.z - owner.transform.position.z);
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
                //else if (true)
                //{
                //    StateMachine.Dispatch((int)ActEvent.Wait);
                //}
                if (owner.m_distance > owner.m_normalSkill[0].SkillRange)
                {
                    Vector3 target = owner.m_targetPosition;
                    target.y = owner.transform.position.y;
                    owner.transform.LookAt(target);
                    owner.transform.position = Vector3.MoveTowards(owner.transform.position, owner.m_targetPosition, owner.m_moveSpeed * Time.deltaTime);
                }
                else
                {
                    Vector3 target = owner.m_targetPosition;
                    target.y = owner.transform.position.y;
                    owner.transform.LookAt(target);
                    StateMachine.Dispatch((int)ActEvent.BattleAction);
                }
            }

            protected override void OnExit(State nextState)
            {
                if (nextState is Wait)
                {
                    owner.m_stop.Value = false;
                }
            }
        }

        public class BattleAction : State
        {
            protected override void OnEnter(State prevState)
            {
                Debug.Log($"{owner.name}BattleAction");
                owner.PlayAnimation("Attack");
            }

            protected override void OnUpdate()
            {
                if (owner.FinishedAnimation())
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
                Debug.Log($"{owner.name}ActionEnd");
                owner.m_stop.Value = false;
                owner.m_targetObject = null;
                owner.m_look.Value = false;
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
                Debug.Log($"{owner.name}Bind");
                owner.m_stop.Value = true;
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
                owner.m_stop.Value = false;
            }
        }

        public class NoBattle : State
        {
            protected override void OnEnter(State prevState)
            {
                Debug.Log($"{owner.name}NoBattle");
                owner.m_stop.Value = false;
                owner.m_isBattle = false;
                owner.m_currentTimer = 0f;
                owner.m_actionCount = 0;
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
                Debug.Log($"{owner.name}Dead");
                owner.m_actionCount = 0;
                owner.m_stop.Value = true;
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
                    owner.m_stop.Value = false;
                }
            }
        }
    }
}