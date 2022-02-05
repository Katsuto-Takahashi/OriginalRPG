using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour , IGetNode
{
    BTSelector selector;

    List<Node> action = new List<Node>();
    void SetAction()
    {
        Debug.Log("selectorの子をセット");
        for (int i = 0; i < transform.childCount; i++)
        {
            action.Add(transform.GetChild(i).GetComponent<IGetNode>().GetNode());
        }
    }

    public Node GetNode()
    {
        SetAction();
        return selector = new BTSelector(action);
    }
}
