using UnityEngine;

public partial class PlayerControllerCC : MonoBehaviour
{
    public class LandStateCC : ActionStateBaseCC
    {
        float landTime = 0.2f;
        float time = 0.2f;
        bool isLand = false;
        public override void OnEnter(PlayerControllerCC owner)
        {
            Debug.Log("Land");
            owner.m_currentVelocity.x = 0f;
            owner.m_currentVelocity.z = 0f;
            owner.PlayAnimation("Land");
        }

        public override void OnExit(PlayerControllerCC owner)
        {
            time = landTime;
            isLand = false;
        }

        public override void OnUpdate(PlayerControllerCC owner)
        {
            time -= Time.deltaTime;
            if (time < 0.1f)
            {
                isLand = true;
                Debug.Log("ついたで");
            }
            if (isLand)
            {
                if (owner.m_direction.sqrMagnitude > 0.1f)
                {
                    Debug.Log("Land -> Walk");
                    owner.ChangeState(owner.walkState);
                }
                else
                {
                    Debug.Log("Land -> Idle");
                    owner.ChangeState(owner.idleState);
                }
                if (Input.GetButtonDown("×button"))
                {
                    Debug.Log("Land -> Jump");
                    owner.ChangeState(owner.jumpState);
                }
            }
        }
    }
}