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
        var playerID = GameManager.Instance.Player.BCSM.BattleID.Value;
        var battle = BattleManager.Instance;
        //BattleManager.Instance.SelectEnemy(m_selectedCommandNumber);
        battle.SelectTarget(m_selectedCommandNumber + battle.CharacterCount, playerID);
        base.CommandSelectedAction();
    }
}
