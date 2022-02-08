using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : BTInverter
{
    List<Node> m_child = new List<Node>();
    void SetChild()
    {
        Debug.Log("Inverterの子をセット");
        m_child = m_childNodes;
        for (int i = 0; i < m_child.Count; i++)
        {
            m_child[i].GetNode();
        }
    }

    public override Node GetNode()
    {
        AddChild();
        SetChild();
        return this;
    }
}
