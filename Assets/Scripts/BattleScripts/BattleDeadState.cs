using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BCharacterStateMachine : MonoBehaviour
{
    public class BattleDeadState : BattleStateMachineBase
    {
        bool death = false;
        public override void OnEnter(BCharacterStateMachine owner)
        {
            if (owner.CompareTag("Enemy"))
            {
                owner.PlayAnimation("Idle");
            }
            else if (owner.CompareTag("Player"))
            {
                owner.PlayAnimation("Idle");
            }
            if (!death)
            {
                Debug.Log($"{owner.name}倒れた");
                death = true;
            }
        }

        public override void OnExit(BCharacterStateMachine owner)
        {
        }

        public override void OnUpdate(BCharacterStateMachine owner)
        {
        }
    }
}
