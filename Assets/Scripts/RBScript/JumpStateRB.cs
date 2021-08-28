using UnityEngine;

public partial class PlayerControllerRB : MonoBehaviour
{
    public class JumpStateRB : ActionStateBaseRB
    {
        float jumpTime = 0.3f;
        float time = 0.3f;
        bool isJump = false;
        public override void OnEnter(PlayerControllerRB owner)
        {
            Debug.Log("Jump");
            //owner.m_currentVelocity.x = 0f;
            //owner.m_currentVelocity.z = 0f;
            owner.m_rb.AddForce(Vector3.up * owner.m_jumpingPower, ForceMode.Impulse);
            owner.PlayAnimation("Jump");
        }

        public override void OnExit(PlayerControllerRB owner)
        {
            time = jumpTime;
            isJump = false;
        }

        public override void OnUpdate(PlayerControllerRB owner)
        {
            time -= Time.deltaTime;
            if (time < 0.3f && time > 0f)
            {
                owner.m_currentVelocity.y = owner.m_jumpingPower;
            }
            if (time < 0.2f)
            {
                isJump = true;
                Debug.Log("空中無敵");
            }
            if (isJump)
            {
                if (owner.IsGround())
                {
                    Debug.Log("Jump -> Land");
                    owner.ChangeState(owner.landState);
                }
                if (time < 0f)
                {
                    //if (owner.direction.sqrMagnitude > 0.1f)
                    //{
                    //    var dir = owner.moveForward;
                    //    dir.y = 0f;
                    //    owner.targetRotation = Quaternion.LookRotation(dir);
                    //    owner.m_currentVelocity = new Vector3(owner.nowTransform.forward.x, owner.m_currentVelocity.y, owner.nowTransform.forward.z);
                    //}
                    Debug.Log("Jump -> Fall");
                    owner.ChangeState(owner.fallState);
                }
            }
        }
    }
}
