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