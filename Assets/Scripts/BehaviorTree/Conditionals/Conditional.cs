using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

[System.Serializable]
public class Conditional : BTConditional
{
    [SerializeField]
    MovementEnemyStateMachine m_mesm;

    protected override NodeState Check()
    {
        if (m_mesm.TargetObject == null) return NodeState.Failure;
        return NodeState.Success;
    }

    public override Node GetNode()
    {
        return this;
    }
}