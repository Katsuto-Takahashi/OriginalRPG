using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SkillUI : UIController
{
    [SerializeField]
    SkillType m_skillType;

    protected override void CommandSelectedAction()
    {
        var player = GameManager.Instance.Player;
        BattleManager.Instance.SelectSkill(m_selectedCommandNumber, m_skillType, player.HasSkillIndex, player.BCSM.BattleID.Value);
        base.CommandSelectedAction();
    }
}
