using UnityEngine;

public partial class PlayerControllerRB : MonoBehaviour
{
    public class FallStateRB : ActionStateBaseRB
    {
        public override void OnEnter(PlayerControllerRB owner)
        {
            Debug.Log("Fall");
            //owner.m_currentVelocity.x = 0.1f;
            //owner.m_currentVelocity.z = 0.1f;
            owner.PlayAnimation("Fall");
        }

        public override void OnExit(PlayerControllerRB owner)
        {
        }

        public override void OnUpdate(PlayerControllerRB owner)
        {
            if (owner.IsGround())
            {
                Debug.Log("Fall -> Land");
                owner.ChangeState(owner.landState);
            }
            else
            {
                owner.m_currentVelocity.y += owner.m_gravityScale * Physics.gravity.y * Time.deltaTime;
            }
        }
    }
}
