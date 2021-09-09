﻿using UnityEngine;

public partial class BattleStateMachine : MonoBehaviour
{
    public class BattleWaitActionState : BattleStateMachineBase
    {
        public override void OnEnter(BattleStateMachine owner)
        {
            if (owner.CompareTag("Enemy"))
            {
                owner.m_targetNumber = Random.Range(0, owner.m_targetCharacters.Count);
            }
            //else if (owner.CompareTag("Player"))
            //{

            //}
        }

        public override void OnExit(BattleStateMachine owner)
        {
        }

        public override void OnUpdate(BattleStateMachine owner)
        {
            if (true)
            {
                if (owner.CompareTag("Enemy"))
                {
                    Debug.Log("敵の攻撃");
                    owner.ChangeState(owner.battleAtatckState);
                }
                else if (owner.CompareTag("Player"))
                {
                    Debug.Log("攻撃");
                    owner.ChangeState(owner.battleAtatckState);
                }
                
            }
        }
    }
}
