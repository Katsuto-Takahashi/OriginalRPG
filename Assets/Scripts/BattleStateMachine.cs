using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BattleStateMachine : MonoBehaviour
{
    public float m_actionTimer;
    float m_countTimer;

    BattleStateMachineBase currentState;
    BattleIdleState battleIdleState = new BattleIdleState();
    BattleAtatckState battleAtatckState = new BattleAtatckState();

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();

        ChangeState(battleIdleState);
    }

    void Update()
    {
        currentState.OnUpdate(this);
    }
    void ChangeState(BattleStateMachineBase nextState)
    {
        currentState?.OnExit(this);
        nextState.OnEnter(this);
        currentState = nextState;
    }
}
