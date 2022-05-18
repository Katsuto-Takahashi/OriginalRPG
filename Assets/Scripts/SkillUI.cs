using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SkillUI : UIController
{
    [SerializeField]
    SkillData.SkillType m_skillType;

    protected override void CommandSelectedAction()
    {
        BattleManager.Instance.SelectSkill(m_selectedCommandNumber, m_skillType);
        base.CommandSelectedAction();
    }
}
