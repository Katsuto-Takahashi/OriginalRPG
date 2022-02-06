using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test5 : MonoBehaviour, IGetNode
{
    BTSequence sequence;

    List<Node> m_child = new List<Node>();
    void SetChild()
    {
        Debug.Log("sequenceの子をセット");
        for (int i = 0; i < transform.childCount; i++)
        {
            m_child.Add(transform.GetChild(i).GetComponent<IGetNode>().GetNode());
        }
    }

    public Node GetNode()
    {
        SetChild();
        return sequence = new BTSequence(m_child);
    }
}
