using UnityEngine;

public partial class PlayerControllerRB : MonoBehaviour
{
    public class WalkStateRB : ActionStateBaseRB
    {
        public override void OnEnter(PlayerControllerRB owner)
        {
            Debug.Log("Walk");
            owner.PlayAnimation("Walk", 0.2f);
        }

        public override void OnExit(PlayerControllerRB owner)
        {
        }

        public override void OnUpdate(PlayerControllerRB owner)
        {
            if (owner.m_direction.sqrMagnitude > 0.1f)
            {
                if (owner.IsGround())
                {
                    var dir = owner.m_moveForward;
                    dir.y = 0f;
                    owner.m_targetRotation = Quaternion.LookRotation(dir);
                    owner.m_currentVelocity = new Vector3(owner.m_nowTransform.forward.x, owner.m_currentVelocity.y, owner.m_nowTransform.forward.z);
                    if (Input.GetButtonDown("×button"))
                    {
                        owner.ChangeState(owner.jumpState);
                    }
                }
                else
                {
                    owner.ChangeState(owner.fallState);
                }
            }
            else
            {
                if (owner.IsGround())
                {
                    if (Input.GetButtonDown("×button"))
                    {
                        Debug.Log("Walk -> Jump");
                        owner.ChangeState(owner.jumpState);
                    }
                    else
                    {
                        Debug.Log("Walk -> Idle");
                        owner.ChangeState(owner.idleState);
                    }
                    //owner.ChangeState(owner.idleState);
                }
                else
                {
                    Debug.Log("Walk -> Fall");
                    owner.ChangeState(owner.fallState);
                }
            }
        }
    }
}
