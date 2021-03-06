using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class ChengeState : BTAction
{
    [SerializeField]
    //MovementEnemyStateMachine m_mesm;

    Enemy m_enemy;

    enum ActEvent : byte
    {
        Idle,
        Walk,
        Run,
        Chase,
        Jump,
        Fall,
        Land,
        Fly,
        Stop,
        Dead
    }

    [SerializeField]
    ActEvent m_state;

    protected override NodeState Act()
    {
        m_enemy.MESM.StateChenge((int)m_state);
        //m_mesm.StateChenge((int)m_state);
        return NodeState.Success;
    }

    public override Node GetNode()
    {
        return this;
    }
}
