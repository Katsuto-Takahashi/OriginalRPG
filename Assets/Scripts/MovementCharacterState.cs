﻿using System.Collections;
using System.Collections.Generic;
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
                owner.PlayAnimation("Idle");
                owner.m_currentVelocity = Vector3.zero;
            }
            protected override void OnUpdate()
            {
                if (owner.IsGround())
                {
                    if (Input.GetButtonDown("L1button"))
                    {
                        StateMachine.Dispatch((int)ActEvent.Jump);
                    }
                    if (owner.m_inputDirection.sqrMagnitude > 0.1f)
                    {
                        if (Input.GetButtonDown("R1button"))
                        {
                            StateMachine.Dispatch((int)ActEvent.Run);
                        }
                        StateMachine.Dispatch((int)ActEvent.Walk);
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
                owner.PlayAnimation("Walk");
            }
            protected override void OnUpdate()
            {
                if (owner.IsGround())
                {
                    if (Input.GetButtonDown("L1button"))
                    {
                        StateMachine.Dispatch((int)ActEvent.Jump);
                    }
                    if (owner.m_inputDirection.sqrMagnitude > 0.1f)
                    {
                        if (Input.GetButtonDown("R1button"))
                        {
                            StateMachine.Dispatch((int)ActEvent.Run);
                        }
                        var dir = owner.m_moveForward;
                        dir.y = 0f;
                        owner.m_targetRotation = Quaternion.LookRotation(dir);
                        owner.m_currentVelocity = new Vector3(owner.m_myTransform.forward.x, owner.m_currentVelocity.y, owner.m_myTransform.forward.z);
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
                if (Input.GetButton("R1button"))
                {
                    owner.PlayAnimation("Run");
                }
            }
            protected override void OnUpdate()
            {
                if (owner.m_inputDirection.sqrMagnitude > 0.1f)
                {
                    if (owner.IsGround())
                    {
                        if (Input.GetButtonDown("L1button"))
                        {
                            StateMachine.Dispatch((int)ActEvent.Jump);
                        }
                        if (Input.GetButtonUp("R1button"))
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
            }
        }

        public class Jump : State
        {
            protected override void OnEnter(State prevState)
            {
                owner.PlayAnimation("Jump");
                owner.m_rigidbody.AddForce(Vector3.up * owner.m_jumpingPower, ForceMode.Impulse);
            }
            protected override void OnUpdate()
            {
                if (owner.FinishedAnimation())
                {
                    StateMachine.Dispatch((int)ActEvent.Fall);
                }
                else
                {
                    owner.m_currentVelocity.y = owner.m_jumpingPower;
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
                if (owner.IsGround())
                {
                    StateMachine.Dispatch((int)ActEvent.Land);
                }
                owner.m_currentVelocity.y += owner.m_gravityScale * Physics.gravity.y * Time.deltaTime;
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
                if (owner.m_inputDirection.sqrMagnitude > 0.1f)
                {
                    if (Input.GetButtonDown("R1button"))
                    {
                        StateMachine.Dispatch((int)ActEvent.Run);
                    }
                    StateMachine.Dispatch((int)ActEvent.Walk);
                }
                else if (owner.FinishedAnimation())
                {
                    StateMachine.Dispatch((int)ActEvent.Idle);
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
