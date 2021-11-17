using UnityEngine;

public partial class PlayerControllerCC : MonoBehaviour
{
    public class FallStateCC : ActionStateBaseCC
    {
        public override void OnEnter(PlayerControllerCC owner)
        {
            //Debug.Log("Fall");
            owner.PlayAnimation("Fall");
        }

        public override void OnExit(PlayerControllerCC owner)
        {
        }

        public override void OnUpdate(PlayerControllerCC owner)
        {
            if (owner.m_characterController.isGrounded)
            {
                //Debug.Log("Fall -> Land");
                owner.ChangeState(owner.landState);
            }
            else
            {
            }
        }
    }
}

