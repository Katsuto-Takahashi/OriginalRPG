using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BCharacterStateMachine : MonoBehaviour
{
    public GameObject m_battlePanel = null;
    [SerializeField] PlayerControllerCC playerControllerCC = null;
    [SerializeField] PlayerControllerRB playerControllerRB = null;
    [SerializeField] float m_moveSpeed = 10f;
    public bool m_open = false;
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
    BattleDeadState battleDeadState = new BattleDeadState();

    [SerializeField] Animator animator = null;
    [SerializeField] HasSkillList hasSkillList = null;
    NewBattleManager battleManager;
    void OnEnable()
    {
        m_characterActionCount = 0;
        ChangeState(battleIdleState);
    }
    void Start()
    {
        battleManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<NewBattleManager>();
        //animator = GetComponent<Animator>();
        //hasSkillList = GetComponent<HasSkillList>();
    }

    void Update()
    {
        currentState.OnUpdate(this);

        Timer();
    }
    void ChangeState(BattleStateMachineBase nextState)
    {
        currentState?.OnExit(this);
        currentState = nextState;
        currentState.OnEnter(this);
    }
    void PlayAnimation(string stateName, float transitionDuration = 0.1f)
    {
        animator.CrossFadeInFixedTime(stateName, transitionDuration);
    }
    //void PlayPlayerAnimation(string stateName, float transitionDuration = 0.1f)
    //{
    //    animator.CrossFadeInFixedTime(stateName, transitionDuration);
    //}
    //void PlayEnemyAnimation(string stateName, float transitionDuration = 0.1f)
    //{
    //    animator.CrossFadeInFixedTime(stateName, transitionDuration);
    //}
    void Timer()
    {
        if (m_characterActionCount < 2 && currentState != battleMoveState)
        {
            m_countTimer -= Time.deltaTime;
        }
    }
    public void ChangeIdle()
    {
        ChangeState(battleIdleState);
        m_countTimer = 0;
    }
    public void ChangeDead()
    {
        ChangeState(battleDeadState);
    }
}
