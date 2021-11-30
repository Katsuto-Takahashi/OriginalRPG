﻿using UnityEngine;
using State = StateMachine<BattleCharacterStateMachine>.State;

public partial class BattleCharacterStateMachine : MonoBehaviour
{
    public class BattleCharacterState
    {
        public class Wait : State
        {
            protected override void OnEnter(State prevState)
            {
            }
            protected override void OnUpdate()
            {
            }
            protected override void OnExit(State nextState)
            {
            }
            //protected override bool OnAnimationEnd(Animator animator, int layer = 0)
            //{
            //    return base.OnAnimationEnd(animator, layer);
            //}
        }

        public class Standby : State
        {
            protected override void OnEnter(State prevState)
            {
            }

            protected override void OnUpdate()
            {
            }

            protected override void OnExit(State nextState)
            {
            }

            //protected override bool OnAnimationEnd(Animator animator, int layer = 0)
            //{
            //    return base.OnAnimationEnd(animator, layer);
            //}
        }

        public class Move : State
        {
            protected override void OnEnter(State prevState)
            {
            }

            protected override void OnUpdate()
            {
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
            }

            protected override void OnExit(State nextState)
            {
            }
        }

        public class NoBattle : State
        {
            protected override void OnEnter(State prevState)
            {
            }

            protected override void OnUpdate()
            {
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
            }

            protected override void OnExit(State nextState)
            {
            }
        }
    }
}