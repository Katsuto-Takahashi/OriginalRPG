using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BattleStateMachine : MonoBehaviour
{
    public float m_actionTimer;
    float m_countTimer;
    public bool m_battle = false;

    public Vector3 m_currentPosition;

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
        m_currentPosition = this.transform.position;
        currentState.OnUpdate(this);
    }
    void ChangeState(BattleStateMachineBase nextState)
    {
        currentState?.OnExit(this);
        nextState.OnEnter(this);
        currentState = nextState;
    }
    void PlayAnimation(string stateName, float transitionDuration = 0.1f)
    {
        animator.CrossFadeInFixedTime(stateName, transitionDuration);
    }
}
