using UnityEngine;
using State = StateMachine<MovementCharacterStateMachine>.State;

public partial class MovementCharacterStateMachine : MonoBehaviour
{
    public class MovementCharacterState : MonoBehaviour
    {
        public class Idle : State
        {
            protected override void OnEnter(State prevState)
            {
                if (owner.m_canOperation)
                {
                    owner.PlayAnimation("Idle");
                    owner.m_currentVelocity = Vector3.zero;
                }
            }
            protected override void OnUpdate()
            {
                if (owner.IsGround() || owner.IsSlope())
                {
                    if (owner.m_playAction.Value)
                    {
                        StateMachine.Dispatch((int)ActEvent.BattleAction);
                    }
                    if (owner.m_inputDirection.sqrMagnitude > 0.1f)
                    {
                        if (InputController.Instance.Run())
                        {
                            StateMachine.Dispatch((int)ActEvent.Run);
                        }
                        StateMachine.Dispatch((int)ActEvent.Walk);
                    }
                    if (InputController.Instance.Jump())
                    {
                        StateMachine.Dispatch((int)ActEvent.Jump);
                    }
                }
                else
                {
                    StateMachine.Dispatch((int)ActEvent.Fall);
                }
            }
            protected override void OnExit(State nextState)
            {
            }
        }

        public class Walk : State
        {
            protected override void OnEnter(State prevState)
            {
                Debug.Log("Walk");
                owner.PlayAnimation("Walk");
            }
            protected override void OnUpdate()
            {
                if (owner.IsGround() || owner.IsSlope())
                {
                    if (owner.m_playAction.Value)
                    {
                        StateMachine.Dispatch((int)ActEvent.BattleAction);
                    }
                    if (InputController.Instance.Jump())
                    {
                        StateMachine.Dispatch((int)ActEvent.Jump);
                    }
                    if (owner.m_inputDirection.sqrMagnitude > 0.1f)
                    {
                        if (InputController.Instance.Run())
                        {
                            StateMachine.Dispatch((int)ActEvent.Run);
                        }
                        var dir = owner.m_moveForward;
                        dir.y = 0f;
                        owner.m_targetRotation = Quaternion.LookRotation(dir);
                        owner.m_currentVelocity = new Vector3(owner.m_moveForward.x, owner.m_currentVelocity.y, owner.m_moveForward.z);
                    }
                    else
                    {
                        StateMachine.Dispatch((int)ActEvent.Idle);
                    }
                }
                else
                {
                    StateMachine.Dispatch((int)ActEvent.Fall);
                }
            }
            protected override void OnExit(State nextState)
            {
                
            }
        }

        public class Run : State
        {
            protected override void OnEnter(State prevState)
            {
                owner.m_movingSpeed = owner.m_runningSpeed;
                owner.PlayAnimation("Run");
            }
            protected override void OnUpdate()
            {
                //if (!owner.m_canOperation)
                //{
                //    StateMachine.Dispatch((int)ActEvent.BattleAction);
                //}
                //else 
                if (owner.m_inputDirection.sqrMagnitude > 0.1f)
                {
                    if (owner.IsGround() || owner.IsSlope())
                    {
                        if (InputController.Instance.Jump())
                        {
                            StateMachine.Dispatch((int)ActEvent.Jump);
                        }
                        if (!InputController.Instance.Run())
                        {
                            StateMachine.Dispatch((int)ActEvent.Walk);
                        }
                        var dir = owner.m_moveForward;
                        dir.y = 0f;
                        owner.m_targetRotation = Quaternion.LookRotation(dir);
                        owner.m_currentVelocity = new Vector3(owner.m_myTransform.forward.x, owner.m_currentVelocity.y, owner.m_myTransform.forward.z);
                    }
                    else
                    {
                        StateMachine.Dispatch((int)ActEvent.Fall);
                    }
                }
                else
                {
                    StateMachine.Dispatch((int)ActEvent.Idle);
                }
            }
            protected override void OnExit(State nextState)
            {
                owner.m_movingSpeed = owner.m_walkingSpeed;
            }
        }

        public class Jump : State
        {
            protected override void OnEnter(State prevState)
            {
                owner.m_currentVelocity = Vector3.up * owner.m_jumpingPower;
                owner.PlayAnimation("Jump");
            }
            protected override void OnUpdate()
            {
                if (owner.FinishedAnimation())
                {
                    StateMachine.Dispatch((int)ActEvent.Fall);
                }
                else
                {
                    if (owner.m_inputDirection.sqrMagnitude > 0.1f)
                    {
                        var dir = owner.m_moveForward;
                        dir.y = 0f;
                        owner.m_targetRotation = Quaternion.LookRotation(dir);
                        owner.m_currentVelocity = new Vector3(owner.m_myTransform.forward.x, owner.m_currentVelocity.y, owner.m_myTransform.forward.z);
                    }
                }
            }
            protected override void OnExit(State nextState)
            {
            }
        }

        public class Fall : State
        {
            protected override void OnEnter(State prevState)
            {
                owner.PlayAnimation("Fall");
            }
            protected override void OnUpdate()
            {
                if (owner.IsGround() || owner.IsSlope())
                {
                    StateMachine.Dispatch((int)ActEvent.Land);
                }
                else
                {
                    owner.m_currentVelocity.y += Physics.gravity.y * Time.deltaTime;
                    if (owner.m_inputDirection.sqrMagnitude > 0.1f)
                    {
                        var dir = owner.m_moveForward;
                        dir.y = 0f;
                        owner.m_targetRotation = Quaternion.LookRotation(dir);
                        owner.m_currentVelocity = new Vector3(owner.m_moveForward.x , owner.m_currentVelocity.y, owner.m_moveForward.z);
                    }
                    else
                    {
                        if (Mathf.Abs(owner.m_currentVelocity.x) > 0.1f)
                        {
                            owner.m_currentVelocity.x = Mathf.MoveTowards(owner.m_currentVelocity.x, 0f, Time.deltaTime);
                        }
                        if (Mathf.Abs(owner.m_currentVelocity.z) > 0.1f)
                        {
                            owner.m_currentVelocity.z = Mathf.MoveTowards(owner.m_currentVelocity.z, 0f, Time.deltaTime); ;
                        }
                    }
                }
            }
            protected override void OnExit(State nextState)
            {
            }
        }

        public class Land : State
        {
            protected override void OnEnter(State prevState)
            {
                owner.PlayAnimation("Land");
                owner.m_currentVelocity.x = 0f;
                owner.m_currentVelocity.z = 0f;
            }
            protected override void OnUpdate()
            {
                if (owner.FinishedAnimation())
                {
                    StateMachine.Dispatch((int)ActEvent.Idle);
                }
                else if (owner.m_inputDirection.sqrMagnitude > 0.1f)
                {
                    if (InputController.Instance.Run())
                    {
                        StateMachine.Dispatch((int)ActEvent.Run);
                    }
                    StateMachine.Dispatch((int)ActEvent.Walk);
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
                owner.PlayAnimation("Attack");

                owner.m_currentVelocity.x = 0f;
                owner.m_currentVelocity.z = 0f;
            }
            protected override void OnUpdate()
            {
                if (owner.FinishedAnimation())
                {
                    owner.m_playAction.Value = false;
                    if (owner.m_canOperation)
                    {
                        StateMachine.Dispatch((int)ActEvent.Idle);
                    }
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
            }
            protected override void OnExit(State nextState)
            {
            }
        }
    }
}
