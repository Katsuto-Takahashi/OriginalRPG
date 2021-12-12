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
                if (owner.m_nowHP < 1)
                {
                    StateMachine.Dispatch((int)ActEvent.Dead);
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
                if (owner.m_nowHP < 1)
                {
                    StateMachine.Dispatch((int)ActEvent.Dead);
                }
                if (true)
                {
                    StateMachine.Dispatch((int)ActEvent.Move);
                }
            }

            protected override void OnExit(State nextState)
            {
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
                if (owner.m_nowHP < 1)
                {
                    StateMachine.Dispatch((int)ActEvent.Dead);
                }
                if (true)
                {
                    StateMachine.Dispatch((int)ActEvent.Wait);
                }
                if (owner.m_distance > 2f)
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
                Debug.Log("BattleAction");
            }

            protected override void OnUpdate()
            {
                if (owner.m_nowHP < 1)
                {
                    StateMachine.Dispatch((int)ActEvent.Dead);
                }
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
                if (owner.m_nowHP < 1)
                {
                    StateMachine.Dispatch((int)ActEvent.Dead);
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
                owner.m_isBattle = false;
            }

            protected override void OnUpdate()
            {
                if (owner.m_nowHP < 1)
                {
                    StateMachine.Dispatch((int)ActEvent.Dead);
                }
                if (owner.m_isBattle)
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
                owner.m_stop.Value = true;
                owner.PlayAnimation("");
            }

            protected override void OnUpdate()
            {
                if (owner.m_nowHP > 0)
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
                owner.m_stop.Value = false;
            }
        }
    }
}