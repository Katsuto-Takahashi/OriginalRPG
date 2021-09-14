using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BattleStateMachine : MonoBehaviour
{
    [SerializeField] GameObject m_battlePanel = null;
    [SerializeField] PlayerControllerCC playerControllerCC = null;
    [SerializeField] PlayerControllerRB playerControllerRB = null;
    [SerializeField] float m_moveSpeed = 10f;
    private bool m_open = false;
    private int m_characterActionCount = 0;

    public float m_actionTimer;
    float m_countTimer;

    public bool m_battle = false;
    public bool m_action = false;
    private Vector3 m_targetPosition;
    private float m_distance = 0;
    public List<GameObject> m_targetCharacters = new List<GameObject>();
    public int m_targetNumber = 0;
    public bool m_firstAction = false;

    BattleStateMachineBase currentState;
    BattleIdleState battleIdleState = new BattleIdleState();
    BattleWaitActionState battleWaitActionState = new BattleWaitActionState();
    BattleMoveState battleMoveState = new BattleMoveState();
    BattleAttackState battleAttackState = new BattleAttackState();

    Animator animator;
    HasSkillList hasSkillList;
    BattleManager battleManager;
    void Start()
    {
        battleManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<BattleManager>();
        animator = GetComponent<Animator>();
        hasSkillList = GetComponent<HasSkillList>();

        ChangeState(battleIdleState);
    }

    void Update()
    {
        currentState.OnUpdate(this);

        Timer();
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
    void Timer()
    {
        if (m_characterActionCount < 2 && currentState != battleMoveState)
        {
            m_countTimer -= Time.deltaTime;
        }
    }
}
