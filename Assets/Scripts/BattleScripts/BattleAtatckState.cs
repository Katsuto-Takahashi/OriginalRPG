using UnityEngine;

public partial class BattleStateMachine : MonoBehaviour
{
    public class BattleAtatckState : BattleStateMachineBase
    {
        public override void OnEnter(BattleStateMachine owner)
        {
            if (true)
            {
                owner.m_targetPosition = owner.m_targetCharacters[owner.m_targetNumber].GetComponent<BattleStateMachine>().m_currentPosition;
            }
            //攻撃アニメーション
            //owner.PlayAnimation("");
            //if (Vector3.Distance(owner.m_currentPosition, owner.m_targetCharacters[owner.m_targetNumber]) > 1)
            //{
            //    //近づく
            //}
        }

        public override void OnExit(BattleStateMachine owner)
        {
            owner.m_characterActionCount--;
        }

        public override void OnUpdate(BattleStateMachine owner)
        {
            if (true)
            {
                Vector3 target = owner.m_targetPosition;
                target.y = owner.m_currentPosition.y;
                owner.transform.LookAt(target);
            }
            if (true)
            {
                owner.ChangeState(owner.battleIdleState);
            }
        }
    }
}
