using UnityEngine;

public partial class BattleStateMachine : MonoBehaviour
{
    public class BattleAtatckState : BattleStateMachineBase
    {
        public override void OnEnter(BattleStateMachine owner)
        {
            //攻撃アニメーション
            //owner.PlayAnimation("");
            //if (Vector3.Distance(owner.m_currentPosition, owner.m_targetCharacters[owner.m_targetNumber]) > 1)
            //{
            //    //近づく
            //}
        }

        public override void OnExit(BattleStateMachine owner)
        {
        }

        public override void OnUpdate(BattleStateMachine owner)
        {
            if (true)
            {
                owner.ChangeState(owner.battleIdleState);
            }
        }
    }
}
