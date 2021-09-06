using UnityEngine;

public partial class BattleStateMachine : MonoBehaviour
{
    public class BattleAtatckState : BattleStateMachineBase
    {
        public override void OnEnter(BattleStateMachine owner)
        {
            //攻撃アニメーション
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
