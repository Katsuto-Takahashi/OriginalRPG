using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<BattleEnemyStateMachine>.State;

public class BattleEnemyStateMachine : MonoBehaviour
{
    StateMachine<BattleEnemyStateMachine> m_stateMachine;
    enum ActEvent : byte
    {
        Wait,
        Standby,
        Move,
        BattleAction,
        //ActionEnd,
        Dead
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
