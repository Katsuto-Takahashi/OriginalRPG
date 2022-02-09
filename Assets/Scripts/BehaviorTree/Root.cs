using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Root : MonoBehaviour
{
    [SerializeReference, SubclassSelector]
    Node m_myChildNode;

    void Start()
    {
        if (m_myChildNode != null)
        {
            Debug.Log($"子は{m_myChildNode}");
            m_myChildNode.GetNode();
            Debug.Log(m_myChildNode.Result());
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
