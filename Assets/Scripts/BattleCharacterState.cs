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
                owner.m_isBattle = true;
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

        public class Move : State
        {
            protected override void OnEnter(State prevState)
            {
                owner.PlayAnimation("Run");
            }

            protected override void OnUpdate()
            {
                if (owner.m_nowHP < 1)
                {
                    StateMachine.Dispatch((int)ActEvent.Dead);
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
            }
        }

        public class BattleAction : State
        {
            protected override void OnEnter(State prevState)
            {
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

        public class NoBattle : State
        {
            protected override void OnEnter(State prevState)
            {
                owner.m_isBattle = false;
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

        public class Dead : State
        {
            protected override void OnEnter(State prevState)
            {
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
            }
        }
    }
}