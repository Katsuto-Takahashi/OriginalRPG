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
                Debug.Log("Standby");
            }

            protected override void OnUpdate()
            {
                if (owner.m_isDead)
                {
                    StateMachine.Dispatch((int)ActEvent.Dead);
                }
                else if (!owner.m_isBattle)
                {
                    StateMachine.Dispatch((int)ActEvent.NoBattle);
                }
                else if (owner.m_distance > 0)
                {
                    StateMachine.Dispatch((int)ActEvent.Move);
                }
                //else
                //{
                //    StateMachine.Dispatch((int)ActEvent.BattleAction);
                //}
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
                Debug.Log("Move");
                owner.m_stop.Value = true;
                owner.PlayAnimation("Run");
            }

            protected override void OnUpdate()
            {
                if (owner.m_isDead)
                {
                    StateMachine.Dispatch((int)ActEvent.Dead);
                }
                else if (!owner.m_isBattle)
                {
                    StateMachine.Dispatch((int)ActEvent.NoBattle);
                }
                else if (true)
                {
                    StateMachine.Dispatch((int)ActEvent.Wait);
                }
                if (owner.m_distance > owner.m_normalSkill[0].AttackRange)
                {
                    //Vector3 target = owner.m_targetPosition;
                    //target.y = 0f;
                    //owner.transform.position = new Vector3(owner.transform.position.x, 0f, owner.transform.position.z);
                    //owner.transform.LookAt(target);
                    //owner.transform.position = Vector3.MoveTowards(owner.transform.position, owner.m_targetPosition, owner.m_moveSpeed * Time.deltaTime);
                }
                else
                {
                    //Vector3 target = owner.m_targetPosition;
                    //target.y = 0f;
                    //owner.transform.position = new Vector3(owner.transform.position.x, 0f, owner.transform.position.z);
                    //owner.transform.LookAt(target);
                    //StateMachine.Dispatch((int)ActEvent.BattleAction);
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
                Debug.Log("BattleAction");
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
                Debug.Log("ActionEnd");
                owner.m_stop.Value = false;
            }

            protected override void OnUpdate()
            {
                if (owner.m_isDead)
                {
                    StateMachine.Dispatch((int)ActEvent.Dead);
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

        public class NoBattle : State
        {
            protected override void OnEnter(State prevState)
            {
                Debug.Log("NoBattle");
                owner.m_stop.Value = false;
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