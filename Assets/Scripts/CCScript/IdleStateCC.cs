using UnityEngine;

public partial class PlayerControllerCC : MonoBehaviour
{
    public class IdleStateCC : ActionStateBaseCC
    {
        public override void OnEnter(PlayerControllerCC owner)
        {
            Debug.Log("Idle");
            owner.m_currentVelocity.x = 0f;
            owner.m_currentVelocity.z = 0f;
            owner.PlayAnimation("Idle", 0.2f);
        }

        public override void OnExit(PlayerControllerCC owner)
        {
        }

        public override void OnUpdate(PlayerControllerCC owner)
        {
            if (owner.m_direction.sqrMagnitude > 0.1f)
            {
                Debug.Log("Idle -> Walk");
                owner.ChangeState(owner.walkState);
            }
            if (owner.m_characterController.isGrounded)
            {
                if (Input.GetButtonDown("×button"))
                {
                    Debug.Log("Idle -> Jump");
                    owner.ChangeState(owner.jumpState);
                }
            }
            else
            {
                Debug.Log("Idle -> Fall");
                owner.ChangeState(owner.fallState);
            }
        }
    }
}
