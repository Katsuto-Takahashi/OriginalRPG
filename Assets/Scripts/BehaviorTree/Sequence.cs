using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sequence : BTSequence, IGetNode
{
    List<Node> m_child = new List<Node>();
    void SetChild()
    {
        //testSequence = new TestSequence(m_child);
        //Debug.Log("sequenceの子をセット");
        //for (int i = 0; i < transform.childCount; i++)
        //{
        m_child = m_childNodes;
        //}
    }

    public override Node GetNode()
    {
        new Sequence();
        SetChild();
        return Add();
    }
}