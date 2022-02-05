using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test3 : MonoBehaviour, IGetNode
{
    [SerializeReference, SubclassSelector]
    Node m_countDownAction = new CountDownAction();
    public Node GetNode()
    {
        return m_countDownAction;
    }
}
[System.Serializable]
public class CountDownAction : BTAction
{
    [SerializeField]
    int m_count = 10;

    protected override NodeState Act()
    {
        m_count--;
        Debug.Log(m_count);
        return NodeState.Success;
    }
}