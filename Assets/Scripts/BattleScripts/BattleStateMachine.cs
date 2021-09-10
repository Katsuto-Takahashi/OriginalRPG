using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BattleStateMachine : MonoBehaviour
{
    [SerializeField] GameObject m_battlePanel = null;
    private bool m_open = false;
    private int m_characterActionCount = 0;

    public float m_actionTimer;
    float m_countTimer;
    public bool m_battle = false;

    private Vector3 m_targetPosition;
    public Vector3 m_currentPosition;
    public List<GameObject> m_targetCharacters = new List<GameObject>();
    public int m_targetNumber = 0;
    public bool m_firstAction = false;

    BattleStateMachineBase currentState;
    BattleIdleState battleIdleState = new BattleIdleState();
    BattleWaitActionState battleWaitActionState = new BattleWaitActionState();
    BattleAtatckState battleAtatckState = new BattleAtatckState();

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();

        ChangeState(battleIdleState);
    }

    void Update()
    {
        m_currentPosition = this.transform.position;
        currentState.OnUpdate(this);
    }
    void ChangeState(BattleStateMachineBase nextState)
    {
        currentState?.OnExit(this);
        nextState.OnEnter(this);
        currentState = nextState;
    }
    void PlayAnimation(string stateName, float transitionDuration = 0.1f)
    {
        animator.CrossFadeInFixedTime(stateName, transitionDuration);
    }
    internal static T GetRandom<T>(params T[] Params)
    {
        return Params[Random.Range(0, Params.Length)];
    }
}
