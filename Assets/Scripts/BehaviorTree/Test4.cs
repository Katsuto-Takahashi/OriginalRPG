using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test4 : MonoBehaviour, IGetNode
{
    [SerializeReference, SubclassSelector]
    Node m_checker = new BTChecker();
    public Node GetNode()
    {
        return m_checker;
    }
}
[System.Serializable]
public class BTChecker : BTConditional
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
}