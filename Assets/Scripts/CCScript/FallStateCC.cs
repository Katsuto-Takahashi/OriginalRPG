using UnityEngine;

public partial class PlayerControllerCC : MonoBehaviour
{
    public class FallStateCC : ActionStateBaseCC
    {
        public override void OnEnter(PlayerControllerCC owner)
        {
            Debug.Log("Fall");
            //owner.m_currentVelocity.x = 0.1f;
            //owner.m_currentVelocity.z = 0.1f;
            owner.PlayAnimation("Fall");
        }

        public override void OnExit(PlayerControllerCC owner)
        {
            owner.PlayAnimation("Land");
        }

        public override void OnUpdate(PlayerControllerCC owner)
        {
            if (owner.m_characterController.isGrounded)
            {
                if (owner.m_direction.sqrMagnitude > 0.1f)
                {
                    Debug.Log("Fall -> Walk");
                    owner.ChangeState(owner.walkState);
                }
                else
                {
                    Debug.Log("Fall -> Idle");
                    owner.ChangeState(owner.idleState);
                }
                if (Input.GetButtonDown("×button"))
                {
                    Debug.Log("Fall -> Jump");
                    owner.ChangeState(owner.jumpState);
                }
            }
            else
            {
                //owner.m_currentVelocity.y = 0f;
            }
        }
    }
}

