using UnityEngine;

public partial class PlayerControllerCC : MonoBehaviour
{
    public class JumpStateCC : ActionStateBaseCC
    {
        float jumpTime = 0.3f;
        float time = 0.3f;
        bool isJump = false;
        public override void OnEnter(PlayerControllerCC owner)
        {
            Debug.Log("Jump");
            //owner.m_currentVelocity.x = 0f;
            //owner.m_currentVelocity.z = 0f;
            owner.m_currentVelocity.y = owner.m_jumpingPower;
            owner.PlayAnimation("Jump");
        }

        public override void OnExit(PlayerControllerCC owner)
        {
            time = jumpTime;
            isJump = false;
        }

        public override void OnUpdate(PlayerControllerCC owner)
        {
            time -= Time.deltaTime;

            if (time < 0.2f)
            {
                isJump = true;
                Debug.Log("空中無敵");
            }
            if (isJump)
            {
                if (owner.m_characterController.isGrounded)
                {
                    if (owner.m_direction.sqrMagnitude > 0.1f)
                    {
                        owner.PlayAnimation("Land");
                        Debug.Log("Jump -> Walk");
                        owner.ChangeState(owner.walkState);
                    }
                    else
                    {
                        owner.PlayAnimation("Land");
                        Debug.Log("Jump -> Idle");
                        owner.ChangeState(owner.idleState);
                    }
                }
                if (time < 0f)
                {
                    //if (owner.m_direction.sqrMagnitude > 0.1f)
                    //{
                    //    var dir = owner.m_moveForward;
                    //    dir.y = 0f;
                    //    owner.m_targetRotation = Quaternion.LookRotation(dir);
                    //    owner.m_currentVelocity = new Vector3(owner.m_nowTransform.forward.x, owner.m_currentVelocity.y, owner.m_nowTransform.forward.z);
                    //}
                    Debug.Log("Jump -> Fall");
                    owner.ChangeState(owner.fallState);
                }
            }
        }
    }
}
