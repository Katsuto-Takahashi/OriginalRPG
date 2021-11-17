using UnityEngine;

public partial class PlayerControllerRB : MonoBehaviour
{
    public class IdleStateRB : ActionStateBaseRB
    {
        public override void OnEnter(PlayerControllerRB owner)
        {
            Debug.Log("Idle");
            owner.m_currentVelocity.x = 0f;
            owner.m_currentVelocity.z = 0f;
            owner.PlayAnimation("Idle",0.2f);
        }

        public override void OnExit(PlayerControllerRB owner)
        {
        }

        public override void OnUpdate(PlayerControllerRB owner)
        {
            if (owner.m_direction.sqrMagnitude > 0.1f)
            {
                Debug.Log("Idle -> Walk");
                owner.ChangeState(owner.walkState);
            }
            if (owner.IsGround())
            {
                if (Input.GetButtonDown("L1button"))
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
