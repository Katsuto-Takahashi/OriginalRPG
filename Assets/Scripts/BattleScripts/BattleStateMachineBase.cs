using UnityEngine;

public partial class BattleCharacterStateMachine : MonoBehaviour
{
    public abstract class BattleStateMachineBase
    {
        public abstract void OnEnter(BattleCharacterStateMachine owner);
        public abstract void OnUpdate(BattleCharacterStateMachine owner);
        public abstract void OnExit(BattleCharacterStateMachine owner);
    }
}
