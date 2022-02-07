using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Selector : BTSelector, IGetNode
{
    //[SerializeReference, SubclassSelector]
    //BTSelector selector;

    List<Node> action = new List<Node>();
    void SetChild()
    {
        Debug.Log("selectorの子をセット");
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    action.Add(transform.GetChild(i).GetComponent<IGetNode>().GetNode());
        //}
        action = m_childNodes;
    }

    public override Node GetNode()
    {
        new BTSelector();
        SetChild();
        //return this;// = new BTSelector(action);
        return Add();
    }
}
