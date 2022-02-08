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

[System.Serializable]
public class Node : IReturnResult, IGetNode
{
    [SerializeReference, SubclassSelector]
    List<Node> m_childNode = new List<Node>();

    public List<Node> ChildNode => m_childNode;

    protected NodeState m_currentState;
    public NodeState CurrentState { get => m_currentState; }

    public virtual NodeState Result()
    {
        return m_currentState;
    }

    public virtual Node GetNode()
    {
        return this;
    }

    public List<Node> SetNode()
    {
        return m_childNode;
    }
}

public enum NodeState
{
    Running,
    Success,
    Failure
}

public class BTAction : Node
{
    public override NodeState Result()
    {
        return Act();
    }

    /// <summary>実行内容</summary>
    /// <returns>baseはFailure</returns>
    protected virtual NodeState Act()
    {
        return NodeState.Failure;
    }
}

public class BTConditional : Node
{
    public override NodeState Result()
    {
        return Check();
    }

    /// <summary>条件判定</summary>
    /// <returns>baseはFailure</returns>
    protected virtual NodeState Check()
    {
        return NodeState.Failure;
    }
}

public class BTSelector : Node
{
    protected List<Node> m_childNodes = new List<Node>();
    public List<Node> ChildNodes => m_childNodes;

    public BTSelector AddChild()
    {
        m_childNodes = ChildNode;
        return this;
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

//[System.Serializable]
public class BTSequence : Node
{
    //[SerializeField]
    protected List<Node> m_childNodes = new List<Node>();
    public List<Node> ChildNodes => m_childNodes;

    public BTSequence AddChild()
    {
        m_childNodes = ChildNode;
        return this;
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

[System.Serializable]
public class BTRepeater : Node
{
    protected List<Node> m_childNodes = new List<Node>();
    public List<Node> ChildNodes => m_childNodes;

    public BTRepeater AddChild()
    {
        m_childNodes = ChildNode;
        return this;
    }

    public override NodeState Result()
    {
        var m_node = m_childNodes[0];

        if (m_currentState == NodeState.Running)
        {
            switch (m_node.Result())
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

public class BTInverter: Node
{
    protected List<Node> m_childNodes = new List<Node>();
    public List<Node> ChildNodes => m_childNodes;

    public BTInverter AddChild()
    {
        m_childNodes = ChildNode;
        return this;
    }

    public override NodeState Result()
    {
        var child = m_childNodes[0];

        switch (child.Result())
        {
            case NodeState.Running:
                m_currentState = NodeState.Running;
                return CurrentState;
            case NodeState.Success:
                m_currentState = NodeState.Failure;
                return CurrentState;
            case NodeState.Failure:
                m_currentState = NodeState.Success;
                return CurrentState;
            default:
                return CurrentState;
        }
    }
}

//root
////Sequence(子を順に実行し初めに失敗した時点でfalse,全て成功ならtrue)
////Selector(子を順に実行し初めに成功した子をtrue,全て失敗ならfalse)
////Repeater(子を指定回数実行)
////Inverter(子を実行し失敗ならtrue,成功ならfalse)
////Conditional(条件判定)
////Action(処理)