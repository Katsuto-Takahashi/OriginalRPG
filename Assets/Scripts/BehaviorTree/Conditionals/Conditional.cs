using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

[System.Serializable]
public class Conditional : BTConditional
{
    [SerializeField]
    bool m_check = false;
    public void C(bool cc)
    {
        m_check = cc;
    }

    protected override NodeState Check()
    {
        Debug.Log($"判定結果{m_check}");
        if (m_check) return NodeState.Success;
        return NodeState.Failure;
    }

    public override Node GetNode()
    {
        return this;
    }
}