using UnityEngine;
using State = StateMachine<CharacterStateMachine>.State;

public partial class CharacterStateMachine : MonoBehaviour
{
    public class BattleCharacterState
    {
        public class WaitState : State
        {
            protected override void OnEnter(State prevState)
            {
                Debug.Log("wait");
            }
            protected override void OnUpdate()
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StateMachine.Dispatch((int)ActEvent.Standby);
                }
            }
            protected override void OnExit(State nextState)
            {
            }
            protected override bool OnAnimationEnd(Animator animator, int layer = 0)
            {
                return base.OnAnimationEnd(animator, layer);
            }
        }
        public class StandbyState : State
        {
            protected override void OnEnter(State prevState)
            {
                Debug.Log("standby");
            }

            protected override void OnUpdate()
            {
            }

            protected override void OnExit(State nextState)
            {
            }

            protected override bool OnAnimationEnd(Animator animator, int layer = 0)
            {
                return base.OnAnimationEnd(animator, layer);
            }
        }
        public class MoveState : State
        {
            protected override void OnEnter(State prevState)
            {
            }
        }
    }
}