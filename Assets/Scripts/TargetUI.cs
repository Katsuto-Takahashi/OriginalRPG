using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetUI : UIController
{
    [SerializeField] BattleEnemyList m_battleEnemyList = null;

    protected override void StartSet()
    {
        base.StartSet();
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
    }
    protected override void GoToZero()
    {
        m_battleEnemyList.SetEnemy(m_commandList);
        base.GoToZero();
    }
    protected override void CommandSelectedAction()
    {
        //var player = GameObject.FindGameObjectWithTag("Player").GetComponent<BCharacterStateMachine>();
        //player.m_targetNumber = m_selectedCommandNumber;
        //player.m_action = true;
        BattleManager.Instance.SelectEnemy(m_selectedCommandNumber);

        GoToZero();
        gameObject.SetActive(false);
    }
}
