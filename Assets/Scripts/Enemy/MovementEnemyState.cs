using UnityEngine;
using State = StateMachine<MovementEnemyStateMachine>.State;

public partial class MovementEnemyStateMachine : MonoBehaviour
{
    public class MovementEnemyState
    {
        public class Idle : State
        {
            protected override void OnEnter(State prevState)
            {
                Debug.Log($"{owner.name}Idleになったで");
                if (!(prevState is Idle))
                {
                    owner.PlayAnimation("Idle");
                }
                owner.m_currentVelocity = Vector3.zero;
                //owner.m_childNode.Result();
            }
            protected override void OnUpdate()
            {
                var dir = owner.m_moveForward;
                dir.y = 0f;
                owner.m_targetRotation = Quaternion.LookRotation(dir);
                //if (owner.m_childNode.Result() == BehaviorTree.NodeState.Failure)
                //{
                //    StateMachine.Dispatch((int)ActEvent.Idle);
                //}
                if (!owner.m_canMove)
                {
                    StateMachine.Dispatch((int)ActEvent.Stop);
                }
                //else if (owner.IsGround() || owner.IsSlope())
                //{
                //    if (true)//動く
                //    {
                //        if (owner.m_targetObject != null)
                //        {
                //            StateMachine.Dispatch((int)ActEvent.Run);
                //        }
                //        StateMachine.Dispatch((int)ActEvent.Walk);
                //    }
                //    if (Input.GetButtonDown("L1button"))
                //    {
                //        StateMachine.Dispatch((int)ActEvent.Jump);
                //    }
                //}
                if (!(owner.IsGround() || owner.IsSlope()))
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
                Debug.Log($"{owner.name}Walkになったで");
                //owner.PlayAnimation("Walk");
            }
            protected override void OnUpdate()
            {
                if (true)//
                {
                    StateMachine.Dispatch((int)ActEvent.Stop);
                }
                else if (owner.IsGround() || owner.IsSlope())
                {
                    if (Input.GetButtonDown("L1button"))
                    {
                        StateMachine.Dispatch((int)ActEvent.Jump);
                    }
                    if (true)//動く
                    {
                        if (Input.GetButton("R1button"))
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
                Debug.Log($"{owner.name}Runになったで");
                owner.m_movingSpeed = owner.m_runningSpeed;
                owner.PlayAnimation("Run");
            }
            protected override void OnUpdate()
            {
                if (true)//
                {
                    StateMachine.Dispatch((int)ActEvent.Stop);
                }
                else if (true)//動く
                {
                    if (owner.IsGround() || owner.IsSlope())
                    {
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

        public class Chase : State
        {
            protected override void OnEnter(State prevState)
            {
                Debug.Log($"{owner.name}Chaseになったで");
                owner.m_movingSpeed = owner.m_runningSpeed;
                owner.PlayAnimation("Run");
            }
            protected override void OnUpdate()
            {
                if (true)//
                {
                    StateMachine.Dispatch((int)ActEvent.Stop);
                }
                else if (true)//動く
                {
                    if (owner.IsGround() || owner.IsSlope())
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
                owner.m_movingSpeed = owner.m_walkingSpeed;
            }
        }

        public class Jump : State
        {
            protected override void OnEnter(State prevState)
            {
                Debug.Log($"{owner.name}Jumpになったで");
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
                    if (true)//動く
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
                Debug.Log($"{owner.name}Fallになったで");
                //owner.m_currentVelocity.x = 0f;
                //owner.m_currentVelocity.z = 0f;
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
                    if (true)//動く
                    {
                        var dir = owner.m_moveForward;
                        dir.y = 0f;
                        owner.m_targetRotation = Quaternion.LookRotation(dir);
                        owner.m_currentVelocity = new Vector3(owner.m_moveForward.x, owner.m_currentVelocity.y, owner.m_moveForward.z);
                    }
                    else
                    {
                        if (Mathf.Abs(owner.m_currentVelocity.x) > 0.1f)
                        {
                            owner.m_currentVelocity.x *= 0.99f;
                        }
                        if (Mathf.Abs(owner.m_currentVelocity.z) > 0.1f)
                        {
                            owner.m_currentVelocity.z *= 0.99f;
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
                Debug.Log($"{owner.name}Landになったで");
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
                //else if (true)//動く
                //{
                //    if (Input.GetButton("R1button"))
                //    {
                //        StateMachine.Dispatch((int)ActEvent.Run);
                //    }
                //    StateMachine.Dispatch((int)ActEvent.Walk);
                //}
            }
            protected override void OnExit(State nextState)
            {
            }
        }

        public class Fly : State
        {
            protected override void OnEnter(State prevState)
            {
                Debug.Log($"{owner.name}Flyになったで");
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
                    if (true)//動く
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

        public class Stop : State
        {
            protected override void OnEnter(State prevState)
            {
                Debug.Log($"{owner.name}Stopになったで");
                //owner.PlayAnimation("Stop");
            }
            protected override void OnUpdate()
            {
                if (owner.m_canMove)
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
