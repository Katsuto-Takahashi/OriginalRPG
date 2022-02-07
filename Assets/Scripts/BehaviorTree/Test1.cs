using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    [SerializeReference, SubclassSelector]
    Node repeater = new Re();

    IGetNode getNode;

    Node m_child;
    //[SerializeReference, SubclassSelector]
    List<Node> m_nodes = new List<Node>();

    void SetChild()
    {
        m_child = repeater.ChildNode[0];
    }

    public Node GetNode()
    {
        SetChild();
        return transform.GetChild(0).GetComponent<IGetNode>().GetNode();
    }
    void Start()
    {
        m_child = repeater.GetNode();
        m_nodes = repeater.ChildNode;

        //repeater = new BTRepeater(m_child);

        if (m_nodes != null)
        {
            Debug.Log(m_child);
            for (int i = 0; i < m_child.ChildNode.Count; i++)
            {
                m_child.ChildNode[i].GetNode();
            }
            if (m_child.ChildNode != null)
            {
                Debug.Log(m_child.Result());
            }
            else
            {
                Debug.Log("childnull");
            }
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

[System.Serializable]
public class Re : BTRepeater, IGetNode
{
    //[SerializeReference, SubclassSelector]
    List<Node> action = new List<Node>();
    void SetChild()
    {
        Debug.Log("Repeaterの子をセット");
        for (int i = 0; i < SetNode().Count; i++)
        {
            Debug.Log($"子は{SetNode().Count}");
        }
        action = SetNode();
    }

    public override Node GetNode()
    {
        new BTRepeater();
        SetChild();
        //return this;// = new BTSelector(action);
        //Debug.Log(new BTRepeater());
        for (int i = 0; i < action.Count; i++)
        {
            action[0].GetNode();
        }
        return action[0];
    }
}