using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour//, IGetNode
{
    //[SerializeReference, SubclassSelector]
    //CountUpAction m_countUpAction = new CountUpAction();
    //public Node GetNode()
    //{
    //    return m_countUpAction;
    //}
}

[System.Serializable]
public class CountUpAction : BTAction, IGetNode
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
