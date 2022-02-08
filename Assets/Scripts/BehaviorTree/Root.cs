using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeReference, SubclassSelector]
    Node m_myChildNode;

    List<Node> m_nodes = new List<Node>();

    void Start()
    {
        m_nodes = m_myChildNode.ChildNode;

        if (m_myChildNode != null)
        {
            Debug.Log($"子は{m_myChildNode}");
            m_myChildNode.GetNode();
            if (m_nodes != null)
            {
                for (int i = 0; i < m_nodes.Count; i++)
                {
                    Debug.Log($"{i + 1}番目の孫は{m_nodes[i]}");
                }
                Debug.Log(m_myChildNode.Result());
            }
            else
            {
                Debug.Log("孫がない");
            }
        }
        else
        {
            Debug.Log("子がない");
        }
    }

    void Update()
    {
        
    }
}
