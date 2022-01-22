using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<MovementEnemyStateMachine>.State;

public partial class MovementEnemyStateMachine : MonoBehaviour
{
    StateMachine<MovementCharacterStateMachine> m_stateMachine;
    enum ActEvent : byte
    {
        Idle,
        Walk,
        Run,
        Jump,
        Fall,
        Land,
        Stop,
        Dead
    }
}
