using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree
{
    [SerializeField]
    List<Node> m_nodes = new List<Node>();
}

public class Node
{

}

public enum NodeState
{
    Running,
    Success,
    Failure
}
//root
////repeter(≓update),
///sequence(順に実行し初めに失敗した時点でfalse,全て成功ならtrue)
///selector(順に実行し初めに成功した子をtrue,全て失敗ならfalse)
//////action(処理),conditional(条件判定)