using UnityEngine;

public partial class BCharacterStateMachine : MonoBehaviour
{
    public abstract class BattleStateMachineBase
    {
        public abstract void OnEnter(BCharacterStateMachine owner);
        public abstract void OnUpdate(BCharacterStateMachine owner);
        public abstract void OnExit(BCharacterStateMachine owner);
    }
}
