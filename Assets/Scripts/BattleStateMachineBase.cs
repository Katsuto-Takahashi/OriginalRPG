using UnityEngine;

public partial class BattleStateMachine : MonoBehaviour
{
    public abstract class BattleStateMachineBase
    {
        public abstract void OnEnter(BattleStateMachine owner);
        public abstract void OnUpdate(BattleStateMachine owner);
        public abstract void OnExit(BattleStateMachine owner);
    }
}
