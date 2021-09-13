using UnityEngine;

public partial class BattleStateMachine : MonoBehaviour
{
    public class BattleAtatckState : BattleStateMachineBase
    {
        public override void OnEnter(BattleStateMachine owner)
        {
            if (owner.CompareTag("Player"))
            {
                if (owner.playerControllerCC)
                {
                    owner.playerControllerCC.enabled = false;
                }
                else if (owner.playerControllerRB)
                {
                    owner.playerControllerRB.enabled = false;
                }
            }
        }

        public override void OnExit(BattleStateMachine owner)
        {
            owner.m_characterActionCount--;
            if (owner.CompareTag("Player"))
            {
                if (owner.playerControllerCC)
                {
                    owner.playerControllerCC.enabled = true;
                }
                else if (owner.playerControllerRB)
                {
                    owner.playerControllerRB.enabled = true;
                }
            }
            owner.m_open = false;
        }

        public override void OnUpdate(BattleStateMachine owner)
        {
            owner.m_targetPosition = owner.m_targetCharacters[owner.m_targetNumber].transform.position;
            if (Vector3.Distance(owner.m_currentPosition, owner.m_targetPosition) > 1)
            {
                //近づく
                owner.autoMove.restart = true;
                owner.autoMove.ChangeTarget(owner.m_targetPosition);
                owner.autoMove.enabled = true;
            }
            else
            {
                Debug.Log(owner.name + "回転はじめ");
                //振り向き
                Vector3 target = owner.m_targetPosition;
                target.y = owner.m_currentPosition.y;
                owner.transform.LookAt(target);
                //攻撃アニメーション
                //owner.PlayAnimation("");
                Debug.Log(owner.name + "回転終わり");
                owner.ChangeState(owner.battleIdleState);
            }
        }
    }
}
