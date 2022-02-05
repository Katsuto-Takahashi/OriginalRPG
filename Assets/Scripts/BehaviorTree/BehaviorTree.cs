using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree :MonoBehaviour
{
    [SerializeField]
    protected Node[] m_nodes = new Node[] { };
}

public interface IReturnResult
{
    NodeState Result();
}

public abstract class Node : IReturnResult
{
    protected NodeState m_currentState;
    public NodeState CurrentState { get => m_currentState; }

    public virtual NodeState Result()
    {
        return m_currentState;
    }

    //public Node() { }
}

public enum NodeState
{
    Running,
    Success,
    Failure
}

public class BTAction : Node
{
    int a = 0;
    public override NodeState Result()
    {
        return Act();
    }

    //実行してほしいこと
    /// <summary>実行内容</summary>
    /// <returns>baseはFailure</returns>
    protected virtual NodeState Act()
    {
        return NodeState.Failure;
    }
}

public class BTConditional : Node
{
    protected bool check = false;//条件
    public override NodeState Result()
    {
        //条件判断
        if (!check) return NodeState.Failure;

        return NodeState.Success;
    }
}

[System.Serializable]
public class BTSelector : Node
{
    [SerializeField]
    protected List<Node> m_childNodes = new List<Node>();
    public List<Node> ChildNodes => m_childNodes;

    public BTSelector(List<Node> childNodes)
    {
        m_childNodes = childNodes;
    }

    public override NodeState Result()
    {
        for (int i = 0; i < m_childNodes.Count; i++)
        {
            switch (m_childNodes[i].Result())
            {
                case NodeState.Running:
                    m_currentState = NodeState.Running;
                    return CurrentState;
                case NodeState.Success:
                    m_currentState = NodeState.Success;
                    return CurrentState;
                case NodeState.Failure:
                    continue;
                default:
                    break;
            }
        }
        m_currentState = NodeState.Failure;
        return CurrentState;
    }
}

[System.Serializable]
public class BTSequence : Node
{
    [SerializeField]
    protected List<Node> m_childNodes = new List<Node>();
    public List<Node> ChildNodes => m_childNodes;

    public BTSequence(List<Node> childNodes)
    {
        m_childNodes = childNodes;
    }

    public override NodeState Result()
    {
        bool m_isRun = false;
        for (int i = 0; i < m_childNodes.Count; i++)
        {
            switch (m_childNodes[i].Result())
            {
                case NodeState.Running:
                    m_isRun = true;
                    continue;
                case NodeState.Success:
                    continue;
                case NodeState.Failure:
                    m_currentState = NodeState.Failure;
                    return CurrentState;
                default:
                    break;
            }
        }
        if (m_isRun)
        {
            m_currentState = NodeState.Running;
            return CurrentState;
        }
        else
        {
            m_currentState = NodeState.Success;
            return CurrentState;
        }
    }
}

public class BTRepeater : Node
{
    protected Node m_child;
    public Node Child => m_child;

    public BTRepeater(Node child)
    {
        m_child = child;
    }

    public override NodeState Result()
    {
        if (m_currentState == NodeState.Running)
        {
            switch (m_child.Result())
            {
                case NodeState.Running:
                    m_currentState = NodeState.Running;
                    return CurrentState;
                case NodeState.Success:
                    m_currentState = NodeState.Success;
                    return CurrentState;
                case NodeState.Failure:
                    m_currentState = NodeState.Failure;
                    return CurrentState;
                default:
                    return CurrentState;
            }
        }
        else
        {
            return m_currentState;
        }
    }
}

//root
////repeater(≓update),
///sequence(順に実行し初めに失敗した時点でfalse,全て成功ならtrue)
///selector(順に実行し初めに成功した子をtrue,全て失敗ならfalse)
//////action(処理),conditional(条件判定)