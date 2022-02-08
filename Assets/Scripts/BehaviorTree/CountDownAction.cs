using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CountDownAction : BTAction
{
    [SerializeField]
    int m_count = 10;

    protected override NodeState Act()
    {
        m_count--;
        Debug.Log(m_count);
        return NodeState.Failure;
    }

    public override Node GetNode()
    {
        return this;
    }
}