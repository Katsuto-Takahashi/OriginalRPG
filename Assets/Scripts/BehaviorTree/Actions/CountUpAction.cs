using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

[System.Serializable]
public class CountUpAction : BTAction
{
    [SerializeField]
    int m_count = 10;

    protected override NodeState Act()
    {
        A();
        m_count++;
        Debug.Log(m_count);
        return NodeState.Success;
    }

    public void A()
    {

    }

    public override Node GetNode()
    {
        return this;
    }
}