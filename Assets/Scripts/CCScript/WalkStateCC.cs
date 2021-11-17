using UnityEngine;

public partial class PlayerControllerCC : MonoBehaviour
{
    public class WalkStateCC : ActionStateBaseCC
    {
        public override void OnEnter(PlayerControllerCC owner)
        {
            //Debug.Log("Walk");
            owner.PlayAnimation("Walk");
        }

        public override void OnExit(PlayerControllerCC owner)
        {
        }

        public override void OnUpdate(PlayerControllerCC owner)
        {
            if (owner.m_direction.sqrMagnitude > 0.1f)
            {
                var dir = owner.m_moveForward;
                dir.y = 0f;
                owner.m_targetRotation = Quaternion.LookRotation(dir);
                owner.m_currentVelocity = new Vector3(owner.m_nowTransform.forward.x, owner.m_currentVelocity.y, owner.m_nowTransform.forward.z);
            }
            else
            {
                //Debug.Log("Walk -> Idle");
                owner.ChangeState(owner.idleState);
            }
            if (owner.m_characterController.isGrounded)
            {
                if (Input.GetButtonDown("L1button"))
                {
                    //Debug.Log("Walk -> Jump");
                    owner.ChangeState(owner.jumpState);
                }
            }
            else
            {
                //Debug.Log("Walk -> Fall");
                owner.ChangeState(owner.fallState);
            }
        }
    }
}
