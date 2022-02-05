using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour, IGetNode
{
    BTRepeater repeater;

    Node m_child;
    public Node GetNode()
    {
        return transform.GetChild(0).GetComponent<IGetNode>().GetNode();
    }
    void Start()
    {
        m_child = GetNode();

        repeater = new BTRepeater(m_child);

        if (m_child != null)
        {
            Debug.Log("do not null");
            Debug.Log(repeater.Result());
        }
        else
        {
            Debug.Log("null");
        }
    }

    //void Update()
    //{
    //    if (repeater.Result() == NodeState.Running)
    //    {

    //    }
    //}
}
