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
        var skillIndex = GameManager.Instance.Player.HasSkillIndex;
        BattleManager.Instance.SelectSkill(m_selectedCommandNumber, m_skillType, skillIndex);
        base.CommandSelectedAction();
    }
}
